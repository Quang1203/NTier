using MISA.Web07.GD.ndquang.Common.Entity;
using MISA.Web07.GD.ndquang.Common.Entity.DTO;
using MISA.Web07.GD.ndquang.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web07.GD.ndquang.BL
{
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        #region Field

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
        public int UpdateEmployee(Guid employeeID, Employee employee)
        {
            return _employeeDL.UpdateEmployee(employeeID , employee);
        }

        #endregion
    }
}
