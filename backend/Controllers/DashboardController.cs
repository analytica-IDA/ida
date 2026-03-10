using Microsoft.AspNetCore.Mvc;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        private long GetCurrentRoleId() => long.Parse(User.FindFirst("roleId")?.Value ?? "0");
        private long GetCurrentClientId() => long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

        [HttpGet("accessible-models")]
        public async Task<IActionResult> GetAccessibleModels()
        {
            var roleId = GetCurrentRoleId();
            var idCliente = GetCurrentClientId();

            if (roleId == 1) // Admin
            {
                return Ok(new[] { "Cadastros", "Varejo", "Saúde" });
            }

            var accessibleModels = await _context.ClientesModelosControles
                .Where(cmc => cmc.IdCliente == idCliente)
                .Include(cmc => cmc.ModeloControle)
                .Select(cmc => cmc.ModeloControle!.Nome)
                .ToListAsync();

            return Ok(accessibleModels);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats([FromQuery] long? idCliente, [FromQuery] long? idArea)
        {
            var roleId = GetCurrentRoleId();
            var claimClientId = GetCurrentClientId();

            // Tenant boundary
            if (roleId != 1) idCliente = claimClientId;

            var usersQuery = _context.Usuarios.AsQueryable();
            var clientsQuery = _context.Clientes.AsQueryable();
            var personsQuery = _context.Pessoas.AsQueryable();

            if (idCliente.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.Pessoa!.IdCliente == idCliente.Value);
                clientsQuery = clientsQuery.Where(c => c.Id == idCliente.Value);
                personsQuery = personsQuery.Where(p => p.IdCliente == idCliente.Value);

                if (idArea.HasValue)
                {
                    var userIdsInArea = await _context.ClientesUsuarios
                        .Where(cu => cu.IdCliente == idCliente.Value && cu.IdArea == idArea.Value)
                        .Select(cu => cu.IdUsuario)
                        .ToListAsync();
                    usersQuery = usersQuery.Where(u => userIdsInArea.Contains(u.Id));
                    personsQuery = personsQuery.Where(p => p.IdCliente == idCliente.Value); // Persons are client-wide? or should we filter by creator? usually client-wide in stats
                }
            }

            var activeUsers = await usersQuery.CountAsync(u => u.FlAtivo);
            var partnerClients = await clientsQuery.CountAsync();
            var registeredPersons = await personsQuery.CountAsync();
            var reportsGenerated = await _context.LogsAuditoria.CountAsync(); // Audit logs usually global or we filter by user in client?

            return Ok(new
            {
                activeUsers,
                partnerClients,
                registeredPersons,
                reportsGenerated
            });
        }

        [HttpGet("varejo")]
        public async Task<IActionResult> GetVarejoStats([FromQuery] long? idCliente, [FromQuery] long? idArea, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var roleId = GetCurrentRoleId();
            var claimClientId = GetCurrentClientId();
            var currentUserId = long.Parse(User.FindFirst("id")?.Value ?? "0");

            if (roleId != 1) idCliente = claimClientId;

            // Base queries
            var varejoQuery = _context.LancamentosVarejo
                .Include(l => l.Usuario).ThenInclude(u => u!.Pessoa)
                .Include(l => l.ClienteInvestimentoMeta)
                .Include(l => l.ClienteInvestimentoGoogle)
                .Where(l => l.Usuario!.Pessoa!.IdCliente == idCliente)
                .AsQueryable();

            var invMetaQuery = _context.ClientesInvestimentosMeta.Where(i => i.IdCliente == idCliente && i.IdArea != null && i.IdArea > 0).AsQueryable();
            var invGoogleQuery = _context.ClientesInvestimentosGoogle.Where(i => i.IdCliente == idCliente && i.IdArea != null && i.IdArea > 0).AsQueryable();

            // Filter by Area - User based for launches, Area based for meta
            if (idArea.HasValue && idArea > 0)
            {
                var userIdsInArea = await _context.UsuariosAreas
                    .Where(ua => ua.IdArea == idArea.Value)
                    .Select(ua => ua.IdUsuario)
                    .ToListAsync();

                varejoQuery = varejoQuery.Where(l => 
                    (l.ClienteInvestimentoMeta != null && l.ClienteInvestimentoMeta.IdArea == idArea.Value) ||
                    (l.ClienteInvestimentoGoogle != null && l.ClienteInvestimentoGoogle.IdArea == idArea.Value) ||
                    ((l.IdClienteInvestimentoMeta == null || l.IdClienteInvestimentoMeta == 0) && 
                     (l.IdClienteInvestimentoGoogle == null || l.IdClienteInvestimentoGoogle == 0) && 
                     userIdsInArea.Contains(l.IdUsuario)));

                invMetaQuery = invMetaQuery.Where(i => i.IdArea == idArea.Value);
                invGoogleQuery = invGoogleQuery.Where(i => i.IdArea == idArea.Value);
            }

            // Role adjustment
            if (roleId == 4) varejoQuery = varejoQuery.Where(l => l.IdUsuario == currentUserId);

            // Date filtering for launches
            if (startDate.HasValue) varejoQuery = varejoQuery.Where(l => l.DataLancamento >= startDate.Value);
            if (endDate.HasValue) varejoQuery = varejoQuery.Where(l => l.DataLancamento <= endDate.Value.AddDays(1).AddTicks(-1));

            // Date filtering for investments (Month Overlap)
            if (startDate.HasValue)
            {
                var monthStart = new DateTime(startDate.Value.Year, startDate.Value.Month, 1);
                invMetaQuery = invMetaQuery.Where(i => i.DataReferencia >= monthStart);
                invGoogleQuery = invGoogleQuery.Where(i => i.DataReferencia >= monthStart);
            }
            if (endDate.HasValue)
            {
                var monthEnd = new DateTime(endDate.Value.Year, endDate.Value.Month, 1).AddMonths(1).AddTicks(-1);
                invMetaQuery = invMetaQuery.Where(i => i.DataReferencia <= monthEnd);
                invGoogleQuery = invGoogleQuery.Where(i => i.DataReferencia <= monthEnd);
            }

            // Cleanup orphaned investments for this client (no area)
            var orphansMeta = await _context.ClientesInvestimentosMeta.Where(i => i.IdCliente == idCliente && (i.IdArea == null || i.IdArea == 0)).ToListAsync();
            var orphansGoogle = await _context.ClientesInvestimentosGoogle.Where(i => i.IdCliente == idCliente && (i.IdArea == null || i.IdArea == 0)).ToListAsync();
            if (orphansMeta.Any() || orphansGoogle.Any())
            {
                if (orphansMeta.Any()) _context.ClientesInvestimentosMeta.RemoveRange(orphansMeta);
                if (orphansGoogle.Any()) _context.ClientesInvestimentosGoogle.RemoveRange(orphansGoogle);
                await _context.SaveChangesAsync();
            }

            var results = await varejoQuery.ToListAsync();
            var userIdsInResults = results.Select(r => r.IdUsuario).Distinct().ToList();
            var userAreaMap = await _context.ClientesUsuarios
                .Where(cu => userIdsInResults.Contains(cu.IdUsuario) && cu.IdCliente == idCliente)
                .Include(cu => cu.Area)
                .ToListAsync();

            // Detect and remove dirty records (launches that cannot be mapped to any area)
            var dirtyLaunchIds = results
                .Where(r => {
                    var areaId = r.ClienteInvestimentoMeta?.IdArea ?? r.ClienteInvestimentoGoogle?.IdArea;
                    if (areaId.HasValue) return false;
                    
                    return !userAreaMap.Any(ua => ua.IdUsuario == r.IdUsuario);
                })
                .Select(r => r.Id)
                .ToList();

            if (dirtyLaunchIds.Any())
            {
                var dirtyRecords = await _context.LancamentosVarejo.Where(l => dirtyLaunchIds.Contains(l.Id)).ToListAsync();
                _context.LancamentosVarejo.RemoveRange(dirtyRecords);
                await _context.SaveChangesAsync();
                
                results = results.Where(r => !dirtyLaunchIds.Contains(r.Id)).ToList();
                userIdsInResults = results.Select(r => r.IdUsuario).Distinct().ToList();
                userAreaMap = userAreaMap.Where(ua => userIdsInResults.Contains(ua.IdUsuario)).ToList();
            }

            var totalAtendimento = results.Sum(r => r.QtdAtendimento);
            var totalFechamento = results.Sum(r => r.QtdFechamento);
            var totalFaturamento = results.Sum(r => r.Faturamento);

            var totalInvestimentoMeta = await invMetaQuery.SumAsync(i => i.VlrInvestimentoMeta);
            var totalInvestimentoGoogle = await invGoogleQuery.SumAsync(i => i.VlrInvestimentoGoogle);
            var totalInvestimento = totalInvestimentoMeta + totalInvestimentoGoogle;

            // Investments per area for breakdown
            var metaInvDict = await invMetaQuery
                .GroupBy(i => i.IdArea)
                .Select(g => new { IdArea = g.Key ?? 0, Total = g.Sum(x => x.VlrInvestimentoMeta) })
                .ToDictionaryAsync(x => x.IdArea, x => x.Total);

            var googleInvDict = await invGoogleQuery
                .GroupBy(i => i.IdArea)
                .Select(g => new { IdArea = g.Key ?? 0, Total = g.Sum(x => x.VlrInvestimentoGoogle) })
                .ToDictionaryAsync(x => x.IdArea, x => x.Total);
            // Pre-load all necessary areas to avoid sync DB calls inside sync projections
            var areaIdsFromLaunches = results.Select(r => r.ClienteInvestimentoMeta?.IdArea ?? r.ClienteInvestimentoGoogle?.IdArea)
                .Concat(results.Select(r => userAreaMap.FirstOrDefault(ua => ua.IdUsuario == r.IdUsuario)?.IdArea))
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();

            var areaIdsNeeded = metaInvDict.Keys.Union(googleInvDict.Keys).Union(areaIdsFromLaunches).Distinct().ToList();
            var allAreaEntities = await _context.Areas
                .Where(a => areaIdsNeeded.Contains(a.Id))
                .ToListAsync();

            // Areas from launches
            var areasWithLaunches = results.GroupBy(r => {
                var areaId = r.ClienteInvestimentoMeta?.IdArea ?? r.ClienteInvestimentoGoogle?.IdArea;
                if (!areaId.HasValue) {
                    areaId = userAreaMap.FirstOrDefault(ua => ua.IdUsuario == r.IdUsuario)?.IdArea;
                }
                
                var areaNome = "Sem Área";
                if (areaId.HasValue) {
                    areaNome = userAreaMap.FirstOrDefault(ua => ua.IdArea == areaId.Value)?.Area?.Nome 
                               ?? allAreaEntities.FirstOrDefault(a => a.Id == areaId.Value)?.Nome 
                               ?? "Área Desconhecida";
                }

                return new { Id = areaId ?? 0, Nome = areaNome };
            })
            .Where(g => g.Key.Id > 0) // Filter out "Sem Área"
            .Select(g => new {
                Id = g.Key.Id,
                Name = g.Key.Nome,
                Faturamento = g.Sum(x => x.Faturamento),
                Atendimento = (long)g.Sum(x => x.QtdAtendimento),
                Fechamento = (long)g.Sum(x => x.QtdFechamento),
                Instagram = (long)g.Sum(x => x.QtdInstagram),
                Facebook = (long)g.Sum(x => x.QtdFacebook),
                Google = (long)g.Sum(x => x.QtdGoogle),
                Indicacao = (long)g.Sum(x => x.QtdIndicacao)
            }).ToList();


            var areaBreakdown = areasWithLaunches.Select(al => {
                decimal areaInvMeta = 0;
                decimal areaInvGoogle = 0;
                if (metaInvDict.TryGetValue(al.Id, out var m)) areaInvMeta = m;
                if (googleInvDict.TryGetValue(al.Id, out var go)) areaInvGoogle = go;

                var totalAreaInv = areaInvMeta + areaInvGoogle;

                return new {
                    Name = al.Name,
                    Faturamento = al.Faturamento,
                    Atendimento = al.Atendimento,
                    Fechamento = al.Fechamento,
                    Conversao = al.Atendimento > 0 ? (decimal)al.Fechamento / al.Atendimento : 0,
                    Investimento = totalAreaInv,
                    InvestimentoMeta = areaInvMeta,
                    InvestimentoGoogle = areaInvGoogle,
                    Roas = totalAreaInv > 0 ? al.Faturamento / totalAreaInv : 0,
                    Instagram = al.Instagram,
                    Facebook = al.Facebook,
                    Google = al.Google,
                    Indicacao = al.Indicacao
                };
            }).ToList();

            // Add areas that have investments but NO launches
            foreach (var areaId in metaInvDict.Keys.Union(googleInvDict.Keys))
            {
                if (!areasWithLaunches.Any(a => a.Id == areaId))
                {
                    decimal areaInvMeta = 0;
                    decimal areaInvGoogle = 0;
                    if (metaInvDict.TryGetValue(areaId, out var m)) areaInvMeta = m;
                    if (googleInvDict.TryGetValue(areaId, out var go)) areaInvGoogle = go;
                    
                    var areaName = allAreaEntities.FirstOrDefault(a => a.Id == areaId)?.Nome ?? "Área Desconhecida";

                    areaBreakdown.Add(new {
                        Name = areaName,
                        Faturamento = 0m,
                        Atendimento = 0L,
                        Fechamento = 0L,
                        Conversao = 0m,
                        Investimento = areaInvMeta + areaInvGoogle,
                        InvestimentoMeta = areaInvMeta,
                        InvestimentoGoogle = areaInvGoogle,
                        Roas = 0m,
                        Instagram = 0L,
                        Facebook = 0L,
                        Google = 0L,
                        Indicacao = 0L
                    });
                }
            }

            var sellerBreakdown = results.GroupBy(r => {
                var sellerName = r.Usuario?.Pessoa?.Nome ?? "Desconhecido";
                var areaId = r.ClienteInvestimentoMeta?.IdArea ?? r.ClienteInvestimentoGoogle?.IdArea;
                if (!areaId.HasValue) {
                    areaId = userAreaMap.FirstOrDefault(ua => ua.IdUsuario == r.IdUsuario)?.IdArea;
                }
                
                var areaNome = "";
                if (areaId.HasValue) {
                    areaNome = userAreaMap.FirstOrDefault(ua => ua.IdArea == areaId.Value)?.Area?.Nome 
                               ?? allAreaEntities.FirstOrDefault(a => a.Id == areaId.Value)?.Nome 
                               ?? "";
                }

                return new { SellerName = sellerName, AreaName = areaNome };
            })
            .Select(g => new {
                Name = g.Key.SellerName,
                Area = g.Key.AreaName,
                Faturamento = g.Sum(x => x.Faturamento),
                Atendimento = g.Sum(x => x.QtdAtendimento),
                Fechamento = g.Sum(x => x.QtdFechamento),
                Conversao = g.Sum(x => x.QtdAtendimento) > 0 ? (decimal)g.Sum(x => x.QtdFechamento) / g.Sum(x => x.QtdAtendimento) : 0
            }).ToList();

            return Ok(new
            {
                totalAtendimento,
                totalFechamento,
                totalFaturamento,
                totalInstagram = results.Sum(r => r.QtdInstagram),
                totalFacebook = results.Sum(r => r.QtdFacebook),
                totalGoogle = results.Sum(r => r.QtdGoogle),
                totalIndicacao = results.Sum(r => r.QtdIndicacao),
                totalInvestimentoMeta,
                totalInvestimentoGoogle,
                totalInvestimento,
                roas = totalInvestimento > 0 ? totalFaturamento / totalInvestimento : 0,
                conversionRate = totalAtendimento > 0 ? (decimal)totalFechamento / totalAtendimento : 0,
                areaBreakdown,
                sellerBreakdown
            });
        }

        [HttpGet("saude")]
        public async Task<IActionResult> GetSaudeStats([FromQuery] long? idCliente, [FromQuery] long? idArea, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var roleId = GetCurrentRoleId();
            var claimClientId = GetCurrentClientId();
            var currentUserId = long.Parse(User.FindFirst("id")?.Value ?? "0");

            if (roleId != 1) idCliente = claimClientId;

            var saudeQuery = _context.LancamentosSaude
                .Include(l => l.Usuario!.Pessoa)
                .Where(l => l.Usuario!.Pessoa!.IdCliente == idCliente)
                .AsQueryable();

            var invMetaQuery = _context.ClientesInvestimentosMeta.Where(i => i.IdCliente == idCliente && i.IdArea != null && i.IdArea > 0).AsQueryable();
            var invGoogleQuery = _context.ClientesInvestimentosGoogle.Where(i => i.IdCliente == idCliente && i.IdArea != null && i.IdArea > 0).AsQueryable();

            if (idArea.HasValue && idArea > 0)
            {
                var userIdsInArea = await _context.UsuariosAreas
                    .Where(ua => ua.IdArea == idArea.Value)
                    .Select(ua => ua.IdUsuario)
                    .ToListAsync();

                saudeQuery = saudeQuery.Where(l => 
                    (l.ClienteInvestimentoMeta != null && l.ClienteInvestimentoMeta.IdArea == idArea.Value) ||
                    (l.ClienteInvestimentoGoogle != null && l.ClienteInvestimentoGoogle.IdArea == idArea.Value) ||
                    ((l.IdClienteInvestimentoMeta == null || l.IdClienteInvestimentoMeta == 0) && 
                     (l.IdClienteInvestimentoGoogle == null || l.IdClienteInvestimentoGoogle == 0) && 
                     userIdsInArea.Contains(l.IdUsuario)));

                invMetaQuery = invMetaQuery.Where(i => i.IdArea == idArea.Value);
                invGoogleQuery = invGoogleQuery.Where(i => i.IdArea == idArea.Value);
            }

            if (roleId == 4) saudeQuery = saudeQuery.Where(l => l.IdUsuario == currentUserId);

            if (startDate.HasValue) saudeQuery = saudeQuery.Where(l => l.DataLancamento >= startDate.Value);
            if (endDate.HasValue) saudeQuery = saudeQuery.Where(l => l.DataLancamento <= endDate.Value.AddDays(1).AddTicks(-1));

            if (startDate.HasValue)
            {
                var monthStart = new DateTime(startDate.Value.Year, startDate.Value.Month, 1);
                invMetaQuery = invMetaQuery.Where(i => i.DataReferencia >= monthStart);
                invGoogleQuery = invGoogleQuery.Where(i => i.DataReferencia >= monthStart);
            }
            if (endDate.HasValue)
            {
                var monthEnd = new DateTime(endDate.Value.Year, endDate.Value.Month, 1).AddMonths(1).AddTicks(-1);
                invMetaQuery = invMetaQuery.Where(i => i.DataReferencia <= monthEnd);
                invGoogleQuery = invGoogleQuery.Where(i => i.DataReferencia <= monthEnd);
            }

            // Cleanup orphaned investments for this client (no area)
            var orphansMeta = await _context.ClientesInvestimentosMeta.Where(i => i.IdCliente == idCliente && (i.IdArea == null || i.IdArea == 0)).ToListAsync();
            var orphansGoogle = await _context.ClientesInvestimentosGoogle.Where(i => i.IdCliente == idCliente && (i.IdArea == null || i.IdArea == 0)).ToListAsync();
            if (orphansMeta.Any() || orphansGoogle.Any())
            {
                if (orphansMeta.Any()) _context.ClientesInvestimentosMeta.RemoveRange(orphansMeta);
                if (orphansGoogle.Any()) _context.ClientesInvestimentosGoogle.RemoveRange(orphansGoogle);
                await _context.SaveChangesAsync();
            }

            var results = await saudeQuery.ToListAsync();
            var userIdsInResults = results.Select(r => r.IdUsuario).Distinct().ToList();
            var userAreaMap = await _context.ClientesUsuarios
                .Where(cu => userIdsInResults.Contains(cu.IdUsuario) && cu.IdCliente == idCliente)
                .Include(cu => cu.Area)
                .ToListAsync();

            // Detect and remove dirty records
            var dirtyLaunchIds = results
                .Where(r => !userAreaMap.Any(ua => ua.IdUsuario == r.IdUsuario))
                .Select(r => r.Id)
                .ToList();

            if (dirtyLaunchIds.Any())
            {
                var dirtyRecords = await _context.LancamentosSaude.Where(l => dirtyLaunchIds.Contains(l.Id)).ToListAsync();
                _context.LancamentosSaude.RemoveRange(dirtyRecords);
                await _context.SaveChangesAsync();
                results = results.Where(r => !dirtyLaunchIds.Contains(r.Id)).ToList();
            }

            var totalClickMeta = results.Sum(r => r.QtdClickMeta);
            var totalClickGoogle = results.Sum(r => r.QtdClickGoogle);
            var totalContatosReais = results.Sum(r => r.QtdContatosReais);
            var totalConversaoConsultas = results.Sum(r => r.QtdConversaoConsultas);
            var totalEntradaRedesSociais = results.Sum(r => r.QtdEntradaRedesSociais);
            var totalEntradaGoogle = results.Sum(r => r.QtdEntradaGoogle);
            
            var totalFaturamento = results.Sum(l => l.QtdConversaoConsultas * (l.VlrTicketMedioConsultas ?? 0));

            var totalInvestimentoMeta = await invMetaQuery.SumAsync(i => i.VlrInvestimentoMeta);
            var totalInvestimentoGoogle = await invGoogleQuery.SumAsync(i => i.VlrInvestimentoGoogle);
            var totalInvestimento = totalInvestimentoMeta + totalInvestimentoGoogle;

            return Ok(new
            {
                totalClickMeta,
                totalClickGoogle,
                totalContatosReais,
                totalConversaoConsultas,
                totalEntradaRedesSociais,
                totalEntradaGoogle,
                totalFaturamento,
                totalInvestimentoMeta,
                totalInvestimentoGoogle,
                totalInvestimento,
                roas = totalInvestimento > 0 ? totalFaturamento / totalInvestimento : 0,
                conversionRate = (totalClickMeta + totalClickGoogle) > 0 ? (decimal)totalContatosReais / (totalClickMeta + totalClickGoogle) : 0
            });
        }

        [HttpGet("cadastro")]
        public async Task<IActionResult> GetCadastroStats([FromQuery] long? idCliente, [FromQuery] long? idArea, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var roleId = GetCurrentRoleId();
            var claimClientId = GetCurrentClientId();
            var currentUserId = long.Parse(User.FindFirst("id")?.Value ?? "0");

            if (roleId != 1) idCliente = claimClientId;

            var cadastroQuery = _context.LancamentosCadastro
                .Include(l => l.Usuario!.Pessoa)
                .Where(l => l.Usuario!.Pessoa!.IdCliente == idCliente)
                .AsQueryable();

            var invMetaQuery = _context.ClientesInvestimentosMeta.Where(i => i.IdCliente == idCliente && i.IdArea != null && i.IdArea > 0).AsQueryable();
            var invGoogleQuery = _context.ClientesInvestimentosGoogle.Where(i => i.IdCliente == idCliente && i.IdArea != null && i.IdArea > 0).AsQueryable();

            if (idArea.HasValue && idArea > 0)
            {
                var userIdsInArea = await _context.UsuariosAreas
                    .Where(ua => ua.IdArea == idArea.Value)
                    .Select(ua => ua.IdUsuario)
                    .ToListAsync();

                cadastroQuery = cadastroQuery.Where(l => 
                    (l.ClienteInvestimentoMeta != null && l.ClienteInvestimentoMeta.IdArea == idArea.Value) ||
                    (l.ClienteInvestimentoGoogle != null && l.ClienteInvestimentoGoogle.IdArea == idArea.Value) ||
                    ((l.IdClienteInvestimentoMeta == null || l.IdClienteInvestimentoMeta == 0) && 
                     (l.IdClienteInvestimentoGoogle == null || l.IdClienteInvestimentoGoogle == 0) && 
                     userIdsInArea.Contains(l.IdUsuario)));

                invMetaQuery = invMetaQuery.Where(i => i.IdArea == idArea.Value);
                invGoogleQuery = invGoogleQuery.Where(i => i.IdArea == idArea.Value);
            }

            if (roleId == 4) cadastroQuery = cadastroQuery.Where(l => l.IdUsuario == currentUserId);

            if (startDate.HasValue) cadastroQuery = cadastroQuery.Where(l => l.DataLancamento >= startDate.Value);
            if (endDate.HasValue) cadastroQuery = cadastroQuery.Where(l => l.DataLancamento <= endDate.Value.AddDays(1).AddTicks(-1));

            if (startDate.HasValue)
            {
                var monthStart = new DateTime(startDate.Value.Year, startDate.Value.Month, 1);
                invMetaQuery = invMetaQuery.Where(i => i.DataReferencia >= monthStart);
                invGoogleQuery = invGoogleQuery.Where(i => i.DataReferencia >= monthStart);
            }
            if (endDate.HasValue)
            {
                var monthEnd = new DateTime(endDate.Value.Year, endDate.Value.Month, 1).AddMonths(1).AddTicks(-1);
                invMetaQuery = invMetaQuery.Where(i => i.DataReferencia <= monthEnd);
                invGoogleQuery = invGoogleQuery.Where(i => i.DataReferencia <= monthEnd);
            }

            // Cleanup orphaned investments for this client (no area)
            var orphansMeta = await _context.ClientesInvestimentosMeta.Where(i => i.IdCliente == idCliente && (i.IdArea == null || i.IdArea == 0)).ToListAsync();
            var orphansGoogle = await _context.ClientesInvestimentosGoogle.Where(i => i.IdCliente == idCliente && (i.IdArea == null || i.IdArea == 0)).ToListAsync();
            if (orphansMeta.Any() || orphansGoogle.Any())
            {
                if (orphansMeta.Any()) _context.ClientesInvestimentosMeta.RemoveRange(orphansMeta);
                if (orphansGoogle.Any()) _context.ClientesInvestimentosGoogle.RemoveRange(orphansGoogle);
                await _context.SaveChangesAsync();
            }

            var results = await cadastroQuery.ToListAsync();
            var userIdsInResults = results.Select(r => r.IdUsuario).Distinct().ToList();
            var userAreaMap = await _context.ClientesUsuarios
                .Where(cu => userIdsInResults.Contains(cu.IdUsuario) && cu.IdCliente == idCliente)
                .Include(cu => cu.Area)
                .ToListAsync();

            // Detect and remove dirty records
            var dirtyLaunchIds = results
                .Where(r => !userAreaMap.Any(ua => ua.IdUsuario == r.IdUsuario))
                .Select(r => r.Id)
                .ToList();

            if (dirtyLaunchIds.Any())
            {
                var dirtyRecords = await _context.LancamentosCadastro.Where(l => dirtyLaunchIds.Contains(l.Id)).ToListAsync();
                _context.LancamentosCadastro.RemoveRange(dirtyRecords);
                await _context.SaveChangesAsync();
                results = results.Where(r => !dirtyLaunchIds.Contains(r.Id)).ToList();
            }

            var totalClickLink = results.Sum(r => r.QtdClickLink);
            var totalCadastros = results.Sum(r => r.QtdCadastros);
            var totalFaturamento = results.Sum(r => (decimal)(r.QtdCadastros * r.VlrTicketMedio));

            var totalInvestimentoMeta = await invMetaQuery.SumAsync(i => i.VlrInvestimentoMeta);
            var totalInvestimentoGoogle = await invGoogleQuery.SumAsync(i => i.VlrInvestimentoGoogle);
            var totalInvestimento = totalInvestimentoMeta + totalInvestimentoGoogle;

            return Ok(new
            {
                totalClickLink,
                totalCadastros,
                totalFaturamento,
                totalInvestimentoMeta,
                totalInvestimentoGoogle,
                totalInvestimento,
                roas = totalInvestimento > 0 ? totalFaturamento / totalInvestimento : 0,
                conversionRate = totalClickLink > 0 ? (decimal)totalCadastros / totalClickLink : 0,
                cpa = totalCadastros > 0 ? totalInvestimento / totalCadastros : 0
            });
        }
    }
}
