namespace MISA.Web07.gd.nguyendangquang.API.Entity
{
    /// <summary>
    /// QL theo môn
    /// </summary>
    public class Subject
    {
        /// <summary>
        /// ID môn 
        /// </summary>
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
