using ASP.Web.Models;
using ASP.Web.Web.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Web.Data
{
    public class IdentitySeed : IIdentitySeed
    {
        public async Task Seed(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,IOptions<ApplicationSettings> options){
            // Get all comma-separated roles
            var roles = options.Value.Roles.Split(new char[] { ',' });
            // create roles if they dont exist
            foreach(var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    IdentityRole storageRole = new IdentityRole
                    {
                        Name = role
                    };
                    IdentityResult roleResult = await roleManager.CreateAsync(storageRole);
                }
            }
            // create admin i fhe doesnt exist
            var admin = await userManager.FindByEmailAsync(options.Value.AdminEmail);
            if(admin == null){
                ApplicationUser user = new ApplicationUser
                {
                    UserName = options.Value.AdminName,
                    Email = options.Value.AdminEmail,
                    EmailConfirmed = true
                };
                IdentityResult result = await userManager.CreateAsync(user, options.Value.AdminPassword);
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/chaims/emailaddress", options.Value.AdminEmail));
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("IsActive", "True"));
                // add admin to admin roles
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, "Admin");
                }
            }
        }

        public Task Seed(UserManager<ApplicationUser> userManager, Func<RoleManager<IdentityRole>> getService)
        {
            throw new NotImplementedException();
        }

        Task IIdentitySeed.Seed(UserManager<ApplicationUser> userManager, Func<Microsoft.AspNetCore.Identity.RoleManager> getService)
        {
            throw new NotImplementedException();
        }
    }
}
