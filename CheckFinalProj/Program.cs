using System;
using System.Linq;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BCrypt.Net;

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
        
        var user = context.Usuarios.FirstOrDefault(x => x.Login == "pedro.almeida");
        if(user != null) {
            Console.WriteLine("Login: " + user.Login);
            Console.WriteLine("Hash: " + user.Senha);
            Console.WriteLine("Hash Length: " + user.Senha?.Length);
            Console.WriteLine("Valid abc123: " + BCrypt.Net.BCrypt.Verify("abc123", user.Senha));
            Console.WriteLine("FlAtivo: " + user.FlAtivo);
        } else {
            Console.WriteLine("User pedro.almeida not found.");
        }
    }
}
