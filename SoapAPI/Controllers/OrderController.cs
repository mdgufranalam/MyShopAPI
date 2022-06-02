using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DataAccess;
using ShopAPI.DataAccess.Data;
using ShopAPI.Models.ViewModel;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryController> _logger;

        public OrderController(ApplicationDbContext context, IUnitOfWork unitOfWork, ILogger<CategoryController> logger)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("{OrderId}")]
        public IActionResult GetOrderDetails(int OrderId)
        {
            try
            {
                var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o=>o.Id==OrderId,includeProperties:"ApplicationUser");
                var orderDetails = _unitOfWork.OrderDetails.GetAll(o=>o.OrderId==OrderId,includeProperties: "Product");
                OrderVM orderVM = new OrderVM(){ OrderDetail=orderDetails,OrderHeader= orderHeader};
                return Ok(orderVM);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }
       



    }
}
