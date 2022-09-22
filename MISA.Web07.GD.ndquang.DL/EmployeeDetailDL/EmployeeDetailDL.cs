using Dapper;
using MISA.Web07.GD.ndquang.Common.Entity;
using MISA.Web07.GD.ndquang.Common.Entity.DTO;
using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web07.GD.ndquang.DL
{
    public class EmployeeDetailDL : IEmployeeDetailDL
    {
        
        public EmployeeDetail GetEmployeeDetailByID(Guid employeeID)
        {
            // Khởi tạo kết nối tới DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);
            var mySqlConnection2 = new MySqlConnection(DatabaseContext.ConnectionString);

            // Chuẩn bị tên Stored procedure
            string storedProcedureGetEmployee = "Proc_employee_GetByEmployeeID";
            string storedProcedureGetSubject = "Proc_GetAllEmployeeSubjectsByEmployeeID";
            string storedProcedureGetStorageRoom = "Proc_GetEmployeeStorageRoomsByEmployeeID";

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@v_EmployeeID", employeeID);



            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var employee = mySqlConnection.QueryFirstOrDefault<Employee>(storedProcedureGetEmployee, parameters, commandType: System.Data.CommandType.StoredProcedure);
            var subject = mySqlConnection.QueryMultiple(storedProcedureGetSubject, parameters, commandType: System.Data.CommandType.StoredProcedure);
            var storageRoom = mySqlConnection2.QueryMultiple(storedProcedureGetStorageRoom, parameters, commandType: System.Data.CommandType.StoredProcedure);


            if (employee != null && subject != null && storageRoom != null) 
            {
                var employeeDetail = new EmployeeDetail();
                employeeDetail.employee = (Employee?)employee;
                employeeDetail.ListSubject = subject.Read<Subject>().ToList();
                employeeDetail.ListStorageRoom = storageRoom.Read<StorageRoom>().ToList();

                return (EmployeeDetail)employeeDetail;
            }
            else
            {
                return null;
            }

        }

        public PagingData<EmployeeDetail> FilterEmployeeDetails(string? keyword, Guid? groupID, Guid? storageRoomID, Guid? subjectID, int pageSize, int pageNumber)
        {
            // Khởi tạo kết nối tới DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // Chuẩn bị tên Stored procedure
            string storedProcedureName = "Proc_employee_GetPaging";

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@v_Offset", (pageNumber - 1) * pageSize);
            parameters.Add("@v_Limit", pageSize);
            parameters.Add("@v_Sort", "ModifiedDate DESC");

            var orConditions = new List<string>();
            var andConditions = new List<string>();
            string whereClause = "";

            if (keyword != null)
            {
                orConditions.Add($"EmployeeCode LIKE '%{keyword}%'");
                orConditions.Add($"EmployeeName LIKE '%{keyword}%'");
                orConditions.Add($"TelNumber LIKE '%{keyword}%'");
            }
            if (orConditions.Count > 0)
            {
                whereClause = $"({string.Join(" OR ", orConditions)})";
            }

            if (groupID != null)
            {
                andConditions.Add($"GroupID LIKE '%{groupID}%'");
            }
            if (storageRoomID != null)
            {
                andConditions.Add($"StorageRoomID LIKE '%{storageRoomID}%'");
            }
            if (subjectID != null)
            {
                andConditions.Add($"SubjectID LIKE '%{subjectID}%'");
            }

            if (andConditions.Count > 0)
            {
                whereClause += $" AND {string.Join(" AND ", andConditions)}";
            }

            parameters.Add("@v_Where", whereClause);

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var multipleResults = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            var employees = multipleResults.Read<Employee>().ToList();
            var listEmployees = new List<EmployeeDetail>();
            foreach (var item in employees)
            {
                listEmployees.Add(GetEmployeeDetailByID(item.EmployeeID));
            }
            var totalCount = multipleResults.Read<long>().Single();
            return new PagingData<EmployeeDetail>()
            {
                Data = listEmployees,
                TotalCount = totalCount
            };
        }

        public Guid InsertOneEmployeeDetail(EmployeeDetail employeeDetail)
        {
            // Khai báo tên stored procedure INSERT
            string insertStoredProcedureEmployee = "Proc_employee_InsertOne";
            string insertStoredProcedureSubject = "Proc_employee_subject_InsertOne";
            string insertStoredProcedureStorageRoom = "Proc_employee_storageroom_InsertOne";

            // Chuẩn bị tham số đầu vào của stored procedure
            var employee = employeeDetail.employee;
            var ListSubject = employeeDetail.ListSubject;
            var ListStorageRoom = employeeDetail.ListStorageRoom;

            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);
            int numberOfAffectedRows1 = 0;
            int numberOfAffectedRows2 = 0;
            int numberOfAffectedRows3 = 0;

            // Thêm vào bảng nhân viên
            var properties = typeof(Employee).GetProperties().Skip(1);
            var parameters = new DynamicParameters();
            var empID = Guid.NewGuid();
            parameters.Add("v_EmployeeID", empID);
            foreach (var property in properties)
            {
                string propertyName = $"v_{property.Name}"; // v_DepartmentCode
                var propertyValue = property.GetValue(employee); // Đọc vào object record --> lấy được value của property tên là DepartmentCode
                parameters.Add(propertyName, propertyValue);
            }
            
            numberOfAffectedRows1 = mySqlConnection.Execute(insertStoredProcedureEmployee, parameters, commandType: System.Data.CommandType.StoredProcedure);

            // Thêm vào bảng nhân viên - môn
            if (ListSubject.Count > 0 )
            {
                var propertiesEmployeeSubjects = typeof(EmployeeSubject).GetProperties().Skip(2);
                var parametersEmployeeSubject = new DynamicParameters();
                foreach (var itemSubject in ListSubject)
                {
                    parametersEmployeeSubject.Add("v_Employee_SubjectID", Guid.NewGuid());
                    parametersEmployeeSubject.Add("v_EmployeeID", empID); 
                    parametersEmployeeSubject.Add("v_SubjectID", itemSubject.SubjectID); 
                    parametersEmployeeSubject.Add("v_CreateDate", itemSubject.CreateDate); 
                    parametersEmployeeSubject.Add("v_CreateBy", itemSubject.CreateBy); 
                    parametersEmployeeSubject.Add("v_ModifiedDate", itemSubject.ModifiedDate); 
                    parametersEmployeeSubject.Add("v_ModifiedBy", itemSubject.ModifiedBy);

                    numberOfAffectedRows2 = mySqlConnection.Execute(insertStoredProcedureSubject, parametersEmployeeSubject, commandType: System.Data.CommandType.StoredProcedure);
                    //foreach (var propertyEmployeeSubject in propertiesEmployeeSubjects)
                    //{
                    //    string propertyNameEmployeeSubject = $"v_{propertyEmployeeSubject.Name}"; // v_DepartmentCode
                    //    var propertyValueEmployeeSubject = propertyEmployeeSubject?.GetValue(itemSubject); // Đọc vào object record --> lấy được value của property tên là DepartmentCode
                    //    parametersEmployeeSubject.Add(propertyNameEmployeeSubject, propertyValueEmployeeSubject);
                    //}
                }

                
                
            }

            // Thêm vào bảng nhân viên - kho phòng
            if (ListStorageRoom.Count > 0)
            {
                var propertiesEmployeeStoragerooms = typeof(EmployeeStorageroom).GetProperties().Skip(2);
                var parametersEmployeeStorageroom = new DynamicParameters();
                foreach (var itemStorageRoom in ListStorageRoom)
                {
                    parametersEmployeeStorageroom.Add("v_Employee_StorageroomID", Guid.NewGuid());
                    parametersEmployeeStorageroom.Add("v_EmployeeID", empID);
                    parametersEmployeeStorageroom.Add("v_StorageroomID", itemStorageRoom.StorageRoomID);
                    parametersEmployeeStorageroom.Add("v_CreateDate", itemStorageRoom.CreateDate);
                    parametersEmployeeStorageroom.Add("v_CreateBy", itemStorageRoom.CreateBy);
                    parametersEmployeeStorageroom.Add("v_ModifiedDate", itemStorageRoom.ModifiedDate);
                    parametersEmployeeStorageroom.Add("v_ModifiedBy", itemStorageRoom.ModifiedBy);
                    //foreach (var propertyEmployeeStorageroom in propertiesEmployeeStoragerooms)
                    //{
                    //    string propertyNameEmployeeStorageroom = $"v_{propertyEmployeeStorageroom.Name}"; // v_DepartmentCode
                    //    var propertyValueEmployeeStorageroom = propertyEmployeeStorageroom.GetValue(itemStorageRoom); // Đọc vào object record --> lấy được value của property tên là DepartmentCode
                    //    parametersEmployeeStorageroom.Add(propertyNameEmployeeStorageroom, propertyValueEmployeeStorageroom);
                    //}
                    numberOfAffectedRows3 = mySqlConnection.Execute(insertStoredProcedureStorageRoom, parametersEmployeeStorageroom, commandType: System.Data.CommandType.StoredProcedure);

                }


            }
            

            var result = Guid.Empty;
            if (numberOfAffectedRows1 > 0)
            {
                var primaryKeyProperty = typeof(Employee).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
                var newId = empID;
                if (newId != null)
                {
                    result = (Guid)newId;
                }
            }
            return result;
            
        }

        public Guid UpdateOneEmployeeDetail(Guid employeeID, EmployeeDetail employeeDetail)
        {
            // Khai báo tên stored procedure Update
            string updateStoredProcedureEmployee = "Proc_employee_UpdateOne";
            string deleteStoredProcedureSubject = "Proc_employee_subject_DeleteAll";
            string insertStoredProcedureSubject = "Proc_employee_subject_InsertOne";
            string deleteStoredProcedureStorageRoom = "Proc_employee_storageroom_DeleteAll";
            string insertStoredProcedureStorageRoom = "Proc_employee_storageroom_InsertOne";

            // Chuẩn bị tham số đầu vào của stored procedure
            var employee = employeeDetail.employee;
            var ListSubject = employeeDetail.ListSubject;
            var ListStorageRoom = employeeDetail.ListStorageRoom;

            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);
            int numberOfAffectedRows1 = 0;
            int numberOfAffectedRows2 = 0;
            int numberOfAffectedRows3 = 0;
            int numberOfAffectedRows4 = 0;
            int numberOfAffectedRows5 = 0;

            // Sửa thông tin nhân viên
            // Chuẩn bị tham số đầu vào của stored procedure
            var properties = typeof(Employee).GetProperties();
            var parameters = new DynamicParameters();
            var primaryKeyProperty = typeof(Employee).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
            primaryKeyProperty.SetValue(employee, employeeID);

            foreach (var property in properties)
            {
                string propertyName = $"v_{property.Name}"; // v_DepartmentCode
                var propertyValue = property.GetValue(employee); // Đọc vào object record --> lấy được value của property tên là DepartmentCode
                parameters.Add(propertyName, propertyValue);
            }

            //using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            numberOfAffectedRows1 = mySqlConnection.Execute(updateStoredProcedureEmployee, parameters, commandType: System.Data.CommandType.StoredProcedure);


            // Xóa tất cả bản ghi ở bảng nhân viên - môn có employeeID tương ứng
            // Chuẩn bị tham số đầu vào của stored procedure delete subject
            var parametersDeleteSubject = new DynamicParameters();
            parametersDeleteSubject.Add("v_EmployeeID", employeeID);


            // Thực hiện gọi vào DB để chạy câu lệnh DELETE với tham số đầu vào ở trên
            numberOfAffectedRows2 = mySqlConnection.Execute(deleteStoredProcedureSubject, parametersDeleteSubject, commandType: System.Data.CommandType.StoredProcedure);


            // Thêm vào bảng nhân viên - môn
            if (ListSubject.Count > 0)
            {
                var propertiesEmployeeSubjects = typeof(EmployeeSubject).GetProperties().Skip(2);
                var parametersEmployeeSubject = new DynamicParameters();
                foreach (var itemSubject in ListSubject)
                {
                    parametersEmployeeSubject.Add("v_Employee_SubjectID", Guid.NewGuid());
                    parametersEmployeeSubject.Add("v_EmployeeID", employeeID);
                    parametersEmployeeSubject.Add("v_SubjectID", itemSubject.SubjectID);
                    parametersEmployeeSubject.Add("v_CreateDate", itemSubject.CreateDate);
                    parametersEmployeeSubject.Add("v_CreateBy", itemSubject.CreateBy);
                    parametersEmployeeSubject.Add("v_ModifiedDate", itemSubject.ModifiedDate);
                    parametersEmployeeSubject.Add("v_ModifiedBy", itemSubject.ModifiedBy);

                    numberOfAffectedRows3 = mySqlConnection.Execute(insertStoredProcedureSubject, parametersEmployeeSubject, commandType: System.Data.CommandType.StoredProcedure);
                   
                }
            }

            // Xóa tất cả bản ghi ở bảng nhân viên - kho phòng có employeeID tương ứng
            // Chuẩn bị tham số đầu vào của stored procedure delete storageRoom
            var parametersDeleteStorageRoom = new DynamicParameters();
            parametersDeleteStorageRoom.Add("v_EmployeeID", employeeID);


            // Thực hiện gọi vào DB để chạy câu lệnh DELETE với tham số đầu vào ở trên
            numberOfAffectedRows4 = mySqlConnection.Execute(deleteStoredProcedureStorageRoom, parametersDeleteStorageRoom, commandType: System.Data.CommandType.StoredProcedure);


            // Thêm vào bảng nhân viên - kho phòng
            if (ListStorageRoom.Count > 0)
            {
                var propertiesEmployeeStoragerooms = typeof(EmployeeStorageroom).GetProperties().Skip(2);
                var parametersEmployeeStorageroom = new DynamicParameters();
                foreach (var itemStorageRoom in ListStorageRoom)
                {
                    parametersEmployeeStorageroom.Add("v_Employee_StorageroomID", Guid.NewGuid());
                    parametersEmployeeStorageroom.Add("v_EmployeeID", employeeID);
                    parametersEmployeeStorageroom.Add("v_StorageroomID", itemStorageRoom.StorageRoomID);
                    parametersEmployeeStorageroom.Add("v_CreateDate", itemStorageRoom.CreateDate);
                    parametersEmployeeStorageroom.Add("v_CreateBy", itemStorageRoom.CreateBy);
                    parametersEmployeeStorageroom.Add("v_ModifiedDate", itemStorageRoom.ModifiedDate);
                    parametersEmployeeStorageroom.Add("v_ModifiedBy", itemStorageRoom.ModifiedBy);
                    
                    numberOfAffectedRows5 = mySqlConnection.Execute(insertStoredProcedureStorageRoom, parametersEmployeeStorageroom, commandType: System.Data.CommandType.StoredProcedure);

                }
            }

            var result = Guid.Empty;
            if (numberOfAffectedRows1 > 0)
            {
                result = (Guid)employeeID;
                
            }
            return result;
        }
    }
}
