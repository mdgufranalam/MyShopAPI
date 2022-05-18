using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DataAccess;
using ShopAPI.DataAccess.Data;
using ShopAPI.Models;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopingCartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryController> _logger;

        public ShopingCartController(ApplicationDbContext context, IUnitOfWork unitOfWork, ILogger<CategoryController> logger)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        [HttpPost]
        public IActionResult AddToCart(ShoppingCart input)
        {
            try
            {
                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
            u => u.ApplicationUserId == input.ApplicationUserId && u.ProductId == input.ProductId);


                if (cartFromDb == null)
                {

                    _unitOfWork.ShoppingCart.Add(input);
                    _unitOfWork.Save();
                    //HttpContext.Session.SetInt32(SD.SessionCart,
                    //    _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);
                    return Ok(_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == input.ApplicationUserId).ToList().Count);
                }
                else
                {
                    _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, input.Count);
                    _unitOfWork.Save();
                }
                return Ok();

            }
            catch (Exception Ex)
            {
                return BadRequest("Exception occured while AddToCart : "+Ex.Message);    
               
            }
        }


        [HttpGet("[action]/{applicationUserId}")]        
        public IActionResult CartCount(string applicationUserId)
        {
            try
            {
                return Ok(_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == applicationUserId).ToList().Count);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
