using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web07.GD.ndquang.BL;
using MISA.Web07.GD.ndquang.Common.Entity;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.Web07.GD.ndquang.API.NTier
{
    
    public class GroupsController : BasesController<Groups>
    {
        #region Field

        private IGroupBL _groupBL;

        #endregion

        #region Constructor

        public GroupsController(IGroupBL groupBL) : base(groupBL)
        {
            _groupBL = groupBL;
        }

        #endregion

    }
}
