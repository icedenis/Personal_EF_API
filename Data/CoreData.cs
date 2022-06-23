using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Personal_EF_API.Data
{
    public static class CoreData
    {
        

        public async static Task Core(UserManager<IdentityUser> user_manager, RoleManager<IdentityRole> role_manager)
        {
            await CoreRoles(role_manager);
            await CoreUsers(user_manager);
        }

        private  async static Task CoreUsers(UserManager<IdentityUser> user_manager)
        {
            if(await user_manager.FindByNameAsync("admin@fh_aachen.de") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@fh_aachen.de"
                };
              var reuslt =  await user_manager.CreateAsync(user, "P@ssword1");

                if (reuslt.Succeeded)
                {
                    await user_manager.AddToRoleAsync(user, "Admin");
                }
            }

            if (await user_manager.FindByNameAsync("denis@fh_aachen.de") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "denis",
                    Email = "denis@fh_aachen.de"
                };
                var reuslt = await user_manager.CreateAsync(user, "P@ssword1");

                if (reuslt.Succeeded)
                {
                    await user_manager.AddToRoleAsync(user, "Personal");
                }
            }
            if (await user_manager.FindByNameAsync("test@fh_aachen.de") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "Test",
                    Email = "test@fh_aachen.de"
                };
                var reuslt = await user_manager.CreateAsync(user, "P@ssword1");

                if (reuslt.Succeeded)
                {
                    await user_manager.AddToRoleAsync(user, "Personal");
                }
            }


        }

        private  async static  Task CoreRoles(RoleManager<IdentityRole> role_manager)
        {
            if (!await role_manager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
                 await role_manager.CreateAsync(role);
            }
            if (!await role_manager.RoleExistsAsync("Personal"))
            {
                var role = new IdentityRole
                {
                    Name = "Personal"
                };
                await role_manager.CreateAsync(role);
            }

        }
    }
}
