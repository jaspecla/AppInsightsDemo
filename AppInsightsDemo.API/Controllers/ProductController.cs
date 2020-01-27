using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppInsightsDemo.API.DataAccess;
using AppInsightsDemo.API.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AppInsightsDemo.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private IAdventureWorksDataAccess _dataAccess;
    public ProductController(IAdventureWorksDataAccess dataAccess)
    {
      _dataAccess = dataAccess;
    }

    [HttpGet("{productId}", Name = "GetProduct")]
    public async Task<IActionResult> GetProduct(string productId)
    {
      var productForId = await _dataAccess.GetProductForId(productId);

      return Ok(productForId);
    }
  }
}