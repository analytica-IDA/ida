using System;
using System.Linq;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static void Main()
    {
        var services = new ServiceCollection();
        string connString = "Host=localhost;Port=5434;Database=ida_db;Username=ida_admin;Password=ida_password";
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connString));
        
        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        var userRaw = context.Usuarios.FirstOrDefault(u => u.Login == "pedro.almeida");
        Console.WriteLine("User exists in table: " + (userRaw != null));

        var userJoined = context.Usuarios
            .Include(u => u.Pessoa)
            .Include(u => u.Cargo)
            .ThenInclude(c => c!.Role)
            .FirstOrDefault(u => u.Login == "pedro.almeida");
            
        Console.WriteLine("User resolved with Include: " + (userJoined != null));
        
        if (userRaw != null && userJoined == null)
        {
            Console.WriteLine("Cargo is null? " + (userRaw.IdCargo == 0 ? "YES" : "NO" + " (Cargo ID="" + userRaw.IdCargo + ")"));
            var pessoa = context.Pessoas.FirstOrDefault(p => p.Id == userRaw.Id);
            Console.WriteLine("Pessoa exists? " + (pessoa != null));
            var cargo = context.Cargos.FirstOrDefault(c => c.Id == userRaw.IdCargo);
            Console.WriteLine("Cargo exists? " + (cargo != null));
            if (cargo != null)
            {
                var role = context.Roles.FirstOrDefault(r => r.Id == cargo.IdRole);
                Console.WriteLine("Role exists? " + (role != null) + " (Role ID="" + cargo.IdRole + ")");
            }
        }
    }
}
