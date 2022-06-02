using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopAPI.DataAccess;
using ShopAPI.DataAccess.Data;
using ShopAPI.Models;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderHeaderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryController> _logger;

        public OrderHeaderController(ApplicationDbContext context, IUnitOfWork unitOfWork, ILogger<CategoryController> logger)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult AddOrderHeader(OrderHeader item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.OrderHeader.Add(item);
                    _unitOfWork.Save();
                    return Ok(item);
                }
                return BadRequest("OrderHeader Validation Failed.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public IActionResult UpdateOrderHeader(UpdateOrderHeader obj)
        {
            try
            {
                _unitOfWork.OrderHeader.UpdateStatus(obj.Id, obj.OrderStatus, obj.PaymentStatus); ;
                _unitOfWork.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpGet("[action]/{OrderId}")]
        public IActionResult GetFirstOrDefault(string OrderId)
        {
            try
            {
               var OrderHeader= _unitOfWork.OrderHeader.GetFirstOrDefault(o=>o.Id ==Convert.ToInt32(OrderId),includeProperties: "ApplicationUser");
                return Ok(OrderHeader);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
        [HttpGet("[action]/{OrderId}")]
        public IActionResult GetFirstOrDefaultNoTrack(string OrderId)
        {
            try
            {
                var OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == Convert.ToInt32(OrderId), includeProperties: "ApplicationUser",tracked:false);
                return Ok(OrderHeader);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public IActionResult Update(OrderHeader obj)
        {
            try
            {
                _unitOfWork.OrderHeader.Update(obj);
                _unitOfWork.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public void ShipOrder(OrderHeader order)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == order.Id, tracked: false);
            orderHeader.TrackingNumber = order.TrackingNumber;
            orderHeader.Carrier = order.Carrier;
            orderHeader.OrderStatus = order.OrderStatus;
            orderHeader.ShippingDate = DateTime.Now;           
            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
        }
        [HttpGet("[action]")]
        public IActionResult GetAll()
        {            
           return Ok(_unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser"));
           
        }
        [HttpGet("[action]/{appUser}")]
        public IActionResult GetApplicationUserOrderHeader(string appUser)
        {
           
            var header = _unitOfWork.OrderHeader.GetAll(o => o.ApplicationUserId == appUser, includeProperties: "ApplicationUser");
            if (header == null)
            {
                return NotFound("Order Header not found.");
            }
            return Ok(header);

        }
        
        [HttpPost("[action]")]
        public IActionResult UpdateStripePaymentID(UpdateStripePayment obj)
        {

            _unitOfWork.OrderHeader.UpdateStripePaymentID(obj.id, obj.session, obj.paymentItentId);
            _unitOfWork.Save();
            return Ok();

        }
    }
}
