using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Samples.AdminUI.UserSettings.Controllers;
using Samples.AdminUI.UserSettings.Models;
using Samples.AdminUI.UserSettings.Services;
using Xunit;

namespace Samples.AdminUI.UserSettings.Tests.Unit.Controllers
{
    public class ManageUserClaimsControllerTests
    {
        private readonly Mock<IManageUserClaimsService> userClaimsService;
        private readonly ManageUserClaimsController controller;

        public ManageUserClaimsControllerTests()
        {
            userClaimsService = new Mock<IManageUserClaimsService>();

            controller = new ManageUserClaimsController(userClaimsService.Object);
        }

        [Fact]
        public async Task UserClaims_OnGet_CallsGetUserClaimsOnClaimsServiceAndReturnsView()
        {
            var claim = new ClaimDto("type", "value");

            var claims = new List<ClaimDto>
            {
                claim
            };

            userClaimsService.Setup(x => x.GetUserClaims()).ReturnsAsync(claims);

            var result = await controller.UserClaims() as ViewResult;

            var model = result.ViewData.Model as ManageUserClaimsModel;

            model.Claims.Should().Contain(x => x.Type == claim.Type && x.Value == claim.Value);
        }

        [Fact]
        public async Task UserClaims_OnPostWithModel_CallsUpdateUserClaimsServiceAndReturnsView()
        {
            var claim = new ClaimDto("type", "value");

            var claims = new List<ClaimDto>
            {
                claim
            };

            var expectedModel = new ManageUserClaimsModel
            {
                Claims = claims
            };

            var result = await controller.UserClaims(expectedModel) as RedirectToActionResult;

            result.ActionName.Should().Be("UserClaims");
        }
    }
}
