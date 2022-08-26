using MISA.Web07.GD.ndquang.Common.Entity;
using MISA.Web07.GD.ndquang.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web07.GD.ndquang.BL
{
    public class StorageRoomBL : BaseBL<StorageRoom>, IStorageRoomBL
    {
        #region Field

        private IStorageRoomDL _storageRoomDL;

        #endregion

        #region Constructor

        public StorageRoomBL(IStorageRoomDL storageRoomDL) : base(storageRoomDL)
        {
            _storageRoomDL = storageRoomDL;
        }

        #endregion


    }
}

