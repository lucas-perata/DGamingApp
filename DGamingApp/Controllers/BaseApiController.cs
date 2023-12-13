using DGamingApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DGamingApp.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]

    public class BaseApiController : ControllerBase
    {

    }
}