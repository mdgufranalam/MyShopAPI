using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DataAccess;
using ShopAPI.DataAccess.Data;
using ShopAPI.Models;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryController> _logger;

        public OrderDetailsController(ApplicationDbContext context, IUnitOfWork unitOfWork, ILogger<CategoryController> logger)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpPost]       
        public IActionResult AddOrderHeader(OrderDetail item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.OrderDetails.Add(item);
                    _unitOfWork.Save();
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                // _logger.LogWarning(ex.Message, "CreateProduct(Product item)", "Exception");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

    }
}
