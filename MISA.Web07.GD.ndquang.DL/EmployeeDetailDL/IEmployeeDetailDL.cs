using MISA.Web07.GD.ndquang.Common.Entity;
using MISA.Web07.GD.ndquang.Common.Entity.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web07.GD.ndquang.DL
{
    public interface IEmployeeDetailDL
    {
        /// <summary>
        /// Lấy thông tin chi tiết của 1 nhân viên
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn lấy thông tin chi tiết</param>
        /// <returns>Đối tượng nhân viên muốn lấy thông tin chi tiết</returns>
        /// Created by: NDQuang (17/08/2022)
        public EmployeeDetail GetEmployeeDetailByID(Guid employeeID);

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
        public PagingData<EmployeeDetail> FilterEmployeeDetails(
            string? keyword,
            Guid? groupID,
            Guid? storageRoomID,
            Guid? subjectID,
            int pageSize,
            int pageNumber);

        /// <summary>
        /// Thêm mới một nhân viên
        /// </summary>
        /// <param name="employeeDetail">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>ID nhân viên được thêm mới</returns>
        /// Created by: NDQuang (24/08/2022)
        public Guid InsertOneEmployeeDetail(EmployeeDetail employeeDetail);

        /// <summary>
        /// Sửa một nhân viên
        /// </summary>
        /// <param name="employeeDetail">Đối tượng nhân viên cần sửa</param>
        /// <returns>ID nhân viên được sửa</returns>
        /// Created by: NDQuang (24/08/2022)
        public Guid UpdateOneEmployeeDetail(Guid employeeID, EmployeeDetail employeeDetail);
        
    }
}
