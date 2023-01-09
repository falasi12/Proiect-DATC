using LanguageExt;
using System.Threading.Tasks;

namespace APIAPP.Repositories
{
    public interface IUserRepository
    {
        Task<bool> TryLogin(string username, string password);
        Task<bool> TryAdminLogin(string username, string password);
    }
}
