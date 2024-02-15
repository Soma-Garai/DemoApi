using DemoApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DemoApi.Controllers
{
	[ApiController]
	[Route("api/v1/info")]
	public class InformationController : Controller
	{
		private readonly ILogger<InformationController> _logger;

		public InformationController(ILogger<InformationController> logger)
		{
			_logger = logger;
		}

		[Authorize(Policy = "BearerPolicy")]
		[HttpGet]
		[Route("getInformations")]
		public async Task<IActionResult> GetInformations()
		{	
			List<Information> informationList = await Task.Run (() => new List<Information>
			{
				new Information { info_name = "Weather", info_desc = "Current weather forecast for your location" },
				new Information { info_name = "News", info_desc = "Latest news headlines from around the world" },
				new Information { info_name = "Stocks", info_desc = "Real-time stock market updates and trends" }
				// Add more sample data as needed
			});
			return Ok(informationList);
		}
	}
}