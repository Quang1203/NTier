using MISA.Web07.GD.ndquang.Common.Entity;
using MISA.Web07.GD.ndquang.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web07.GD.ndquang.BL
{
    public class GroupBL : BaseBL<Groups>, IGroupBL
    {

        #region Field

        private IGroupDL _groupDL;

        #endregion

        #region Constructor

        public GroupBL(IGroupDL groupDL) : base(groupDL)
        {
            _groupDL = groupDL;
        }

        #endregion

        //#region Field

        //private IGroupDL _groupDL;

        //#endregion

        //#region Constructor

        //public GroupBL(IGroupDL groupDL)
        //{
        //    _groupDL = groupDL;
        //}

        //#endregion

        //#region Method
        ///// <summary>
        ///// API Lấy toàn bộ danh sách tổ bộ môn
        ///// </summary>
        ///// <returns>Danh sách tổ bộ môn</returns>
        ///// Created by: NDQuang (17/08/2022)
        //public List<Group> GetAllGroups()
        //{
        //    return _groupDL.GetAllGroups();
        //} 
        //#endregion

    }
}
