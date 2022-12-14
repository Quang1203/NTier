using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web07.GD.ndquang.DL
{
    public interface IBaseDL<T>
    {
        /// <summary>
        /// Lấy tất cả bản ghi  
        /// </summary>
        /// <returns>Trả về tất cả bản ghi</returns>
        /// Created by: NDQuang (23/08/2022)
        public IEnumerable<dynamic> GetAllRecords();



        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi cần thêm mới</param>
        /// <returns>ID bản ghi được thêm mới</returns>
        /// Created by: NDQuang (24/08/2022)
        public Guid InsertOneRecord(T record);


        /// <summary>
        /// Sửa một bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi cần sửa</param>
        /// <returns>ID bản ghi được sửa</returns>
        /// Created by: NDQuang (24/08/2022)
        public Guid UpdateOneRecord(Guid recordID, T record);



    }
}
