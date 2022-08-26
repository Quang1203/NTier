using Dapper;
using MISA.Web07.GD.ndquang.Common.Entity;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web07.GD.ndquang.DL
{
    public class GroupDL : BaseDL<Groups>, IGroupDL
    {
        //public List<Group> GetAllGroups()
        //{
        //    // Khởi tạo kết nối tới DB MySQL
        //    string connectionString = "Server=localhost;Port=3306;Database=misa.web07.gd.nguyendangquang;Uid=root;Pwd=123456789;";
        //    var mySqlConnection = new MySqlConnection(connectionString);

        //    // Chuẩn bị câu lệnh truy vấn
        //    string getAllGroupsCommand = "SELECT * FROM `group`;";

        //    // Thực hiện gọi vào DB để chạy câu lệnh truy vấn ở trên
        //    var groups = mySqlConnection.Query<Group>(getAllGroupsCommand);

        //    return (List<Group>)groups;
        //}

    }
}
