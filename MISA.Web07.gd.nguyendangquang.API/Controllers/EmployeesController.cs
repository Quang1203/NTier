using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web07.gd.nguyendangquang.API.Entity;
using MISA.Web07.gd.nguyendangquang.API.Entity.DTO;
using MISA.Web07.gd.nguyendangquang.API.Enums;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.Web07.gd.nguyendangquang.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        #region Method

        /// <summary>
        /// API Thêm mới 1 nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên muốn thêm mới</param>
        /// <returns>ID của nhân viên vừa thêm mới</returns>
        /// Created by: NDQuang (17/08/2022)
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult InsertEmployee([FromBody] Employee employee)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web07.gd.nguyendangquang;Uid=root;Pwd=123456789;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh INSERT INTO
                //INSERT INTO employee(EmployeeID, EmployeeCode, EmployeeName, TelNumber, DateOfBirth, Gender, IdentityNumber, Email, Salary, CreateDate, CreateBy, ModifiedDate, ModifiedBy, GroupID, GroupName, SubjectID, SubjectName, StorageRoomID, StorageRoomName)
                string insertEmployeeCommand = "INSERT INTO employee (EmployeeID, EmployeeCode, EmployeeName, TelNumber, DateOfBirth, Gender, IdentityNumber, Email, Salary, CreateDate, CreateBy, ModifiedDate, ModifiedBy, GroupID, SubjectID, StorageRoomID) " +
                    "VALUES (@EmployeeID, @EmployeeCode, @EmployeeName, @TelNumber, @DateOfBirth, @Gender, @IdentityNumber, @Email, @Salary, @CreateDate, @CreateBy, @ModifiedDate, @ModifiedBy, @GroupID, @SubjectID, @StorageRoomID);";
 
                // Chuẩn bị tham số đầu vào cho câu lệnh INSERT INTO
                var employeeID = Guid.NewGuid();
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID);
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@EmployeeName", employee.EmployeeName);
                parameters.Add("@TelNumber", employee.TelNumber);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@Salary", employee.Salary);
                //parameters.Add("@WorkStatus", employee.WorkStatus);
                parameters.Add("@CreateDate", employee.CreateDate);
                parameters.Add("@CreateBy", employee.CreateBy);
                parameters.Add("@ModifiedDate", employee.ModifiedDate);
                parameters.Add("@ModifiedBy", employee.ModifiedBy);
                parameters.Add("@GroupID", employee.GroupID);
                parameters.Add("@SubjectID", employee.SubjectID);
                parameters.Add("@StorageRoomID", employee.StorageRoomID);

                // Thực hiện gọi vào DB để chạy câu lệnh INSERT INTO với tham số đầu vào ở trên
                int numberOfAffectedRows = mySqlConnection.Execute(insertEmployeeCommand, parameters);

                // Xử lý kết quả trả về từ DB
                if (numberOfAffectedRows > 0)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status201Created, employeeID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (MySqlException mySqlException)
            {
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// API Sửa 1 nhân viên
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn sửa</param>
        /// <param name="employee">Đối tượng nhân viên muốn sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        /// Created by: NDQuang (17/08/2022)
        [HttpPut("{employeeID}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateEmployee([FromRoute] Guid employeeID, [FromBody] Employee employee)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web07.gd.nguyendangquang;Uid=root;Pwd=123456789;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh UPDATE
                string updateEmployeeCommand = "UPDATE employee " +
                    "SET " +
                    "EmployeeCode = @EmployeeCode, " +
                    "EmployeeName = @EmployeeName, " +
                    "TelNumber = @TelNumber, " +
                    "DateOfBirth = @DateOfBirth, " +
                    "Gender = @Gender, " +
                    "IdentityNumber = @IdentityNumber, " +
                    "Email = @Email, " +
                    "Salary = @Salary, " +
                    "ModifiedDate = @ModifiedDate, " +
                    "ModifiedBy = @ModifiedBy, " +
                    "GroupID = @GroupID, " +
                    "SubjectID = @SubjectID, " +
                    "StorageRoomID = @StorageRoomID " +
                "WHERE EmployeeID = @EmployeeID;";

                // Chuẩn bị tham số đầu vào cho câu lệnh UPDATE
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID);
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@EmployeeName", employee.EmployeeName);
                parameters.Add("@TelNumber", employee.TelNumber);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@Salary", employee.Salary);
                //parameters.Add("@WorkStatus", employee.WorkStatus);
                parameters.Add("@ModifiedDate", employee.ModifiedDate);
                parameters.Add("@ModifiedBy", employee.ModifiedBy);
                parameters.Add("@GroupID", employee.GroupID);
                parameters.Add("@SubjectID", employee.SubjectID);
                parameters.Add("@StorageRoomID", employee.StorageRoomID);

                // Thực hiện gọi vào DB để chạy câu lệnh UPDATE với tham số đầu vào ở trên
                int numberOfAffectedRows = mySqlConnection.Execute(updateEmployeeCommand, parameters);

                // Xử lý kết quả trả về từ DB
                if (numberOfAffectedRows > 0)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, employeeID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }

            }
            catch (MySqlException mySqlException)
            {
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }

                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// API Xóa 1 nhân viên
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn xóa</param>
        /// <returns>ID của nhân viên vừa xóa</returns>
        /// Created by: NDQuang (17/08/2022)
        [HttpDelete("{employeeID}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteEmployeeByID([FromRoute] Guid employeeID)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web07.gd.nguyendangquang;Uid=root;Pwd=123456789;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh DELETE
                string deleteEmployeeCommand = "DELETE FROM employee WHERE EmployeeID = @EmployeeID";

                // Chuẩn bị tham số đầu vào cho câu lệnh DELETE
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID);

                // Thực hiện gọi vào DB để chạy câu lệnh DELETE với tham số đầu vào ở trên
                int numberOfAffectedRows = mySqlConnection.Execute(deleteEmployeeCommand, parameters);

                // Xử lý kết quả trả về từ DB
                if (numberOfAffectedRows > 0)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, employeeID);
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

        /// <summary>
        /// API Lấy danh sách nhân viên cho phép lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param> 
        /// <param name="groupID">ID tổ bộ môn</param>
        /// <param name="storageRoomID">ID kho phòng</param>
        /// <param name="subjectID">ID môn</param>
        /// <param name="pageSize">Số trang muốn lấy</param>
        /// <param name="pageNumber">Thứ tự trang muốn lấy</param>
        /// <returns>Một đối tượng gồm:
        /// + Danh sách nhân viên thỏa mãn điều kiện lọc và phân trang
        /// + Tổng số nhân viên thỏa mãn điều kiện</returns>
        /// Created by: NDQuang (17/08/2022)
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PagingData<Employee>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult FilterEmployees(
            [FromQuery] string? keyword,
            [FromQuery] Guid? groupID,
            [FromQuery] Guid? storageRoomID,
            [FromQuery] Guid? subjectID,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web07.gd.nguyendangquang;Uid=root;Pwd=123456789;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị tên Stored procedure
                string storedProcedureName = "Proc_employee_GetPaging";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("@v_Offset", (pageNumber - 1) * pageSize);
                parameters.Add("@v_Limit", pageSize);
                parameters.Add("@v_Sort", "ModifiedDate DESC");

                var orConditions = new List<string>();
                var andConditions = new List<string>();
                string whereClause = "";

                if (keyword != null)
                {
                    orConditions.Add($"EmployeeCode LIKE '%{keyword}%'");
                    orConditions.Add($"EmployeeName LIKE '%{keyword}%'");
                    orConditions.Add($"TelNumber LIKE '%{keyword}%'");
                }
                if (orConditions.Count > 0)
                {
                    whereClause = $"({string.Join(" OR ", orConditions)})";
                }

                if (groupID != null)
                {
                    andConditions.Add($"GroupID LIKE '%{groupID}%'");
                }
                if (storageRoomID != null)
                {
                    andConditions.Add($"StorageRoomID LIKE '%{storageRoomID}%'");
                }
                if (subjectID != null)
                {
                    andConditions.Add($"SubjectID LIKE '%{subjectID}%'");
                }

                if (andConditions.Count > 0)
                {
                    whereClause += $" AND {string.Join(" AND ", andConditions)}";
                }

                parameters.Add("@v_Where", whereClause);

                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var multipleResults = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB
                if (multipleResults != null)
                {
                    var employees = multipleResults.Read<Employee>().ToList();
                    var totalCount = multipleResults.Read<long>().Single();
                    return StatusCode(StatusCodes.Status200OK, new PagingData<Employee>()
                    {
                        Data = employees,
                        TotalCount = totalCount
                    });
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

        /// <summary>
        /// API Lấy thông tin chi tiết của 1 nhân viên
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn lấy thông tin chi tiết</param>
        /// <returns>Đối tượng nhân viên muốn lấy thông tin chi tiết</returns>
        /// Created by: NDQuang (17/08/2022)
        [HttpGet("{employeeID}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Employee))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetEmployeeByID([FromRoute] Guid employeeID)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web07.gd.nguyendangquang;Uid=root;Pwd=123456789;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị tên Stored procedure
                string storedProcedureName = "Proc_employee_GetByEmployeeID";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("@v_EmployeeID", employeeID);

                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var employee = mySqlConnection.QueryFirstOrDefault<Employee>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB
                if (employee != null)
                {
                    return StatusCode(StatusCodes.Status200OK, employee);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// Lấy mã nhân viên tự động tăng
        /// </summary>
        /// <returns>
        /// Mã nhân viên tự động tăng
        /// </returns>
        /// Created by: NDQuang (17/08/2022)
        [HttpGet("new-code")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetNewEmployeeCode()
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3306;Database=misa.web07.gd.nguyendangquang;Uid=root;Pwd=123456789;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_employee_GetMaxCode";

                // Thực hiện gọi vào DB để chạy stored procedure ở trên
                string maxEmployeeCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý sinh mã nhân viên mới tự động tăng
                // Cắt chuỗi mã nhân viên lớn nhất trong hệ thống để lấy phần số
                // Mã nhân viên mới = "NV" + Giá trị cắt chuỗi ở  trên + 1
                // "NV99997"
                string newEmployeeCode = "NV" + (Int64.Parse(maxEmployeeCode.Substring(2)) + 1).ToString();

                // Trả về dữ liệu cho client
                return StatusCode(StatusCodes.Status200OK, newEmployeeCode);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        #endregion
    }

}
