namespace MISA.Web07.gd.nguyendangquang.API.Entity
{
    /// <summary>
    /// QL kho phòng
    /// </summary>
    public class StorageRoom
    {
        /// <summary>
        /// ID kho phòng
        /// </summary>
        public Guid StorageRoomID { get; set; }

        /// <summary>
        ///  MÃ kho phòng
        /// </summary>
        public string StorageRoomCode { get; set; }

        /// <summary>
        /// Tên kho phòng
        /// </summary>
        public string StorageRoomName { get; set; }

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
