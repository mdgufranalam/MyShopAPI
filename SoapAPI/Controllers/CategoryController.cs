using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DataAccess;
using ShopAPI.DataAccess.Data;
using ShopAPI.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace ShopAPI.Controllers
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
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                _logger.LogWarning("Delete(int id)");
                _logger.LogInformation("Fetching Categories.", "Get()");
                return Ok(_unitOfWork.Category.GetAll());
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

                var category = _unitOfWork.Category.GetFirstOrDefault(a => a.Id == id);
                if (category == null)
                {
                    _logger.LogInformation("Category not found.", "Get()");
                    return NotFound("Category not found.");
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
                   
                    
                    item.CreatedDateTime = DateTime.Now;
                    _unitOfWork.Category.Add(item);
                    _unitOfWork.Save();
                    _logger.LogInformation("Category created successfully. : " + JsonConvert.SerializeObject(item), "CreateCategory(Category item)");
                    return Ok("Category created successfully.");
                }
                else
                {
                    _logger.LogInformation("ModelState Validation Failed, Model : "+JsonConvert.SerializeObject(item), "CreateCategory(Category item)");
                    return BadRequest(ModelState);
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message, "CreateCategory(Category item)", "Exception");
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
                { var isValid=_unitOfWork.Category.GetFirstOrDefault(a=>a.Id == id);
                    if (isValid == null)
                    {
                        _logger.LogInformation("Category not found.", "EditCategory()");
                        return NotFound("Category not found.");
                    }
                    item.Id = id;
                    _unitOfWork.Category.Update(item);                   
                    _unitOfWork.Save();
                    _logger.LogInformation("Category updated successfully.", "EditCategory()");
                    return Ok("Category updated successfully.");
                }
                else
                {
                    _logger.LogInformation("ModelState validation failed, Category : "+JsonConvert.SerializeObject(item), "EditCategory()");
                    return BadRequest(ModelState);
                }
               
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
                    var item=_unitOfWork.Category.GetFirstOrDefault(u=>u.Id == id); 
                    if(item == null)
                    {
                        return NotFound("Category not found.");
                    }
                   _unitOfWork.Category.Remove(item);
                    _unitOfWork.Save();
                _logger.LogInformation("Category deleted successfully.", "Delete()");
                return Ok("Category deleted successfully.");  
                
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message, "Delete(int id)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

    }
}
