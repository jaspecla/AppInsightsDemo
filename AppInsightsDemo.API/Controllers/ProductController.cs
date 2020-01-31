using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppInsightsDemo.API.DataAccess;
using AppInsightsDemo.API.Models;
using AppInsightsDemo.API.Options;
using Microsoft.ApplicationInsights;
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
    private TelemetryClient _telemetryClient;
    public ProductController(IAdventureWorksDataAccess dataAccess, TelemetryClient telemetryClient)
    {
      _dataAccess = dataAccess;
      _telemetryClient = telemetryClient;
    }

    /// <summary>
    /// Get a product from the AdventureWorks database.  Use product number 720 to induce a ten second delay in the database.  Use product number 707 to induce a ten second delay in code.
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpGet("{productId}", Name = "GetProduct")]
    public async Task<IActionResult> GetProduct(string productId)
    {
      var properties = new Dictionary<string, string>();
      properties.Add("productId", productId);

      var stopwatch = System.Diagnostics.Stopwatch.StartNew();

      var productForId = await _dataAccess.GetProductForId(productId);

      if (productId == "707")
      {
        await Task.Delay(10000);
      }

      stopwatch.Stop();
      var metrics = new Dictionary<string, double>() { { "elapsedTime", stopwatch.ElapsedMilliseconds } };

      _telemetryClient.TrackEvent("GetProduct", properties, metrics);

      if (productForId == null)
      {
        return NotFound();
      }

      return Ok(productForId);
    }
  }
}