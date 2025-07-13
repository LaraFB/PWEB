using GestaoLojaTP.Data;
using Microsoft.AspNetCore.Identity;

namespace GestaoLojaTP.Data
{
    public static class Inicializacao
    {
        public static async Task CriaDadosIniciais(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Adicionar três perfis/roles 
            string[] roles = ["Admin", "Gestor", "Cliente"];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    IdentityRole roleRole = new IdentityRole(role);
                    await roleManager.CreateAsync(roleRole);
                }
            }

            // Adicionar dois utilizadores com role de Admin
            var admin1 = new ApplicationUser
            {
                UserName = "Admin+01@mail.pt",
                Email = "admin01@mail.pt",
                Nome = "Admin",
                Apelido = "01",
                PhoneNumber = "123456789",
                NIF = 123456789,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var admin2 = new ApplicationUser
            {
                UserName = "Admin+02@mail.pt",
                Email = "admin02@mail.pt",
                Nome = "Admin",
                Apelido = "02",
                PhoneNumber = "987654321",
                NIF = 987654321,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var admin3 = new ApplicationUser
            {
                UserName = "Admin+03@mail.pt",
                Email = "admin03@mail.pt",
                Nome = "Admin",
                Apelido = "03",
                PhoneNumber = "987654322",
                NIF = 987654322,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != admin1.Id))
            {
                var user1 = await userManager.FindByEmailAsync(admin1.Email);

                if (user1 == null)
                {
                    await userManager.CreateAsync(admin1, "Admin+01"); // password
                    await userManager.AddToRoleAsync(admin1, "Admin"); // role
                }
            }

            if (userManager.Users.All(u => u.Id != admin2.Id))
            {
                var user2 = await userManager.FindByEmailAsync(admin2.Email);

                if (user2 == null)
                {
                    await userManager.CreateAsync(admin2, "Admin+02"); // password
                    await userManager.AddToRoleAsync(admin2, "Admin"); // role
                }
            }

            if (userManager.Users.All(u => u.Id != admin3.Id))
            {
                var user3 = await userManager.FindByEmailAsync(admin3.Email);

                if (user3 == null)
                {
                    await userManager.CreateAsync(admin3, "Admin+03"); // password
                    await userManager.AddToRoleAsync(admin3, "Admin"); // role
                }
            }

            var gestor1 = new ApplicationUser
            {
                UserName = "Gestor+01@mail.pt",
                Email = "gestor01@mail.pt",
                Nome = "Gestor",
                Apelido = "01",
                PhoneNumber = "987654321",
                NIF = 987654321,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var gestor2 = new ApplicationUser
            {
                UserName = "Gestor+02@mail.pt",
                Email = "gestor02@mail.pt",
                Nome = "Gestor",
                Apelido = "02",
                PhoneNumber = "123456789",
                NIF = 123456789,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != gestor1.Id))
            {
                var user3 = await userManager.FindByEmailAsync(gestor1.Email);

                if (user3 == null)
                {
                    await userManager.CreateAsync(gestor1, "Gestor+01"); // password
                    await userManager.AddToRoleAsync(gestor1, "Gestor"); // role
                }
            }

            if (userManager.Users.All(u => u.Id != gestor2.Id))
            {
                var user4 = await userManager.FindByEmailAsync(gestor2.Email);

                if (user4 == null)
                {
                    await userManager.CreateAsync(gestor2, "Gestor+02"); // password
                    await userManager.AddToRoleAsync(gestor2, "Gestor"); // role
                }
            }
        }
    }
}