using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestCore.Models;
using GestCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthenticationService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserApp loginModel)
    {
        if (string.IsNullOrEmpty(loginModel.Name) || string.IsNullOrEmpty(loginModel.Password))
        {
            return BadRequest("Nome de usuário e senha são obrigatórios.");
        }

        var user = await _authService.AuthenticateUserAsync(loginModel.Name, loginModel.Password);
        if (user != null)
        {
            var token = GenerateToken(user); // Supondo que você tenha uma função para gerar o token JWT
            return Ok(new { Id = user.Id, UserName = user.Name, Token = token });
        }
        return Unauthorized("Credenciais inválidas.");
    }

    private string GenerateToken(UserApp user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]); // A chave secreta está armazenada em appsettings.json

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
                // Você pode adicionar mais claims se necessário
            }),
            Expires = DateTime.UtcNow.AddHours(1), // Token expira em 1 hora
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
