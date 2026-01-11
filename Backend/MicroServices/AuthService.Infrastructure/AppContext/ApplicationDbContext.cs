using AuthService.Domain.BaseEntity;
using AuthService.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.AppContext;

public class ApplicationDbContext : IdentityDbContext<nguoi_dung, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<refresh_token> Refresh_Tokens { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(AuditableBaseEntity).IsAssignableFrom(entityType.ClrType)
                && entityType.ClrType != typeof(nguoi_dung))
            {
                builder.Entity(entityType.ClrType)
                    .HasOne(typeof(nguoi_dung), nameof(AuditableBaseEntity.nguoi_tao))
                    .WithMany()
                    .HasForeignKey(nameof(AuditableBaseEntity.nguoi_tao_id))
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity(entityType.ClrType)
                    .HasOne(typeof(nguoi_dung), nameof(AuditableBaseEntity.nguoi_chinh_sua))
                    .WithMany()
                    .HasForeignKey(nameof(AuditableBaseEntity.nguoi_chinh_sua_id))
                    .OnDelete(DeleteBehavior.NoAction);
            }
        }
        builder.Entity<nguoi_dung>(e =>
        {
            e.ToTable("nguoi_dung");
            e.HasOne(e => e.nguoi_chinh_sua)
             .WithMany()
             .HasForeignKey(e => e.nguoi_chinh_sua_id)
             .OnDelete(DeleteBehavior.NoAction);
            e.HasOne(e => e.nguoi_tao)
             .WithMany()
             .HasForeignKey(e => e.nguoi_tao_id)
             .OnDelete(DeleteBehavior.NoAction);
            e.HasMany(x => x.Refresh_Tokens)
             .WithOne(x => x.nguoi_dung)
             .HasForeignKey(x => x.nguoi_dung_id)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<IdentityRole<Guid>>(b =>
        {
            b.ToTable("vai_tro");
        });

        builder.Entity<IdentityUserRole<Guid>>(b =>
        {
            b.ToTable("nguoi_dung_vai_tro");
        });

        builder.Entity<IdentityUserClaim<Guid>>(b =>
        {
            b.ToTable("nguoi_dung_claim");
        });

        builder.Entity<IdentityUserLogin<Guid>>(b =>
        {
            b.ToTable("nguoi_dung_login");
        });

        builder.Entity<IdentityRoleClaim<Guid>>(b =>
        {
            b.ToTable("vai_tro_claim");
        });

        builder.Entity<IdentityUserToken<Guid>>(b =>
        {
            b.ToTable("nguoi_dung_token");
        });
        builder.Entity<refresh_token>(e =>
        {
            e.ToTable("refresh_token");
            e.HasKey(e => e.Id);
            e.HasIndex(x => x.Token).IsUnique();
            e.HasOne(x => x.nguoi_dung)
             .WithMany(y => y.Refresh_Tokens)
             .HasForeignKey(x => x.nguoi_dung_id)
             .OnDelete(DeleteBehavior.Cascade);
        });

    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.ngay_tao = DateTime.Now;
                    entry.Entity.ngay_chinh_sua = DateTime.Now;
                    entry.Entity.nguoi_chinh_sua_id = Guid.Empty;
                    entry.Entity.nguoi_tao_id = Guid.Empty;
                    break;
                case EntityState.Modified:
                    entry.Entity.ngay_tao = DateTime.Now;
                    entry.Entity.ngay_chinh_sua = DateTime.Now;
                    entry.Entity.nguoi_chinh_sua_id = Guid.Empty;
                    entry.Entity.nguoi_tao_id = Guid.Empty;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
