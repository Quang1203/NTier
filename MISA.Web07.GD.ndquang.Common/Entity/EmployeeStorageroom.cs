using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web07.GD.ndquang.Common.Entity
{
    /// <summary>
    /// Nhân viên - Kho phòng (Entity trung gian quan hệ nhiều - nhiều ) 
    /// </summary>
    [Table("employee_storageroom")]
    public class EmployeeStorageroom
    {
        /// <summary>
        /// ID nhân viên-kho phòng
        /// </summary>
        [Key]
        public Guid EmployeeStorageroomID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// ID nhân viên
        /// </summary>
        public Guid EmployeeID { get; set; } 

        /// <summary>
        /// ID kho phòng
        /// </summary>
        public Guid StorageRoomID { get; set; }

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
    }
}
