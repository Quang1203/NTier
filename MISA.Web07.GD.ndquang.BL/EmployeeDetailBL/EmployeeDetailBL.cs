using MISA.Web07.GD.ndquang.BL.Exceptions;
using MISA.Web07.GD.ndquang.Common.Entity;
using MISA.Web07.GD.ndquang.Common.Entity.DTO;
using MISA.Web07.GD.ndquang.Common.Resources;
using MISA.Web07.GD.ndquang.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.Web07.GD.ndquang.BL
{
    public class EmployeeDetailBL : IEmployeeDetailBL
    {
        #region Field
        private IEmployeeDetailDL _employeeDetailDL;
        private IEmployeeDL _employeeDL;
        List<string> Errors = new List<string>();
        #endregion

        #region Constructor
        public EmployeeDetailBL(IEmployeeDetailDL employeeDetailDL, IEmployeeDL employeeDL)
        {
            _employeeDetailDL = employeeDetailDL;
            _employeeDL = employeeDL;
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
        public PagingData<Common.Entity.EmployeeDetail> FilterEmployeeDetails(string? keyword, Guid? groupID, Guid? storageRoomID, Guid? subjectID, int pageSize, int pageNumber)
        {
            return _employeeDetailDL.FilterEmployeeDetails(keyword, groupID, storageRoomID, subjectID, pageSize, pageNumber);
        }


        #endregion

        #region Method
        // <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Trả về tất cả bản ghi</returns>
        /// Created by: NDQuang (23/08/2022)
        public Common.Entity.EmployeeDetail GetEmployeeDetailByID(Guid employeeID)
        {
            return _employeeDetailDL.GetEmployeeDetailByID(employeeID);
        }

        /// <summary>
        /// Thêm mới một nhân viên
        /// </summary>
        /// <param name="employeeDetail">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>ID nhân viên được thêm mới</returns>
        /// Created by: NDQuang (24/08/2022)
        public Guid InsertOneEmployeeDetail(EmployeeDetail employeeDetail)
        {
            Validate(employeeDetail);
            return _employeeDetailDL.InsertOneEmployeeDetail(employeeDetail);

        }

        public Guid UpdateOneEmployeeDetail(Guid employeeID, EmployeeDetail employeeDetail)
        {
            Validates(employeeDetail);
            return _employeeDetailDL.UpdateOneEmployeeDetail(employeeID, employeeDetail);
        }

        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="employeeDetail"></param>
        protected void Validate(EmployeeDetail employeeDetail)
        {
            // mã nhân viên không được phép trùng:
            if (_employeeDL.CheckDuplicateEmployeeCode(employeeDetail.employee.EmployeeCode) != null)
            {
                Errors.Add(Resource.duplicateCode_expception);
            }

            // Ngày nghỉ việc không được lớn hơn ngày hiện tại:
            if (employeeDetail.employee.QuitDate > DateTime.Now)
            {
                Errors.Add(Resource.quitDate_expception);
            }

            // Số điện thoại không đủ độ dài:
            if (employeeDetail.employee.TelNumber.Length < 10)
            {
                Errors.Add(Resource.telNumber_expception);
            }

            // mã nhân viên không được phép để trống:
            if (employeeDetail.employee.EmployeeCode == null)
            {
                Errors.Add(Resource.empCodeRequired_expception);
            }

            // họ và tên nhân viên không được phép để trống:
            if (employeeDetail.employee.EmployeeName == null)
            {
                Errors.Add(Resource.empNameRequired_expception);
            }

            // email phải hợp lệ:
            if (ValidateEmail(employeeDetail.employee.Email) == false)
            {
                Errors.Add(Resource.email_expception);
            }

            if (Errors.Count > 0)
            {
                throw new ValidateException(Errors);
            }

        }

        /// <summary>
        /// Validate email
        /// </summary>
        /// <param name="email"></param>
        public Boolean ValidateEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="employeeDetail"></param>
        protected void Validates(EmployeeDetail employeeDetail)
        {
            // Ngày nghỉ việc không được lớn hơn ngày hiện tại:
            if (employeeDetail.employee.QuitDate > DateTime.Now)
            {
                Errors.Add(Resource.quitDate_expception);
            }

            // Số điện thoại không đủ độ dài:
            if (employeeDetail.employee.TelNumber.Length < 10)
            {
                Errors.Add(Resource.telNumber_expception);
            }

            // mã nhân viên không được phép để trống:
            if (employeeDetail.employee.EmployeeCode == null)
            {
                Errors.Add(Resource.empCodeRequired_expception);
            }

            // họ và tên nhân viên không được phép để trống:
            if (employeeDetail.employee.EmployeeName == null)
            {
                Errors.Add(Resource.empNameRequired_expception);
            }

            // email phải hợp lệ:
            if (ValidateEmail(employeeDetail.employee.Email) == false)
            {
                Errors.Add(Resource.email_expception);
            }

            if (Errors.Count > 0)
            {
                throw new ValidateException(Errors);
            }

        }
        #endregion
    }
}
