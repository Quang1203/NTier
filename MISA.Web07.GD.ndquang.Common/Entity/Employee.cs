using MISA.Web07.GD.ndquang.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.Web07.GD.ndquang.Common.Entity
{
    /// <summary>
    /// Nhân viên
    /// </summary>
    [Table("employee")]
    public class Employee
    {
        #region Property

        /// <summary>
        /// ID nhân viên
        /// </summary>
        [Key]
        public Guid EmployeeID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [Required(ErrorMessage = "e005")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string? TelNumber { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Số CMND
        /// </summary>
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        //[EmailAddress(ErrorMessage = "e009")]
        public string? Email { get; set; }

        /// <summary>
        /// Lương
        /// </summary>
        public double Salary { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreateBy { get; set; }

        /// <summary>
        /// Ngày sửa gần nhất
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// ID tổ bộ môn
        /// </summary>
        public Guid? GroupID { get; set; }

        /// <summary>
        /// Tên tổ bộ môn
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// ID môn
        /// </summary>
        public Guid? SubjectID { get; set; }

        /// <summary>
        /// Tên môn
        /// </summary>
        public string? SubjectName { get; set; }

        /// <summary>
        /// ID kho phòng
        /// </summary>
        public Guid? StorageRoomID { get; set; }

        /// <summary>
        /// Tên kho phòng
        /// </summary>
        public string? StorageRoomName { get; set; }

        /// <summary>
        /// Đào tạo QLTB (1: có trình độ, 0: không có trình độ)
        /// </summary>
        public int EMT { get; set; }


        /// <summary>
        /// Tình trạng làm việc
        /// </summary>
        public WorkStatus WorkStatus { get; set; }

        /// <summary>
        /// Ngày nghỉ việc
        /// </summary>
        public DateTime QuitDate { get; set; }

        /// <summary>
        /// List môn
        /// </summary>
        public List<Subject>? ListSubject { get; set; }

        /// <summary>
        /// List kho phòng
        /// </summary>
        public List<StorageRoom>? ListStorageRoom { get; set; }

      

        #endregion
    }
}
