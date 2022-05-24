using Microsoft.AspNetCore.Mvc;
using ShopAPI.DataAccess;
using ShopAPI.DataAccess.Data;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductController> _logger;

        public ApplicationUserController(ApplicationDbContext context, IUnitOfWork unitOfWork, ILogger<ProductController> logger)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        [HttpGet("{applicationUserId}")]
        public IActionResult CartCount(string applicationUserId)
        {
            try
            {
                return Ok(_unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == applicationUserId));
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
