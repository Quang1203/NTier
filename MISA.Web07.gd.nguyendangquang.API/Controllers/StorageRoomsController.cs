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
    public class StorageRoomsController : ControllerBase
    {
        /// <summary>
        /// API Lấy toàn bộ danh sách kho phòng
        /// </summary>
        /// <returns>Danh sách kho phòng</returns>
        /// Created by: NDQuang (17/08/2022)
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<StorageRoom>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllStorageRooms()
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web07.gd.nguyendangquang;Uid=root;Pwd=123456789;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh truy vấn
                string getAllStorageRoomsCommand = "SELECT * FROM storageroom;";

                // Thực hiện gọi vào DB để chạy câu lệnh truy vấn ở trên
                var storageRooms = mySqlConnection.Query<StorageRoom>(getAllStorageRoomsCommand);

                // Xử lý dữ liệu trả về
                if (storageRooms != null)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, storageRooms);
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
