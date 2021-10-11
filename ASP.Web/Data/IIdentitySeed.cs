using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.Web.Models;
using ASP.Web.Web.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ASP.Web.Data
{
    interface IIdentitySeed
    {
        Task Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<ApplicationSettings> options);
        Task Seed(UserManager<ApplicationUser> userManager, Func<RoleManager<IdentityRole>> getService);
        Task Seed(UserManager<ApplicationUser> userManager, Func<Microsoft.AspNetCore.Identity.RoleManager> getService);
    }
}
