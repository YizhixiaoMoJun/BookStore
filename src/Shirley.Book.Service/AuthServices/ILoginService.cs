using Shirley.Book.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shirley.Book.Service.AuthServices
{
    public interface ILoginService
    {
        Task<AuthenticateResult> Login(string userName, string password);
    }
}
