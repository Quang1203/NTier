using Dapper;
using MISA.Web07.GD.ndquang.Common.Entity;
using MISA.Web07.GD.ndquang.Common.Entity.DTO;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web07.GD.ndquang.DL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {
        public string CheckDuplicateEmployeeCode(string employeeCode)
        {
            // Khởi tạo kết nối tới DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // Khai báo tên stored procedure Check Duplicate
            string checkStoredProcedureName = "Proc_employee_CheckDuplicate";

            // Chuẩn bị tham số đầu vào của stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("v_EmployeeCode", employeeCode);

            // Thực hiện gọi vào DB để chạy câu lệnh DELETE với tham số đầu vào ở trên
            string checkEmployeeCode = mySqlConnection.QueryFirstOrDefault<string>(checkStoredProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);


            return checkEmployeeCode;
        }

        public int DeleteEmployeeByID(Guid employeeID)
        {
            // Khởi tạo kết nối tới DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // Khai báo tên stored procedure DELETE
            string deleteStoredProcedureName = "Proc_employee_DeleteOne";

            // Chuẩn bị tham số đầu vào của stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("v_EmployeeID", employeeID);



            // Thực hiện gọi vào DB để chạy câu lệnh DELETE với tham số đầu vào ở trên
            int numberOfAffectedRows = mySqlConnection.Execute(deleteStoredProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return numberOfAffectedRows;
        }


        public PagingData<Employee> FilterEmployees(string? keyword, Guid? groupID, Guid? storageRoomID, Guid? subjectID, int pageSize, int pageNumber)
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
            var totalCount = multipleResults.Read<long>().Single();
            return new PagingData<Employee>()
            {
                Data = employees,
                TotalCount = totalCount
            }; 
        }

        public Employee GetEmployeeByID(Guid employeeID)
        {
            // Khởi tạo kết nối tới DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // Chuẩn bị tên Stored procedure
            string storedProcedureName = "Proc_employee_GetByEmployeeID";

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@v_EmployeeID", employeeID);

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var employee = mySqlConnection.QueryFirstOrDefault<Employee>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            return employee;

        }



        public string GetNewEmployeeCode()
        {
            // Khởi tạo kết nối tới DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // Chuẩn bị tên stored procedure
            string storedProcedureName = "Proc_employee_GetMaxCode";

            // Thực hiện gọi vào DB để chạy stored procedure ở trên
            string maxEmployeeCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

            // Xử lý sinh mã nhân viên mới tự động tăng
            // Cắt chuỗi mã nhân viên lớn nhất trong hệ thống để lấy phần số
            // Mã nhân viên mới = "NV" + Giá trị cắt chuỗi ở  trên + 1
            // "NV99997"
            string newEmployeeCode = "NV" + (Int64.Parse(maxEmployeeCode.Substring(2)) + 1).ToString();

            // Trả về dữ liệu cho client
            return newEmployeeCode;
        }

        //public int InsertEmployee(Employee employee)
        //{
            // Khởi tạo kết nối tới DB MySQL
            //string connectionString = "Server=localhost;Port=3306;Database=misa.web07.gd.nguyendangquang;Uid=root;Pwd=123456789;";
            //var mySqlConnection = new MySqlConnection(connectionString);

            //// Chuẩn bị câu lệnh INSERT INTO
            ////INSERT INTO employee(EmployeeID, EmployeeCode, EmployeeName, TelNumber, DateOfBirth, Gender, IdentityNumber, Email, Salary, CreateDate, CreateBy, ModifiedDate, ModifiedBy, GroupID, GroupName, SubjectID, SubjectName, StorageRoomID, StorageRoomName)
            //string insertEmployeeCommand = "INSERT INTO employee (EmployeeID, EmployeeCode, EmployeeName, TelNumber, DateOfBirth, Gender, IdentityNumber, Email, Salary, CreateDate, CreateBy, ModifiedDate, ModifiedBy, GroupID, SubjectID, StorageRoomID) " +
            //    "VALUES (@EmployeeID, @EmployeeCode, @EmployeeName, @TelNumber, @DateOfBirth, @Gender, @IdentityNumber, @Email, @Salary, @CreateDate, @CreateBy, @ModifiedDate, @ModifiedBy, @GroupID, @SubjectID, @StorageRoomID);";

            //// Chuẩn bị tham số đầu vào cho câu lệnh INSERT INTO
            //var employeeID = Guid.NewGuid();
            //var parameters = new DynamicParameters();
            //parameters.Add("@EmployeeID", employeeID);
            //parameters.Add("@EmployeeCode", employee.EmployeeCode);
            //parameters.Add("@EmployeeName", employee.EmployeeName);
            //parameters.Add("@TelNumber", employee.TelNumber);
            //parameters.Add("@DateOfBirth", employee.DateOfBirth);
            //parameters.Add("@Gender", employee.Gender);
            //parameters.Add("@IdentityNumber", employee.IdentityNumber);
            //parameters.Add("@Email", employee.Email);
            //parameters.Add("@Salary", employee.Salary);
            ////parameters.Add("@WorkStatus", employee.WorkStatus);
            //parameters.Add("@CreateDate", employee.CreateDate);
            //parameters.Add("@CreateBy", employee.CreateBy);
            //parameters.Add("@ModifiedDate", employee.ModifiedDate);
            //parameters.Add("@ModifiedBy", employee.ModifiedBy);
            //parameters.Add("@GroupID", employee.GroupID);
            //parameters.Add("@SubjectID", employee.SubjectID);
            //parameters.Add("@StorageRoomID", employee.StorageRoomID);

            //// Thực hiện gọi vào DB để chạy câu lệnh INSERT INTO với tham số đầu vào ở trên
            //int numberOfAffectedRows = mySqlConnection.Execute(insertEmployeeCommand, parameters);

            //return numberOfAffectedRows;

        //}

        //public int UpdateEmployee(Guid employeeID, Employee employee)
        //{
        //    // Khởi tạo kết nối tới DB MySQL
        //    string connectionString = "Server=localhost;Port=3306;Database=misa.web07.gd.nguyendangquang;Uid=root;Pwd=123456789;";
        //    var mySqlConnection = new MySqlConnection(connectionString);

        //    // Chuẩn bị câu lệnh UPDATE
        //    string updateEmployeeCommand = "UPDATE employee " +
        //        "SET " +
        //        "EmployeeCode = @EmployeeCode, " +
        //        "EmployeeName = @EmployeeName, " +
        //        "TelNumber = @TelNumber, " +
        //        "DateOfBirth = @DateOfBirth, " +
        //        "Gender = @Gender, " +
        //        "IdentityNumber = @IdentityNumber, " +
        //        "Email = @Email, " +
        //        "Salary = @Salary, " +
        //        "ModifiedDate = @ModifiedDate, " +
        //        "ModifiedBy = @ModifiedBy, " +
        //        "GroupID = @GroupID, " +
        //        "SubjectID = @SubjectID, " +
        //        "StorageRoomID = @StorageRoomID " +
        //    "WHERE EmployeeID = @EmployeeID;";

        //    // Chuẩn bị tham số đầu vào cho câu lệnh UPDATE
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@EmployeeID", employeeID);
        //    parameters.Add("@EmployeeCode", employee.EmployeeCode);
        //    parameters.Add("@EmployeeName", employee.EmployeeName);
        //    parameters.Add("@TelNumber", employee.TelNumber);
        //    parameters.Add("@DateOfBirth", employee.DateOfBirth);
        //    parameters.Add("@Gender", employee.Gender);
        //    parameters.Add("@IdentityNumber", employee.IdentityNumber);
        //    parameters.Add("@Email", employee.Email);
        //    parameters.Add("@Salary", employee.Salary);
        //    //parameters.Add("@WorkStatus", employee.WorkStatus);
        //    parameters.Add("@ModifiedDate", employee.ModifiedDate);
        //    parameters.Add("@ModifiedBy", employee.ModifiedBy);
        //    parameters.Add("@GroupID", employee.GroupID);
        //    parameters.Add("@SubjectID", employee.SubjectID);
        //    parameters.Add("@StorageRoomID", employee.StorageRoomID);

        //    // Thực hiện gọi vào DB để chạy câu lệnh UPDATE với tham số đầu vào ở trên
        //    int numberOfAffectedRows = mySqlConnection.Execute(updateEmployeeCommand, parameters);

        //    return numberOfAffectedRows;

        //}
    }
}
