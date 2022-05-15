using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DataAccess;
using ShopAPI.DataAccess.Data;
using ShopAPI.Models;
using Newtonsoft.Json;

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
        public IActionResult Get()
        {
            try
            {
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
                    _logger.LogInformation("Category created successfully. : " + JsonConvert.SerializeObject(item), "CreateCategory(Category item)");
                    item.CreatedDateTime = DateTime.Now;
                    _unitOfWork.Category.Add(item);
                    _unitOfWork.Save();
                    return Ok("Category created successfully.");
                }
                else
                {
                    return BadRequest(ModelState);
                }
               
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
                { var isValid=_unitOfWork.Category.GetFirstOrDefault(a=>a.Id == id);
                    if (isValid == null)
                    {
                        return NotFound("Category not found.");
                    }
                    item.Id = id;
                    _unitOfWork.Category.Update(item);                   
                    _unitOfWork.Save();
                    return Ok("Category updated successfully.");
                }
                else
                {
                    return BadRequest(ModelState);
                }
               
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
                    var item=_unitOfWork.Category.GetFirstOrDefault(u=>u.Id == id); 
                    if(item == null)
                    {
                        return NotFound("Category not found.");
                    }
                   _unitOfWork.Category.Remove(item);
                    _unitOfWork.Save();
                    return Ok("Category deleted successfully.");  
                
            }
            catch (Exception ex)
            {
                //_logger.LogWarning(ex.Message, "Delete(int id)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

    }
}
