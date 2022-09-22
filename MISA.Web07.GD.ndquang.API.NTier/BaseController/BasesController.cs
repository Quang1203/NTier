using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web07.GD.ndquang.BL;
using MISA.Web07.GD.ndquang.BL.Exceptions;
using MISA.Web07.GD.ndquang.Common.Resources;

namespace MISA.Web07.GD.ndquang.API.NTier
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {
        #region Field

        private IBaseBL<T> _baseBL;

        #endregion

        #region Constructor

        public BasesController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// API Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Tất cả bản ghi</returns>
        /// Created by: NDQuang (09/06/2022)
        [HttpGet]
        public virtual IActionResult GetAllRecords()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _baseBL.GetAllRecords());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, Resource.error_expception);
            }
        }

        /// <summary>
        /// API Thêm mới 1 bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi cần thêm mới</param>
        /// <returns>ID của bản ghi vừa thêm mới</returns>
        /// Created by: NDQuang (24/08/2022)
        [HttpPost]
        public virtual IActionResult InsertOneRecord([FromBody] T record)
        {
            try
            {
                //var validateResult = HandleError.ValidateEntity(ModelState, HttpContext);
                //if (validateResult != null)
                //{
                //    return StatusCode(StatusCodes.Status400BadRequest, validateResult);
                //}

                var recordID = _baseBL.InsertOneRecord(record);

                if (recordID != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status201Created, recordID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Resource.failedOperation);
                }
            }
            catch (ValidateException ex)
            {
                var res = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Data
                };
                return StatusCode(StatusCodes.Status400BadRequest, res);
            }
            //catch (MySqlException mySqlException)
            //{
            //    return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateDuplicateCodeErrorResult(mySqlException, HttpContext));
            //}
            //catch (Exception exception)
            //{
            //    return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(exception, HttpContext));
            //}
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, Resource.error_expception);
            }
        }

        /// <summary>
        /// API Sửa một bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi cần sửa</param>
        /// <param name="recordID">ID Đối tượng bản ghi cần sửa</param>
        /// <returns>ID bản ghi được sửa</returns>
        /// Created by: NDQuang (24/08/2022)
        [HttpPut("{recordID}")]
        public virtual IActionResult UpdateOneRecord([FromRoute] Guid recordID, [FromBody] T record)
        {
            try
            {
                //var validateResult = HandleError.ValidateEntity(ModelState, HttpContext);
                //if (validateResult != null)
                //{
                //    return StatusCode(StatusCodes.Status400BadRequest, validateResult);
                //}

                var ID = _baseBL.UpdateOneRecord(recordID, record);

                if (ID != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status200OK, recordID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Resource.failedOperation);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, Resource.error_expception);
            }
        }

        #endregion
    }
}
