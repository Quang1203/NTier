using MISA.Web07.GD.ndquang.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web07.GD.ndquang.Common.Entity
{
    public class EmployeeDetail
    {
        #region Property

        /// <summary>
        /// thông tin cá nhân nhân viên
        /// </summary>
        public Employee? employee { get; set; }  

        /// <summary>
        /// List môn
        /// </summary>
        public List<Subject>? ListSubject { get; set; }

        /// <summary>
        /// List kho phòng
        /// </summary>
        public List<StorageRoom>? ListStorageRoom { get; set; }

        #endregion
    }
}
