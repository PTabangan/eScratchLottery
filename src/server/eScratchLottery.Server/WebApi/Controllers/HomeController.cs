using System.IO;
using Microsoft.AspNetCore.Mvc;
using static eScratchLottery.Server.WebApi.DirectoryDelegates;

namespace eScratchLottery.Server.WebApi.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly GetWwwRootDirectory _getWwwRoot;
        public HomeController(GetWwwRootDirectory getWwwRoot)
        {
            _getWwwRoot = getWwwRoot;
        }

        [HttpGet("{*clientInfo}")]
        public IActionResult Index()
        {
            string file = Path.Combine(_getWwwRoot(), "index.html");
            return PhysicalFile(file, "text/html");
        }

    }
}
