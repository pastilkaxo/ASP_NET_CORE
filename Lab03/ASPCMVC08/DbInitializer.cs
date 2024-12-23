using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ASPCMVC08.Data;

namespace ASPCMVC08.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            // Создание контекста базы данных
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();// Из DI-контейнера запрашивается объект UserManager. Если он не зарегистрирован, произойдёт ошибка.
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>(); // Из DI-контейнера запрашивается объект RoleManager.

            // Проверяем существует ли роль Administrator
            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                var role = new IdentityRole("Administrator");
                await roleManager.CreateAsync(role);
            }

            // Проверяем существует ли пользователь Administrator
            if (await userManager.FindByNameAsync("Administrator") == null)
            {
                var user = new IdentityUser { UserName = "Administrator", Email = "admin@example.com" };
                var result = await userManager.CreateAsync(user, "Obobad73"); 

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
            }
        }
    }
}