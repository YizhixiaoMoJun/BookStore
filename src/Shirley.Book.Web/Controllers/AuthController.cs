using BookApi.Model;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shirley.Book.Service.AuthServices;
using Shirley.Book.Model;
using Shirley.Book.Service.Domains;

namespace BookApi.Controllers
{
    /// <summary>
    /// Authentication controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _authServices;

        public AuthController(ILoginService authServices)
        {
            _authServices = authServices;
        }

        /// <summary>
        /// Login method controller
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login(UserInfo userInfo)
        {
            if (string.IsNullOrEmpty(userInfo.UserName) || string.IsNullOrEmpty(userInfo.Pwd))
            {
                Log.Information("UserName or Password is incorrect");
                return BadRequest(new BaseResponse { Message = "UserName or Password is incorrect", IsSuccess = false });
            }
           return Ok(_authServices.Login(userInfo.UserName, userInfo.Pwd));
        }


        [HttpPost("LoginPost")]
        public IActionResult LoginPost(string userName, string pwd)
        {
            if (userName == "dog" && pwd == "shirely")
            {
                throw new DomainException("dog can not login my system!");
            }

            return Ok();
        }

        [HttpPost("LoginPost2")]
        public IActionResult LoginPost2(UserInfo userInfo)
        {
            return Ok();
        }

    }
}
