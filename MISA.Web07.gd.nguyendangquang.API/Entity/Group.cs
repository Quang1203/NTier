namespace MISA.Web07.gd.nguyendangquang.API.Entity
{
    /// <summary>
    /// Tổ bộ môn
    /// </summary>
    public class Group
    {
        /// <summary>
        /// ID Tổ bộ môn
        /// </summary>
        public Guid GroupID { get; set; }

        /// <summary>
        /// Mã Tổ bộ môn
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// Tên Tổ bộ môn
        /// </summary>
        public string GroupName { get; set; }

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
