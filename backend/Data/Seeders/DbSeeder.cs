using System.Linq;
using backend.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Seeders
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // 1. Seed Roles
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(new List<Role>
                {
                    new Role { Nome = "admin" },
                    new Role { Nome = "proprietário" },
                    new Role { Nome = "supervisor" },
                    new Role { Nome = "vendedor" }
                });
                context.SaveChanges();
            }

            // 2. Seed Aplicacoes
            if (context.Aplicacoes.Count() <= 4) // Assuming initial seed had 4
            {
                var currentApps = context.Aplicacoes.Select(a => a.Nome).ToList();
                var newApps = new List<string> 
                { 
                    "Gerenciamento de Pessoas", 
                    "Gerenciamento de Clientes", 
                    "Gerenciamento de Cargos", 
                    "Gerenciamento de Áreas" 
                };

                foreach (var appName in newApps)
                {
                    if (!currentApps.Contains(appName))
                    {
                        context.Aplicacoes.Add(new Aplicacao { Nome = appName });
                    }
                }
                context.SaveChanges();
            }


            // 3. Link All Applications to Admin Role
            var adminRole = context.Roles.FirstOrDefault(r => r.Nome == "admin");
            if (adminRole != null)
            {
                var apps = context.Aplicacoes.ToList();
                var existingLinks = context.RolesAplicacoes
                    .Where(ra => ra.IdRole == adminRole.Id)
                    .Select(ra => ra.IdAplicacao)
                    .ToList();

                foreach (var app in apps)
                {
                    if (!existingLinks.Contains(app.Id))
                    {
                        context.RolesAplicacoes.Add(new RoleAplicacao { IdRole = adminRole.Id, IdAplicacao = app.Id });
                    }
                }
                context.SaveChanges();
            }

            // 4. Seed Admin User
            if (!context.Usuarios.Any(u => u.Login == "admin"))
            {
                // Create Default Cliente
                var defaultCliente = context.Clientes.FirstOrDefault(c => c.Nome == "Analytica IDA");
                if (defaultCliente == null)
                {
                    defaultCliente = new Cliente { 
                        Nome = "Analytica IDA", 
                        Cnpj = "00000000000000", 
                        Email = "suporte@analytica.com", 
                        Telefone = "00000000000" 
                    };
                    context.Clientes.Add(defaultCliente);
                    context.SaveChanges();
                }

                var adminCargo = context.Cargos.FirstOrDefault(c => c.Nome == "Administrador");
                if (adminCargo == null)
                {
                    adminCargo = new Cargo { Nome = "Administrador", IdRole = adminRole!.Id };
                    context.Cargos.Add(adminCargo);
                    context.SaveChanges();
                }

                var adminArea = context.Areas.FirstOrDefault(a => a.Nome == "Administração");
                if (adminArea == null)
                {
                    adminArea = new Area { Nome = "Administração", IdCliente = defaultCliente.Id };
                    context.Areas.Add(adminArea);
                    context.SaveChanges();
                }

                var adminPessoa = new Pessoa { 
                    Nome = "Paulo Takeda", 
                    Email = "paulo.takeda@gmail.com", 
                    Cpf = "00000000000", 
                    Telefone = "00000000000" 
                };
                context.Pessoas.Add(adminPessoa);
                context.SaveChanges();

                var adminUser = new Usuario
                {
                    Id = adminPessoa.Id,
                    Login = "admin",
                    Senha = "admin",
                    IdCargo = adminCargo.Id,
                    IdArea = adminArea.Id,
                    FlAtivo = true
                };
                context.Usuarios.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
}
