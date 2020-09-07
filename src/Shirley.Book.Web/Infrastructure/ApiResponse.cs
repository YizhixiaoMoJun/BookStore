using Shirley.Book.Model;

namespace Shirley.Book.Web.Infrastructure
{
    public class ApiResponse
    {
        /// <summary>
        /// generate a response object with status 200
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static BaseResponse OK<T>(T data)
        {
            return new BaseResponse<T> { StatusCode = 200, Data = data, IsSuccess = true };
        }

        /// <summary>
        /// generate a response object with special error message and status code
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="status">status code. default is 500.</param>
        /// <returns></returns>
        public static BaseResponse Error(string message, int status = 500)
        {
            return new BaseResponse { StatusCode = status, Message = message, IsSuccess = false };
        }

        /// <summary>
        /// generate a response object with invalidate domain state and 400 status code
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="status">status code, default is 400.</param>
        /// <returns></returns>
        public static BaseResponse Invalid(string message, int status = 400)
        {
            return new BaseResponse { StatusCode = status, Message = message, IsSuccess = false };
        }
    }
}
