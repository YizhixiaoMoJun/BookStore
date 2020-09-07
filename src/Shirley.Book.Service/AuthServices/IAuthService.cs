using BookApi.Model;
using Shirley.Book.Model;
using System.Threading.Tasks;

namespace Shirley.Book.Service.AuthServices
{
    public interface IAuthService
    {
        Task<AuthenticateResult> Authentication(UserInfo userInfo);
    }
}
