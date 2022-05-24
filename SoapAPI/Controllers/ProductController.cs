using Microsoft.AspNetCore.Authorization;
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
       //[Authorize]
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
        //[Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public IActionResult CategoryWiseProduct(string category)
        {
            try
            {
               
                if(category.ToLower()=="all")
                {
                    return Ok(_unitOfWork.Product.GetAll(includeProperties: "Category").OrderByDescending(a => a.LastUpdateDate));
                }
                var filteredItems = _unitOfWork.Product.GetAll(filter:(y=>(y.Category.Name.ToLower()==category.ToLower())),includeProperties: "Category");
                if (filteredItems.Count()==0)
                {
                    return NotFound("No products avaliable for this category.");
                }
                return Ok(filteredItems);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message, "Get(int id)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpGet("[action]")]
        [Authorize]
        public IActionResult PagingProducts(int? pageno,int? pagesize)
        {
            try
            {
                var products = _unitOfWork.Product.GetAll(includeProperties:"Category").OrderByDescending(a=>a.LastUpdateDate);
                if (products.Count() == 0)
                {
                    return NotFound();
                }
                var currentPageNumber = pageno ?? 1;
                var currentPageSize = pagesize ?? 5;

                return Ok(products.Skip((currentPageNumber-1)*currentPageSize).Take(currentPageSize));
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("[action]")]
         [Authorize]
        public IActionResult SortProduct(string? criteria,string? sort)
        {
            try
            {
                IOrderedQueryable<Product> products;
                var sortcriteria = criteria ?? "lastupdatedate";
                var sortwise = sort ?? "desc";
                switch (sortcriteria)
                {
                    case "lastupdatedate":
                        if (sortwise == "desc")
                            products = _unitOfWork.Product.GetAllIQueryable(includeProperties: "Category").OrderByDescending(a => a.LastUpdateDate);
                        else
                            products = _unitOfWork.Product.GetAllIQueryable(includeProperties: "Category").OrderBy(p => p.LastUpdateDate);
                        break;
                    case "price":
                        if (sortwise == "desc")
                            products = _unitOfWork.Product.GetAllIQueryable(includeProperties: "Category").OrderByDescending(p => p.Price);
                        else
                            products = _unitOfWork.Product.GetAllIQueryable(includeProperties: "Category").OrderBy(p => p.Price);
                        break;
                    case "category":
                        if (sortwise == "desc")
                            products = _unitOfWork.Product.GetAllIQueryable(includeProperties: "Category").OrderByDescending(p => p.Category.Name);
                        else
                            products = _unitOfWork.Product.GetAllIQueryable(includeProperties: "Category").OrderBy(p => p.Category.Name);
                        break;
                    case "betteroffer":
                        //DiscountPerc is not present in table so i converted into list
                        var productstemp = _unitOfWork.Product.GetAllIQueryable(includeProperties: "Category").ToList().OrderByDescending(p => p.DiscountPerc);
                           return Ok(productstemp);
                      
                    default:
                        products = (IOrderedQueryable<Product>)_unitOfWork.Product.GetAllIQueryable(includeProperties: "Category");
                        break;
                }
                return Ok(products);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
