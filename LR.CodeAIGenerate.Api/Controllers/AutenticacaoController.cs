using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LR.CodeAIGenerate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AutenticacaoController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AutenticacaoController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Autentica o usuário e retorna um token JWT.
    /// </summary>
    /// <remarks>
    /// Neste exemplo, o usuário é validado de forma simplificada (apenas para fins de demonstração).
    /// Em produção, substitua por validação real (banco de dados, Identity, etc.).
    /// </remarks>
    /// <param name="request">Credenciais do usuário.</param>
    [HttpPost("token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<TokenResponse> GerarToken([FromBody] LoginRequest request)
    {
        // Validação simplificada: qualquer usuário com senha "123456" é aceito.
        // Ajuste conforme sua necessidade.
        if (string.IsNullOrWhiteSpace(request.Usuario) || request.Senha != "123456")
        {
            return Unauthorized();
        }

        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "LR.CodeAIGenerate";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "LR.CodeAIGenerate.Audience";
        var jwtSecret = _configuration["Jwt:Secret"] ?? "chave-super-secreta-nao-usar-em-producao-12345";
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, request.Usuario),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("scope", "pessoa.add"),
            new("scope", "endereco")
        };

        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(1);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        // Também expõe o token no header Authorization para conveniência do cliente
        Response.Headers["Authorization"] = $"Bearer {tokenString}";

        return Ok(new TokenResponse
        {
            AccessToken = tokenString,
            TokenType = "Bearer",
            ExpiresIn = (int)TimeSpan.FromHours(1).TotalSeconds
        });
    }

    public sealed class LoginRequest
    {
        /// <summary>
        /// Nome de usuário.
        /// </summary>
        public string Usuario { get; set; } = string.Empty;

        /// <summary>
        /// Senha.
        /// </summary>
        public string Senha { get; set; } = string.Empty;
    }

    public sealed class TokenResponse
    {
        /// <summary>
        /// Token de acesso JWT.
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do token (ex.: Bearer).
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Tempo de expiração em segundos.
        /// </summary>
        public int ExpiresIn { get; set; }
    }
}

