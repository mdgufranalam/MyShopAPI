using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopAPI.DataAccess;
using ShopAPI.DataAccess.Data;
using ShopAPI.Models;
using System.Globalization;
using System.Linq;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductController> _logger;
        private readonly IWebHostEnvironment hostEnvironment;

        public ProductController(ApplicationDbContext context, IUnitOfWork unitOfWork, ILogger<ProductController> logger, IWebHostEnvironment hostEnvironment)
        {

            _context = context;
            this._unitOfWork = unitOfWork;

            _logger = logger;
            this.hostEnvironment = hostEnvironment;
        }
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public IActionResult Get()
        {
            try
            {
                //return Ok(_unitOfWork.Product.GetProduct());
                return Ok(_unitOfWork.Product.GetAll(includeProperties:"Category").OrderByDescending(a=>a.LastUpdateDate));
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

                var Product = _unitOfWork.Product.GetFirstOrDefault(u=>u.Id==id, includeProperties: "Category");
                if (Product == null)
                {
                    return NotFound("Product not found.");
                }
                return Ok(Product);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message, "Get(int id)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateProduct(Product item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("New Product Created : " + JsonConvert.SerializeObject(item), "CreateProduct(Product item)");
                    item.CreatedDateTime = DateTime.Now;
                    item.LastUpdateDate = DateTime.Now;
                    _unitOfWork.Product.Add(item);
                    _unitOfWork.Save();
                }
                return Ok("Product created successfully.");
            }
            catch (Exception ex)
            {
                // _logger.LogWarning(ex.Message, "CreateProduct(Product item)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditProduct(int id, Product item)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    //editeditem = _unitOfWork.Product.Update(item);
                    item.Id= id;
                    _unitOfWork.Product.Update(item);                    
                    _unitOfWork.Save();
                    return Ok("Product updated successfully.");
                }
                else
                {
                    return BadRequest(ModelState);  
                }
                
            }
            catch (Exception ex)
            {
                // _logger.LogWarning(ex.Message, "EditProduct(int id,Product item)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {                
                    var item=_unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
                    if (item == null)
                    {
                        return NotFound("Product not found.");
                    }
                    _unitOfWork.Product.Remove(item);
                    _unitOfWork.Save();                  
                    
                    return Ok("Product deleted successfully.");
                
            }
            catch (Exception ex)
            {
                //_logger.LogWarning(ex.Message, "Delete(int id)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("UploadFile")]
        public IActionResult UploadFile(IFormFile? file)
        {
            try
            {
               
                string wwwRootPath = hostEnvironment.ContentRootPath;
                if (file != null)
                {
                    String myDocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    //if (product.ImageUrl != null)
                    //{
                    //    var oldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));
                    //    if (System.IO.File.Exists(oldImagePath))
                    //    {
                    //        System.IO.File.Delete(oldImagePath);
                    //    }
                    //}
                    if (!Directory.Exists(Path.Combine(uploads, fileName + extension)))
                    {
                        Directory.CreateDirectory(Path.Combine(uploads, fileName + extension));
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    //If we store on Web
                    //product.ImageUrl = @"\images\products\" + fileName + extension;
                    //Store on APP Server
                    //product.ImageUrl = Path.Combine(uploads, fileName + extension);
                    return Ok(Path.Combine(uploads, fileName + extension));
                }
                //if (product.Id == 0)
                //{
                //    _unitOfWork.Product.Add(product);
                //}
                //else
                //{
                //    _unitOfWork.Product.Update(product);
                //}
                //_unitOfWork.Save();

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("[action]/{searchstring}")]       
        public IActionResult SearchProducts(string searchstring)
        {
            try
            {
                var allItems = _unitOfWork.Product.GetAll(filter: y => (y.Title.ToLower().Contains(searchstring.ToLower()) || y.Category.Name.ToLower().Contains(searchstring.ToLower()) || y.Category.Description.ToLower().Contains(searchstring.ToLower()) || y.Description.ToLower().Contains(searchstring.ToLower())), includeProperties: "Category");
                if (allItems.Count() == 0)
                {
                    return NotFound("Product not found.");
                }
                return Ok(allItems);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message, "Get(int id)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
        [HttpGet("[action]/{category}")]       
        public IActionResult CategoryWiseProduct(string category)
        {
            try
            {
                var allItems = _unitOfWork.Product.GetAll(filter:(y=>(y.Category.Name.ToLower()==category.ToLower())),includeProperties: "Category");
                if (allItems.Count()==0)
                {
                    return NotFound("No products avaliable for this category.");
                }
                return Ok(allItems);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message, "Get(int id)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

    }
}
