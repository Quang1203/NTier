using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace MISA.Web07.gd.ndquang.API.Controllers
{
    [Route("api/Employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        [HttpGet]
        public string GetName()
        {
            return "NDQUANG";
        }
    }
}
