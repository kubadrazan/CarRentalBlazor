using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MiniCarRentalAPI.Data;
using MiniCarRentalAPI.Services;
using NuGet.Versioning;
using SharedDataModels;
using SharedDataModels.DTO;

namespace MiniCarRentalAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AcceptationsController : ControllerBase
	{
		private readonly CarRentalContext _context;
		private readonly AzureBlobService _azureBlobService;

		public AcceptationsController(CarRentalContext context, AzureBlobService azureBlobService)
		{
			_context = context;
			_azureBlobService = azureBlobService;
		}

		// GET: api/acceptations/carimage/{rentalId}
		[HttpGet("carimage/{rentalId}")]
		public async Task<IActionResult> GetCarImage(int rentalId)
		{
			try
			{
				var Return = await _context.Returns.FirstOrDefaultAsync(r => r.RentalID == rentalId);
				var Acceptation = await _context.Acceptations.FirstOrDefaultAsync(a => a.ReturnID == Return.ID);
				var image = await _azureBlobService.Download(Acceptation.ImageAzureBlobUri);
				return Ok(image);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// GET: api/acceptations/cardescription/{rentalId}
		[HttpGet("cardescription/{rentalId}")]
		public async Task<IActionResult> GetDescription(int rentalId)
		{
			try
			{
				var Return = await _context.Returns.FirstOrDefaultAsync(r => r.RentalID == rentalId);
				var Acceptation = await _context.Acceptations.FirstOrDefaultAsync(a => a.ReturnID == Return.ID);
				return Ok(Acceptation.Description);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

	}
}
