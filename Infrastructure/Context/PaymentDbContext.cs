using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using PaymentService.Domain.ValueObjects;

namespace PaymentService.Infrastructure.Context
{
    public class PaymentDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; } 

        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Valor)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Ciclista)
                    .IsRequired();

                entity.Property(e => e.HoraSolicitacao)
                    .IsRequired();

                entity.Property(e => e.HoraFinalizacao);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasConversion<string>() 
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CreditCard>(entity =>
            {
                entity.HasKey(e => e.Id); 

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasMaxLength(19); 

                entity.Property(e => e.NomeTitular)
                    .IsRequired()
                    .HasMaxLength(50); 

                entity.Property(e => e.Validade)
                    .IsRequired()
                    .HasMaxLength(7); 

                entity.Property(e => e.Cvv)
                    .IsRequired()
                    .HasMaxLength(4); 
             });
        }
    }
}
