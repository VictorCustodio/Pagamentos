/*using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PaymentService.Infrastructure.Context
{
    public class PaymentDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }

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
                    .HasMaxLength(50);
            });
        }
    }
}
*/