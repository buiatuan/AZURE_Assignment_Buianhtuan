using BuianhtuanAssignment.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuianhtuanAssignment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RouteController<T> : ControllerBase
    {
        protected readonly PracticeDbContext _context;
        protected readonly ILogger<T> _logger;

        public RouteController(PracticeDbContext practiceDbContext, ILogger<T> logger)
        {
            _context = practiceDbContext;
            _logger = logger;
        }
    }
}
