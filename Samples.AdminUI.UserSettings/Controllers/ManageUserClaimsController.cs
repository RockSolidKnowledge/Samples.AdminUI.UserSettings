using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Samples.AdminUI.UserSettings.Models;
using Samples.AdminUI.UserSettings.Services;

namespace Samples.AdminUI.UserSettings.Controllers
{
    [Authorize]
    public class ManageUserClaimsController : Controller
    {
        private readonly IManageUserClaimsService claimsService;

        public ManageUserClaimsController(IManageUserClaimsService claimsService)
        {
            this.claimsService = claimsService ?? throw new ArgumentNullException(nameof(claimsService));
        }
        
        [HttpGet]
        public async Task<IActionResult> UserClaims()
        {
            var model = new ManageUserClaimsModel
            {
                Claims = await claimsService.GetUserClaims()
            };

            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> UserClaims(ManageUserClaimsModel model)
        {
            await claimsService.UpdateUserClaims(model.Claims);

            return RedirectToAction("UserClaims");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}