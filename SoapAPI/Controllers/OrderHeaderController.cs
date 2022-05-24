﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopAPI.DataAccess;
using ShopAPI.DataAccess.Data;
using ShopAPI.Models;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public IActionResult Update(UpdateOrderHeader obj)
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
    }
}
