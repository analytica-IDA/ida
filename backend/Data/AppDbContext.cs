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

            // Area
            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("area");
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.DtUltimaAtualizacao).HasDefaultValueSql("now()");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Areas)
                    .HasForeignKey(d => d.IdCliente);
            });

            // Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedNever(); // Important for PK-FK shared ID
                
                entity.Property(e => e.Login).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Senha).IsRequired().HasMaxLength(15);
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
            });
        }
    }
}
