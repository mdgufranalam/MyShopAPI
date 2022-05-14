using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShopAPI.DataAccess;
using MyShopAPI.DataAccess.Data;
using MyShopAPI.Models;
using Newtonsoft.Json;

namespace MyShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ApplicationDbContext context, IUnitOfWork unitOfWork,ILogger<CategoryController> logger)
        {

            _context = context;
            this._unitOfWork = unitOfWork;
           
                _logger = logger;
        }
        [HttpGet]
        [ResponseCache(Duration =60,Location =ResponseCacheLocation.Client)]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.Category.GetCategory());
            }
            catch (Exception ex)
            {
               _logger.LogWarning(ex.Message, "Get()", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
        [HttpGet("{id}")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
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
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("New Category Created : " + JsonConvert.SerializeObject(item), "CreateCategory(Category item)");
                    item.CreatedDateTime = DateTime.Now;
                    _unitOfWork.Category.CreateCategory(item);
                    _unitOfWork.Save();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                // _logger.LogWarning(ex.Message, "CreateCategory(Category item)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditCategory(int id, Category item)
        {
            try
            {
                Category editeditem=null;
                if (ModelState.IsValid)
                {
                     editeditem = _unitOfWork.Category.Edit(id, item);                   
                    if (editeditem == null)
                    {
                        return NotFound("Category Not Found.");
                    }
                    _unitOfWork.Save();

                }
                return Ok(editeditem);
            }
            catch (Exception ex)
            {
                // _logger.LogWarning(ex.Message, "EditCategory(int id,Category item)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var rs = _unitOfWork.Category.DeleteCategory(id);                    
                    if (rs == 0)
                    {
                        return NotFound("Category Not Found.");
                    }
                    _unitOfWork.Save();
                    return Ok(rs);
                }
                else
                    return BadRequest();
                
            }
            catch (Exception ex)
            {
                //_logger.LogWarning(ex.Message, "Delete(int id)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

    }
}
