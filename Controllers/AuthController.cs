using Microsoft.AspNetCore.Mvc;
using GestCore.Services;
using GestCore.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using IAuthenticationService = GestCore.Services.IAuthenticationService;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserApp loginModel)
    {
        if (string.IsNullOrEmpty(loginModel.Name) || string.IsNullOrEmpty(loginModel.Password))
        {
            return BadRequest("Nome de usuário e senha são obrigatórios.");
        }

        var (isAuthenticated, token) = await _authService.AuthenticateUserAsync(loginModel.Name, loginModel.Password);
        if (isAuthenticated)
        {
            return Ok(new { Token = token, UserName = loginModel.Name });
        }
        return Unauthorized("Credenciais inválidas.");
    }
}
