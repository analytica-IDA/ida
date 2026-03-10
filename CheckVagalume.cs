using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDbContext<AppDbContext>();

using var host = builder.Build();
using var scope = host.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

var vagalume = await context.Clientes.FirstOrDefaultAsync(c => c.Nome.Contains("Vagalume"));
if (vagalume == null) {
    Console.WriteLine("Client Vagalume not found.");
    return;
}

Console.WriteLine($"Vagalume Client ID: {vagalume.Id}");

var areas = await context.Areas.ToListAsync();
var investmentsMeta = await context.ClientesInvestimentosMeta.Where(i => i.IdCliente == vagalume.Id).ToListAsync();
var investmentsGoogle = await context.ClientesInvestimentosGoogle.Where(i => i.IdCliente == vagalume.Id).ToListAsync();

Console.WriteLine("\n--- Areas ---");
foreach (var area in areas) {
    Console.WriteLine($"Area ID: {area.Id}, Name: {area.Nome}");
}

Console.WriteLine("\n--- Meta Investments ---");
Console.WriteLine($"Total: {investmentsMeta.Count}, Orphans (no area): {investmentsMeta.Count(i => i.IdArea == null || i.IdArea == 0)}");
foreach (var inv in investmentsMeta) {
    Console.WriteLine($"ID: {inv.Id}, Area ID: {inv.IdArea}, Amount: {inv.VlrInvestimentoMeta}");
}

Console.WriteLine("\n--- Google Investments ---");
Console.WriteLine($"Total: {investmentsGoogle.Count}, Orphans (no area): {investmentsGoogle.Count(i => i.IdArea == null || i.IdArea == 0)}");
foreach (var inv in investmentsGoogle) {
    Console.WriteLine($"ID: {inv.Id}, Area ID: {inv.IdArea}, Amount: {inv.VlrInvestimentoGoogle}");
}
