using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web07.gd.nguyendangquang.API.Entity;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.Web07.gd.nguyendangquang.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        /// <summary>
        /// API Lấy toàn bộ danh sách tổ bộ môn
        /// </summary>
        /// <returns>Danh sách tổ bộ môn</returns>
        /// Created by: NDQuang (17/08/2022)
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<Group>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllGroups()
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web07.gd.nguyendangquang;Uid=root;Pwd=123456789;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh truy vấn
                string getAllGroupsCommand = "SELECT * FROM `group`;";

                // Thực hiện gọi vào DB để chạy câu lệnh truy vấn ở trên
                var groups = mySqlConnection.Query<Group>(getAllGroupsCommand);

                // Xử lý dữ liệu trả về
                if (groups != null)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, groups);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }
    }
}
