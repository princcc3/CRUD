using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Tekosol
{
    public class TokenRequirement : IAuthorizationRequirement { }
    public class TokenRequirementHandler : AuthorizationHandler<TokenRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenRequirement requirement)
        {
            var httpContext = this._contextAccessor.HttpContext;
            var token = httpContext.Request.Headers["Authorzation"].FirstOrDefault()?.Split(" ").Last();
            
            if(token == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            else
            {
                if(token == "thisisthetoken")
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            throw new NotImplementedException();
        }
        public TokenRequirementHandler(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;

        }
    }
}
