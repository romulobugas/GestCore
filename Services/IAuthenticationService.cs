using System.Threading.Tasks;
using GestCore.Models;

namespace GestCore.Services
{
    public interface IAuthenticationService
    {
        Task<UserApp> AuthenticateUserAsync(string name, string password);
        // Outros métodos, se houver
    }
}
