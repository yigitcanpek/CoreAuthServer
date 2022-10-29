using CoreSharedLibary.DTO_s;
using Microsoft.AspNetCore.Mvc;

namespace CoreAuthServer.API.Controllers
{
    public class CustomBaseController : ControllerBase
    {
      public IActionResult ActionResultInstance<T>(Response<T> response)where T : class
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
