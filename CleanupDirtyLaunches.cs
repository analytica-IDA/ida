using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

using IHost host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    Console.WriteLine("Iniciando limpeza de lançamentos sem área...");

    var allVarejoResults = await context.LancamentosVarejo
        .Include(l => l.Usuario)
            .ThenInclude(u => u!.Pessoa)
        .ToListAsync();

    int deletedCount = 0;
    foreach (var launch in allVarejoResults)
    {
        var clientId = launch.Usuario?.Pessoa?.IdCliente;
        if (!clientId.HasValue) continue;

        var hasArea = await context.ClientesUsuarios
            .AnyAsync(cu => cu.IdUsuario == launch.IdUsuario && cu.IdCliente == clientId && cu.IdArea != null);

        if (!hasArea)
        {
            context.LancamentosVarejo.Remove(launch);
            deletedCount++;
        }
    }

    if (deletedCount > 0)
    {
        await context.SaveChangesAsync();
        Console.WriteLine($"Sucesso: {deletedCount} lançamentos removidos.");
    }
    else
    {
        Console.WriteLine("Nenhum lançamento sujo encontrado.");
    }
}
