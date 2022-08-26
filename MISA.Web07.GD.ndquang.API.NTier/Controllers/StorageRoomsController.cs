using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web07.GD.ndquang.BL;
using MISA.Web07.GD.ndquang.Common.Entity;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.Web07.GD.ndquang.API.NTier
{
    public class StorageRoomsController : BasesController<StorageRoom>
    {
        #region Field

        private IStorageRoomBL _storageRoomBL;

        #endregion

        #region Constructor

        public StorageRoomsController(IStorageRoomBL storageRoomBL) : base(storageRoomBL)
        {
            _storageRoomBL = storageRoomBL;
        }

        #endregion
    }
}
