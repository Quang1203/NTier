using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web07.GD.ndquang.BL;
using MISA.Web07.GD.ndquang.Common.Entity;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.Web07.GD.ndquang.API.NTier
{
    public class SubjectsController : BasesController<Subject>
    {
        #region Field

        private ISubjectBL _subjectBL;

        #endregion

        #region Constructor

        public SubjectsController(ISubjectBL subjectBL) : base(subjectBL)
        {
            _subjectBL = subjectBL;
        }

        #endregion
    }
}
