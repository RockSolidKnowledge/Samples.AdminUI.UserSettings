using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Samples.AdminUI.UserSettings.Models;

namespace Samples.AdminUI.UserSettings.Services
{
    public interface IManageUserClaimsService
    {
        Task<List<ClaimDto>> GetUserClaims();
        Task UpdateUserClaims(List<ClaimDto> updatedClaims);
    }

    public class ManageUserClaimsService : IManageUserClaimsService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ManageUserClaimsOptions options;

        public ManageUserClaimsService(IHttpContextAccessor httpContextAccessor, IOptions<ManageUserClaimsOptions> options)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<List<ClaimDto>> GetUserClaims()
        {
            var client = new HttpClient();

            // get the access token from the user session
            var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            // add access token to request as bearer authorization header
            client.SetBearerToken(accessToken);

            // request user claims from usersettings endpoint
            var getUserClaimsUrl = $"{options.AdminUIApiUrl}/UserSettings/{GetUserSubject()}";
            var response = await client.GetStringAsync(getUserClaimsUrl);

            // deserialize result and return users claims
            var responseAsModel = JsonConvert.DeserializeObject<UserClaimsDto>(response);
            return responseAsModel.Claims;
        }

        public async Task UpdateUserClaims(List<ClaimDto> updatedClaims)
        {
            var client = new HttpClient();

            // get the access token from the user session
            var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            // add access token to request as bearer authorization header
            client.SetBearerToken(accessToken);

            // serialize request body
            var body = JsonConvert.SerializeObject(new UserClaimsDto
            {
                Subject = GetUserSubject(),
                Claims = updatedClaims
            });

            // update users claims
            var result = await client.PutAsync($"{options.AdminUIApiUrl}/UserSettings", new StringContent(body, Encoding.UTF8, "application/json"));

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Update user claims failed");
            }
        }

        private string GetUserSubject()
        {
            var subject = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject)?.Value;

            if (subject == null)
            {
                throw new Exception("No user subject found");
            }

            return subject;
        }
    }
}