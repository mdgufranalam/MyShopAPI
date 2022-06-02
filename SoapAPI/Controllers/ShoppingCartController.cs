using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DataAccess;
using ShopAPI.DataAccess.Data;
using ShopAPI.Models;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryController> _logger;

        public ShoppingCartController(ApplicationDbContext context, IUnitOfWork unitOfWork, ILogger<CategoryController> logger)
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
                return Ok(_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == input.ApplicationUserId).ToList().Count);

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
                return Ok(_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == applicationUserId,includeProperties: "Products").ToList().Count);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }

        }

        [HttpGet("[action]/{AppUserId}")]
        public IActionResult GetShoppingCart(string AppUserId)
        {
            try
            {
                return Ok(_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == AppUserId,includeProperties:"Products").ToList());

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }  
        }

        [HttpGet("[action]/{AppUserId}")]
        public IActionResult GetShoppingCartWithAppUserInfo(string AppUserId)
        {
            try
            {
                return Ok(_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == AppUserId, includeProperties: "Products,ApplicationUser").ToList());

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpGet("[action]/{cartId}")]
        public IActionResult Plus(int cartId)
        {
            try
            {
                var cart=_unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
                _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
                _unitOfWork.Save();
                var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpGet("[action]/{cartId}")]
        public IActionResult Minus(int cartId)
        {
            try
            {
                var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
                if (cart.Count <= 1)
                {
                    _unitOfWork.ShoppingCart.Remove(cart);
                    var countt = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count - 1;
                    _unitOfWork.Save();
                    return Ok(countt);
                }
                else
                {
                    _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
                }
                _unitOfWork.Save();
                var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count();
                return Ok(count);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
        [HttpGet("[action]/{cartId}")]
        public IActionResult Remove(int cartId)
        {
            try
            {
                var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.Save();                
                var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                return Ok(count);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpDelete("[action]/{AppUserId}")]
        public IActionResult DeleteShoppingCart(string AppUserId)
        {
            try
            {
                List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId ==
            AppUserId).ToList();
                _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
                _unitOfWork.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
}

