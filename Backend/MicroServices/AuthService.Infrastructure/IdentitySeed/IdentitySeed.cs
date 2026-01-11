using AuthService.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.IdentitySeed;

public static class IdentitySeed
{
    public static async Task SeedSuperAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<nguoi_dung>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        const string roleName = "SuperAdmin";
        const string userName = "admin";
        const string password = "admin"; // đổi khi lên production

        // ===== 1. Tạo role nếu chưa có =====
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Name = roleName
            });
        }

        // ===== 2. Tạo user nếu chưa có =====
        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
            user = new nguoi_dung
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                ho_ten = "Super",
                ten_dem = "Admin",
                ten_day_du = "Super Admin",
                Email = "superadmin@system.local",
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                throw new Exception(string.Join(" | ",
                    createResult.Errors.Select(e => e.Description)));
            }
        }

        // ===== 3. Gán role cho user =====
        if (!await userManager.IsInRoleAsync(user, roleName))
        {
            await userManager.AddToRoleAsync(user, roleName);
        }
    }
}
