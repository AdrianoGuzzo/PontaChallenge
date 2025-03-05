using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ApiTask
{
    public class JwtUserIdFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Obtém o JWT do cabeçalho Authorization
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedResult(); // Retorna 401 se o token não estiver presente ou for inválido
                return;
            }

            // Remove o prefixo "Bearer " do token
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            // Parse o JWT
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                context.Result = new UnauthorizedResult(); // Retorna 401 se o token for inválido
                return;
            }

            // Obtenha o valor do claim "sub" (ID do usuário)
            var userId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (userId == null)
            {
                context.Result = new UnauthorizedResult(); // Retorna 401 se o claim "sub" não for encontrado
                return;
            }

            // Armazene o ID do usuário no contexto da requisição para uso nas ações
            context.HttpContext.Items["UserId"] = userId;

            base.OnActionExecuting(context);
        }
    }
}
