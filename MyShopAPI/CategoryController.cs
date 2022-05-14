using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShopAPI.DataAccess;
using MyShopAPI.DataAccess.Data;
using MyShopAPI.Models;

namespace MyShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ILogger<CategoryController> logger, ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _context = context;
            this._unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.Category.GetCategory());
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message,"Get()","Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {

                var category = _unitOfWork.Category.GetCategoryById(id);
                if (category == null)
                {
                    return NotFound("Category Not Found.");
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message, "Get(int id)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateCategory(Category item)
        {
            try
            {
                _unitOfWork.Category.CreateCategory(item);
                _unitOfWork.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message, "CreateCategory(Category item)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPut("{id")]
        public IActionResult EditCategory(int id,Category item)
        {
            try
            {
                var editeditem=_unitOfWork.Category.Edit(id,item);
                _unitOfWork.Save();
                if (editeditem == null)
                {
                    return NotFound("Category Not Found!");
                }
                return Ok(editeditem);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message, "EditCategory(int id,Category item)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var rs = _unitOfWork.Category.DeleteCategory(id);
                _unitOfWork.Save();
                if (rs == 0)
                {
                    return NotFound("Category Not Found!");
                }
                return Ok(rs);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message, "Delete(int id)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

    }
}
