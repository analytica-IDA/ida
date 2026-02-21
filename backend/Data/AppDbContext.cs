using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Aplicacao> Aplicacoes { get; set; }
        public DbSet<RoleAplicacao> RolesAplicacoes { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<ClienteCargo> ClientesCargos { get; set; }
        public DbSet<CargoArea> CargosAreas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ClienteUsuario> ClientesUsuarios { get; set; }
        public DbSet<LogAuditoria> LogsAuditoria { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ida");

            // Notificacao
            modelBuilder.Entity<Notificacao>(entity =>
            {
                entity.ToTable("notificacao");
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Mensagem).IsRequired();
                entity.Property(e => e.DtUltimaAtualizacao).HasDefaultValueSql("now()");
                
                entity.HasOne(d => d.UsuarioDestino)
                    .WithMany()
                    .HasForeignKey(d => d.IdUsuarioDestino);
            });

            // LogAuditoria
            modelBuilder.Entity<LogAuditoria>(entity =>
            {
                entity.ToTable("log_auditoria");
                entity.Property(e => e.Usuario).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Acao).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Tabela).IsRequired().HasMaxLength(100);
            });

            // Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.DtUltimaAtualizacao).HasDefaultValueSql("now()");
            });

            // Aplicacao
            modelBuilder.Entity<Aplicacao>(entity =>
            {
                entity.ToTable("aplicacao");
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.DtUltimaAtualizacao).HasDefaultValueSql("now()");
            });

            // RoleAplicacao
            modelBuilder.Entity<RoleAplicacao>(entity =>
            {
                entity.ToTable("role_aplicacao");
                entity.Property(e => e.DtUltimaAtualizacao).HasDefaultValueSql("now()");
                
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RolesAplicacoes)
                    .HasForeignKey(d => d.IdRole);

                entity.HasOne(d => d.Aplicacao)
                    .WithMany(p => p.RolesAplicacoes)
                    .HasForeignKey(d => d.IdAplicacao);
            });

            // Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("cliente");
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Cnpj).IsRequired().HasMaxLength(14);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Telefone).IsRequired().HasMaxLength(14);
                entity.Property(e => e.DtUltimaAtualizacao).HasDefaultValueSql("now()");
            });

            // Pessoa
            modelBuilder.Entity<Pessoa>(entity =>
            {
                entity.ToTable("pessoa");
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Cpf).IsRequired().HasMaxLength(11);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Telefone).IsRequired().HasMaxLength(14);
                entity.Property(e => e.DtUltimaAtualizacao).HasDefaultValueSql("now()");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Pessoas)
                    .HasForeignKey(d => d.IdCliente);
            });

            // Cargo
            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.ToTable("cargo");
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.DtUltimaAtualizacao).HasDefaultValueSql("now()");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Cargos)
                    .HasForeignKey(d => d.IdRole);
            });

            // ClienteCargo
            modelBuilder.Entity<ClienteCargo>(entity =>
            {
                entity.ToTable("cliente_cargo");
                entity.Property(e => e.DtUltimaAtualizacao).HasDefaultValueSql("now()");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.ClientesCargos)
                    .HasForeignKey(d => d.IdCliente);

                entity.HasOne(d => d.Cargo)
                    .WithMany(p => p.ClientesCargos)
                    .HasForeignKey(d => d.IdCargo);
            });

            // CargoArea
            modelBuilder.Entity<CargoArea>(entity =>
            {
                entity.ToTable("cargo_area");
                entity.Property(e => e.DtUltimaAtualizacao).HasDefaultValueSql("now()");

                entity.HasOne(d => d.Cargo)
                    .WithMany(p => p.CargosAreas)
                    .HasForeignKey(d => d.IdCargo);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.CargosAreas)
                    .HasForeignKey(d => d.IdArea);
            });

            // Area
            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("area");
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.DtUltimaAtualizacao).HasDefaultValueSql("now()");
            });

            // Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedNever(); // Important for PK-FK shared ID
                
                entity.Property(e => e.Login).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Senha).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FlAtivo).HasDefaultValue(true);
                entity.Property(e => e.DtUltimaAtualizacao).HasDefaultValueSql("now()");

                entity.HasOne(u => u.Pessoa)
                    .WithOne(p => p.Usuario)
                    .HasForeignKey<Usuario>(d => d.Id);

                entity.HasOne(d => d.Cargo)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdCargo);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdArea);
            });

            // ClienteUsuario
            modelBuilder.Entity<ClienteUsuario>(entity =>
            {
                entity.ToTable("cliente_usuario");
                entity.Property(e => e.DtUltimaAtualizacao).HasDefaultValueSql("now()");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.ClientesUsuarios)
                    .HasForeignKey(d => d.IdCliente);

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.ClientesUsuarios)
                    .HasForeignKey(d => d.IdUsuario);

                entity.HasOne(d => d.Area)
                    .WithMany()
                    .HasForeignKey(d => d.IdArea);
            });

            // Seed Data
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Nome = "admin" },
                new Role { Id = 2, Nome = "proprietário" },
                new Role { Id = 3, Nome = "supervisor" },
                new Role { Id = 4, Nome = "vendedor" }
            );

            // "Página Inicial", "Dashboard", "Gerenciamento de Cliente", "Gerenciamento de Pessoa", "Gerenciamento de Cargo", "Gerenciamento de Área", "Gerenciamento de Usuário", "Relatórios", "Configurações"
            modelBuilder.Entity<Aplicacao>().HasData(
                new Aplicacao { Id = 1, Nome = "Página Inicial" },
                new Aplicacao { Id = 2, Nome = "Dashboard" },
                new Aplicacao { Id = 3, Nome = "Gerenciamento de Cliente" },
                new Aplicacao { Id = 4, Nome = "Gerenciamento de Pessoa" },
                new Aplicacao { Id = 5, Nome = "Gerenciamento de Cargo" },
                new Aplicacao { Id = 6, Nome = "Gerenciamento de Área" },
                new Aplicacao { Id = 7, Nome = "Gerenciamento de Usuário" },
                new Aplicacao { Id = 8, Nome = "Relatórios" },
                new Aplicacao { Id = 9, Nome = "Configurações" }
            );

            modelBuilder.Entity<RoleAplicacao>().HasData(
                // Admin tem acesso a todos
                new RoleAplicacao { Id = 1, IdRole = 1, IdAplicacao = 1 },
                new RoleAplicacao { Id = 2, IdRole = 1, IdAplicacao = 2 },
                new RoleAplicacao { Id = 3, IdRole = 1, IdAplicacao = 3 },
                new RoleAplicacao { Id = 4, IdRole = 1, IdAplicacao = 4 },
                new RoleAplicacao { Id = 5, IdRole = 1, IdAplicacao = 5 },
                new RoleAplicacao { Id = 6, IdRole = 1, IdAplicacao = 6 },
                new RoleAplicacao { Id = 7, IdRole = 1, IdAplicacao = 7 },
                new RoleAplicacao { Id = 8, IdRole = 1, IdAplicacao = 8 },
                new RoleAplicacao { Id = 9, IdRole = 1, IdAplicacao = 9 },
                
                // Proprietário: Página Inicial(1), Dashboard(2), Pessoa(4), Cargo(5), Área(6), Usuário(7), Relatórios(8)
                new RoleAplicacao { Id = 10, IdRole = 2, IdAplicacao = 1 },
                new RoleAplicacao { Id = 11, IdRole = 2, IdAplicacao = 2 },
                new RoleAplicacao { Id = 12, IdRole = 2, IdAplicacao = 4 },
                new RoleAplicacao { Id = 13, IdRole = 2, IdAplicacao = 5 },
                new RoleAplicacao { Id = 14, IdRole = 2, IdAplicacao = 6 },
                new RoleAplicacao { Id = 15, IdRole = 2, IdAplicacao = 7 },
                new RoleAplicacao { Id = 16, IdRole = 2, IdAplicacao = 8 },
                
                // Supervisor: Página Inicial(1), Dashboard(2), Pessoa(4), Usuário(7), Relatórios(8)
                new RoleAplicacao { Id = 17, IdRole = 3, IdAplicacao = 1 },
                new RoleAplicacao { Id = 18, IdRole = 3, IdAplicacao = 2 },
                new RoleAplicacao { Id = 19, IdRole = 3, IdAplicacao = 4 },
                new RoleAplicacao { Id = 20, IdRole = 3, IdAplicacao = 7 },
                new RoleAplicacao { Id = 21, IdRole = 3, IdAplicacao = 8 },
                
                // Vendedor: Página Inicial(1), Dashboard(2), Relatórios(8)
                new RoleAplicacao { Id = 22, IdRole = 4, IdAplicacao = 1 },
                new RoleAplicacao { Id = 23, IdRole = 4, IdAplicacao = 2 },
                new RoleAplicacao { Id = 24, IdRole = 4, IdAplicacao = 8 }
            );

            modelBuilder.Entity<Cliente>().HasData(
                new Cliente { Id = 1, Nome = "Analytica IDA", Cnpj = "00000000000000", Email = "suporte@analytica.com", Telefone = "00000000000" }
            );

            modelBuilder.Entity<Cargo>().HasData(
                new Cargo { Id = 1, Nome = "Administrador", IdRole = 1 }
            );

            modelBuilder.Entity<Area>().HasData(
                new Area { Id = 1, Nome = "Administração" }
            );

            modelBuilder.Entity<ClienteCargo>().HasData(
                new ClienteCargo { Id = 1, IdCliente = 1, IdCargo = 1 }
            );

            modelBuilder.Entity<CargoArea>().HasData(
                new CargoArea { Id = 1, IdCargo = 1, IdArea = 1 }
            );

            modelBuilder.Entity<Pessoa>().HasData(
                new Pessoa { Id = 1, Nome = "Administrador", Cpf = "00000000000", Email = "suporte@analytica.com", Telefone = "00000000000", IdCliente = 1 }
            );

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Login = "admin", Senha = BCrypt.Net.BCrypt.HashPassword("T4k3d4@@!"), IdCargo = 1, IdArea = 1, FlAtivo = true }
            );
        }
    }
}
