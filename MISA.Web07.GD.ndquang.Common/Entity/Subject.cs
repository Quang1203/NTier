using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.Web07.GD.ndquang.Common.Entity
{
    /// <summary>
    /// QL theo môn
    /// </summary>
    [Table("subject")]
    public class Subject
    {
        /// <summary>
        /// ID môn 
        /// </summary>
        [Key]
        public Guid SubjectID { get; set; }

        /// <summary>
        /// MÃ môn
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Tên môn
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// Ngày sửa gần nhất
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
