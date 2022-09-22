using Dapper;
using MISA.Web07.GD.ndquang.Common.Utilities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web07.GD.ndquang.DL
{
    public class BaseDL<T> : IBaseDL<T>
{

        /// <summary>
        /// Lấy tất cả bản ghi  
        /// </summary>
        /// <returns>Trả về tất cả bản ghi</returns>
        /// Created by: NDQuang (23/08/2022)
        public virtual IEnumerable<dynamic> GetAllRecords()
        {
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {

                // Chuẩn bị storedProcedureName 
                string className = typeof(T).Name;
                string storedProcedureName = $"Proc_{className}_GetAll";


                // Thực hiện gọi vào DB để chạy stored procedure 
                var multipleResults = mySqlConnection.Query(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);


                return (IEnumerable<dynamic>)multipleResults;
            }
        }



        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi cần thêm mới</param>
        /// <returns>ID của nhân viên vừa thêm mới</returns>
        /// Created by: NDQuang (24/08/2022)
        public Guid InsertOneRecord(T record)
        {
            // Khai báo tên stored procedure INSERT
            string tableName = EntityUtilities.GetTableName<T>();
            string insertStoredProcedureName = $"Proc_{tableName}_InsertOne";

            // Chuẩn bị tham số đầu vào của stored procedure
            var properties = typeof(T).GetProperties();
            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                string propertyName = $"v_{property.Name}"; // v_DepartmentCode
                var propertyValue = property.GetValue(record); // Đọc vào object record --> lấy được value của property tên là DepartmentCode
                parameters.Add(propertyName, propertyValue);
            }

            // Thực hiện gọi vào DB để chạy câu lệnh stored procedure với tham số đầu vào ở trên
            int numberOfAffectedRows = 0;
            //using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                numberOfAffectedRows = mySqlConnection.Execute(insertStoredProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                var result = Guid.Empty;
                if (numberOfAffectedRows > 0)
                {
                    var primaryKeyProperty = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
                    var newId = primaryKeyProperty?.GetValue(record);
                    if (newId != null)
                    {
                        result = (Guid)newId;
                    }
                }
                return result;
            }

        }

        /// <summary>
        /// Sửa một bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi cần sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        /// Created by: NDQuang (24/08/2022)
        public Guid UpdateOneRecord(Guid recordID, T record)
        {
            // Khai báo tên stored procedure Update
            string tableName = EntityUtilities.GetTableName<T>();
            string updateStoredProcedureName = $"Proc_{tableName}_UpdateOne";

            // Chuẩn bị tham số đầu vào của stored procedure
            var properties = typeof(T).GetProperties();
            var parameters = new DynamicParameters();
            var primaryKeyProperty = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
            primaryKeyProperty.SetValue(record, recordID);

            foreach (var property in properties)
            {
                string propertyName = $"v_{property.Name}"; // v_DepartmentCode
                var propertyValue = property.GetValue(record); // Đọc vào object record --> lấy được value của property tên là DepartmentCode
                parameters.Add(propertyName, propertyValue);
            }

            // Thực hiện gọi vào DB để chạy câu lệnh stored procedure với tham số đầu vào ở trên
            int numberOfAffectedRows = 0;
            //using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                numberOfAffectedRows = mySqlConnection.Execute(updateStoredProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                var result = Guid.Empty;
                if (numberOfAffectedRows > 0)
                {
                    result = (Guid)recordID;
                }
                return result;
            }

        }

        


    }

}
