using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web07.GD.ndquang.Common.Utilities
{
    /// <summary>
    /// Những hàm dùng chung xử lý entity
    /// </summary>
    public static class EntityUtilities
    {
        /// <summary>
        /// Lấy tên bảng của entity
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của entity</typeparam>
        /// <returns>Tên bảng</returns>
        /// Created by: NDQuang (24/08/2022)
        public static string GetTableName<T>()
        {
            string tableName = typeof(T).Name;
            var tableAttributes = typeof(T).GetTypeInfo().GetCustomAttributes<TableAttribute>();
            if (tableAttributes.Count() > 0)
            {
                tableName = tableAttributes.First().Name;
            }
            return tableName;
        }
    }
}
