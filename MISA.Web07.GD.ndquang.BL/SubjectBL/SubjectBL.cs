using MISA.Web07.GD.ndquang.Common.Entity;
using MISA.Web07.GD.ndquang.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web07.GD.ndquang.BL
{
    public class SubjectBL : BaseBL<Subject>, ISubjectBL
    {
        #region Field

        private ISubjectDL _subjectDL;

        #endregion

        #region Constructor

        public SubjectBL(ISubjectDL subjectDL) : base(subjectDL)
        {
            _subjectDL = subjectDL;
        }

        #endregion
    }
}
