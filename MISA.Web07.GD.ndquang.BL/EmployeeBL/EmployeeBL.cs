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
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        #region Field
        List<string> Errors = new List<string>();
        private IEmployeeDL _employeeDL;

        #endregion

        #region Constructor

        public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;
        }



        #endregion

        #region Method
        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="record"></param>
        protected override void Validate(Employee employee)
        {
            // mã nhân không được phép trùng:
            if(_employeeDL.CheckDuplicateEmployeeCode(employee.EmployeeCode) != null)
            {
                Errors.Add(Resource.duplicateCode_expception); 
            }

            // Ngày nghỉ việc không được lớn hơn ngày hiện tại:
            if (employee.QuitDate > DateTime.Now)
            {
                Errors.Add(Resource.quitDate_expception);
            }

            // Số điện thoại không đủ độ dài:
            if (employee.TelNumber.Length < 10)
            {
                Errors.Add(Resource.telNumber_expception);
            }

            // mã nhân viên không được phép để trống:
            if (employee.EmployeeCode == null)
            {
                Errors.Add(Resource.empCodeRequired_expception);
            }

            // họ và tên nhân viên không được phép để trống:
            if (employee.EmployeeName == null)
            {
                Errors.Add(Resource.empNameRequired_expception);
            }

            // email phải hợp lệ:
            if (ValidateEmail(employee.Email) == false)
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
        /// API Xóa 1 nhân viên
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn xóa</param>
        /// <returns>ID của nhân viên vừa xóa</returns>
        /// Created by: NDQuang (17/08/2022)
        public int DeleteEmployeeByID(Guid employeeID)
        {
            return _employeeDL.DeleteEmployeeByID(employeeID);
        }

        /// <summary>
        /// Kiểm tra trùng mã nhân viên 
        /// </summary>
        /// <returns>Mã nhân viên mới tự động tăng</returns>
        /// Created by: NDQuang (17/08/2022)
        public string CheckDuplicateEmployeeCode(string employeeCode)
        {
            return _employeeDL.CheckDuplicateEmployeeCode(employeeCode);
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
        public PagingData<Employee> FilterEmployees(string? keyword, Guid? groupID, Guid? storageRoomID, Guid? subjectID, int pageSize , int pageNumber )
        {
            return _employeeDL.FilterEmployees(keyword, groupID, storageRoomID, subjectID, pageSize, pageNumber);
        }

        /// <summary>
        /// API Lấy thông tin chi tiết của 1 nhân viên
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn lấy thông tin chi tiết</param>
        /// <returns>Đối tượng nhân viên muốn lấy thông tin chi tiết</returns>
        /// Created by: NDQuang (17/08/2022)
        public Employee GetEmployeeByID(Guid employeeID)
        {
            return _employeeDL.GetEmployeeByID(employeeID);
        }

        /// <summary>
        /// Lấy mã nhân viên tự động tăng
        /// </summary>
        /// <returns>
        /// Mã nhân viên tự động tăng
        /// </returns>
        /// Created by: NDQuang (17/08/2022)
        public string GetNewEmployeeCode()
        {
            return _employeeDL.GetNewEmployeeCode();
        }

        /// <summary>
        /// API Thêm mới 1 nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên muốn thêm mới</param>
        /// <returns>ID của nhân viên vừa thêm mới</returns>
        /// Created by: NDQuang (17/08/2022)
        //

        /// <summary>
        /// API Sửa 1 nhân viên
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn sửa</param>
        /// <param name="employee">Đối tượng nhân viên muốn sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        /// Created by: NDQuang (17/08/2022)
        //public int UpdateEmployee(Guid employeeID, Employee employee)
        //{
        //    return _employeeDL.UpdateEmployee(employeeID , employee);
        //}

        #endregion
    }
}
