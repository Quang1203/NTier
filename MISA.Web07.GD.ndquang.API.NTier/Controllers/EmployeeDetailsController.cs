using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web07.GD.ndquang.BL;
using MISA.Web07.GD.ndquang.BL.Exceptions;
using MISA.Web07.GD.ndquang.Common.Entity;
using MISA.Web07.GD.ndquang.Common.Entity.DTO;
using MISA.Web07.GD.ndquang.Common.Enums;
using MISA.Web07.GD.ndquang.Common.Resources;
using OfficeOpenXml;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.Web07.GD.ndquang.API.NTier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDetailsController : ControllerBase
    {
        #region Field

        private IEmployeeDetailBL _employeeDetailBL;

        #endregion

        #region Constructor

        public EmployeeDetailsController(IEmployeeDetailBL employeeDetailBL)
        {
            _employeeDetailBL = employeeDetailBL;
        }

        #endregion

        [HttpGet("GetDetail/{employeeID}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetOfficerDetail([FromRoute] Guid employeeID)
        {
            try
            {
                var result = _employeeDetailBL.GetEmployeeDetailByID(employeeID);
                if (result != null)
                {
                    return base.StatusCode(StatusCodes.Status200OK, new Common.Entity.EmployeeDetail()
                    {
                        employee = result.employee,
                        ListSubject = result.ListSubject,
                        ListStorageRoom = result.ListStorageRoom
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, Resource.error_expception);
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
        [HttpGet("filterDetails")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PagingData<Common.Entity.EmployeeDetail>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult FilterEmployees(
            [FromQuery] string? keyword,
            [FromQuery] Guid? groupID,
            [FromQuery] Guid? storageRoomID,
            [FromQuery] Guid? subjectID,
            [FromQuery] int pageSize = 100,
            [FromQuery] int pageNumber = 1)
        {
            try
            {
                var multipleResults = _employeeDetailBL.FilterEmployeeDetails(keyword, groupID, storageRoomID, subjectID, pageSize, pageNumber);

                // Xử lý kết quả trả về từ DB
                if (multipleResults != null)
                {
                    return StatusCode(StatusCodes.Status200OK, multipleResults);

                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, Resource.error_expception);
            }
        }

        [HttpGet("ExportFile")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PagingData<Common.Entity.EmployeeDetail>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult ExportFile(
            [FromQuery] string? keyword,
            [FromQuery] Guid? groupID,
            [FromQuery] Guid? storageRoomID,
            [FromQuery] Guid? subjectID,
            [FromQuery] int pageSize = 100,
            [FromQuery] int pageNumber = 1)
        {
            var paging = _employeeDetailBL.FilterEmployeeDetails(keyword, groupID, storageRoomID, subjectID, pageSize, pageNumber);
            var employeeDetails = paging.Data;
            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Teachers-Officer");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");

                const int startRow = 4;
                var row = startRow;

                //Create Headers and format them
                worksheet.Row(1).Height = 20;
                worksheet.Column(1).Width = 5;
                worksheet.Column(2).Width = 13;
                worksheet.Column(3).Width = 20;
                worksheet.Column(4).Width = 16;
                worksheet.Column(5).Width = 18;
                worksheet.Column(6).Width = 20;
                worksheet.Column(7).Width = 40;
                worksheet.Column(7).AutoFit();
                worksheet.Column(8).Width = 12;
                worksheet.Column(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Column(9).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Column(9).Width = 15;
                worksheet.Cells["A1"].Value = "Danh sách";
                worksheet.Cells["A1"].Style.Font.Bold = true;
                using (var r = worksheet.Cells["A1:I1"])
                {
                    r.Merge = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                }


                worksheet.Cells["A3"].Value = "STT";
                worksheet.Cells["B3"].Value = "Số hiệu cán bộ";
                worksheet.Cells["C3"].Value = "Họ và tên";
                worksheet.Cells["D3"].Value = "Số điện thoại";
                worksheet.Cells["E3"].Value = "Tổ chuyên môn";
                worksheet.Cells["F3"].Value = "Quản lý thiết bị môn";
                worksheet.Cells["G3"].Value = "Quản lý kho - phòng";
                worksheet.Cells["H3"].Value = "Đào tạo QLTB";
                worksheet.Cells["I3"].Value = "Đang làm việc";

                worksheet.Cells["A3:I3"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A3:I3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;

       
                row = 4;
                foreach (var user in employeeDetails)
                {

                    worksheet.Cells[row, 1].Value = row - 3;
                    worksheet.Cells[row, 2].Value = user.employee.EmployeeCode;
                    worksheet.Cells[row, 3].Value = user.employee.EmployeeName;
                    worksheet.Cells[row, 4].Value = user.employee.TelNumber;
                    worksheet.Cells[row, 5].Value = user.employee.GroupName;
                    var subject = "";
                    foreach (var item in user.ListSubject)
                    {
                        subject += item.SubjectName + "  ";
                    }
                    worksheet.Cells[row, 6].Value = subject;
                    var storageRoom = "";
                    foreach (var item in user.ListStorageRoom)
                    {
                        storageRoom += item.StorageRoomName + "  ";
                    }
                    worksheet.Cells[row, 7].Value = storageRoom;
                    
                    var x = "x";
                    var notX = "";

                    if (user.employee.EMT == 1)
                    {
                        worksheet.Cells[row, 8].Value = x;
                    } else
                    {
                        worksheet.Cells[row, 8].Value = notX;
                    }
                    worksheet.Cells[row, 8].Value = x;
                    worksheet.Cells[row, 9].Value = user.employee.WorkStatus;

                    var workStatus = user.employee.WorkStatus;
                    if (workStatus == WorkStatus.CurrentlyWorking)
                    {
                        worksheet.Cells[row, 9].Value = x;
                    }
                    else
                    {
                        worksheet.Cells[row, 9].Value = notX;
                    }

                    row++;
                }

                // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"DanhSachCanBoGiaoVien_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
        }

        /// <summary>
        /// API Thêm mới 1 nhân viên
        /// </summary>
        /// <param name="employeeDetail">Đối tượng bản ghi cần thêm mới</param>
        /// <returns>ID của bản ghi vừa thêm mới</returns>
        /// Created by: NDQuang (24/08/2022)
        [HttpPost("Detail")]
        public IActionResult InsertOneEmployeeDetail([FromBody] EmployeeDetail employeeDetail)
        {
            try
            {
                //var validateResult = HandleError.ValidateEntity(ModelState, HttpContext);
                //if (validateResult != null)
                //{
                //    return StatusCode(StatusCodes.Status400BadRequest, validateResult);
                //}

                var employeeDetailID = _employeeDetailBL.InsertOneEmployeeDetail(employeeDetail);

                if (employeeDetailID != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status201Created, employeeDetailID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Resource.failedOperation);
                }
            }
            catch (ValidateException ex)
            {
                var res = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Data
                };
                return StatusCode(StatusCodes.Status400BadRequest, res);
            }
            //catch (MySqlException mySqlException)
            //{
            //    return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateDuplicateCodeErrorResult(mySqlException, HttpContext));
            //}
            //catch (Exception exception)
            //{
            //    return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(exception, HttpContext));
            //}
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, Resource.error_expception);
            }
        }



        /// <summary>
        /// API Sửa 1 nhân viên
        /// </summary>
        /// <param name="employeeID">ID Đối tượng bản ghi cần sửa</param>
        /// <param name="employeeDetail">Đối tượng bản ghi cần sửa</param>
        /// <returns>ID của bản ghi vừa sửa</returns>
        /// Created by: NDQuang (24/08/2022)
        [HttpPut("Detail/{employeeID}")]
        public IActionResult UpdateOneEmployeeDetail([FromRoute] Guid employeeID, [FromBody] EmployeeDetail employeeDetail)
        {
            try
            {

                var employeeDetailID = _employeeDetailBL.UpdateOneEmployeeDetail(employeeID, employeeDetail);

                if (employeeDetailID != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeDetailID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Resource.failedOperation);
                }
               
            }
            catch (ValidateException ex)
            {
                var res = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Data
                };
                return StatusCode(StatusCodes.Status400BadRequest, res);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, Resource.error_expception);
            }
        }


    }
}
