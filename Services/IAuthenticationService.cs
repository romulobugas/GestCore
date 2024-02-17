using System.Threading.Tasks;

namespace GestCore.Services
{
    public interface IAuthenticationService
    {
        Task<(bool IsAuthenticated, string Token)> AuthenticateUserAsync(string name, string password);
        // Outros métodos, se houver
    }
}
