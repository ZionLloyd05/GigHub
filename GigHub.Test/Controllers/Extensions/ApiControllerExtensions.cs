using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;

namespace GigHub.Test.Controllers.Extensions
{
    public static class ApiControllerExtensions
    {
        public static void MockCurrentUser(this ApiController controller, string userId, string username)
        {
            var identity = new GenericIdentity(username);
            identity.AddClaim(
                new Claim($"http://schemas.xmlsoap.org/ws/2005/05/identoty/claims/name", username));
            identity.AddClaim(
                new Claim($"http://schemas.xmlsoap.org/ws/2005/05/identoty/claims/nameidentifier", userId));
            
            var principal = new GenericPrincipal(identity, null);

            controller.User = principal;

        }
    }
}
