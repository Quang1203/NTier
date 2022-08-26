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
    public class SubjectsController : ControllerBase
    {
        /// <summary>
        /// API Lấy toàn bộ danh sách môn
        /// </summary>
        /// <returns>Danh sách môn</returns>
        /// Created by: NDQuang (17/08/2022)
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<Subject>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllSubjects()
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web07.gd.nguyendangquang;Uid=root;Pwd=123456789;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh truy vấn
                string getAllSubjectsCommand = "SELECT * FROM subject;";

                // Thực hiện gọi vào DB để chạy câu lệnh truy vấn ở trên
                var subjects = mySqlConnection.Query<Subject>(getAllSubjectsCommand);

                // Xử lý dữ liệu trả về
                if (subjects != null)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, subjects);
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
