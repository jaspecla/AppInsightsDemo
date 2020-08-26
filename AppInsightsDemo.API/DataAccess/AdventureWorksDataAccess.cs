using AppInsightsDemo.API.Models;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace AppInsightsDemo.API.DataAccess
{
  public class AdventureWorksDataAccess : IAdventureWorksDataAccess
  {
    private IDataAccess _dataAccess;
    private TelemetryClient _telemetryClient;
    public AdventureWorksDataAccess(IDataAccess dataAccess, TelemetryClient telemetryClient)
    {
      _dataAccess = dataAccess;
      _telemetryClient = telemetryClient;
    }

    public async Task<Product> GetProductForId(string productId)
    {
      var stopwatch = System.Diagnostics.Stopwatch.StartNew();

      string query;

      if (productId == "720")
      {
        query = GetDelayedProductQuery();
      }
      else if (productId == "999")
      {
        throw new ArgumentException("This is a bad product ID.", "productId");
      }
      else
      {
        query = GetProductQuery();
      }

      var param = _dataAccess.GetParameter("@ProductId", productId);

      var dataReader = await _dataAccess.GetDataReaderAsync(query, new List<DbParameter> { param });

      var products = new List<Product>();

      while (await dataReader.ReadAsync())
      {
        var product = new Product();
        product.ProductId = dataReader["ProductID"].ToString();
        product.Name = dataReader["Name"].ToString();
        product.ProductNumber = dataReader["ProductNumber"].ToString();
        product.Color = dataReader["Color"].ToString();
        product.ListPrice = dataReader["ListPrice"].ToString();
        product.Category = dataReader["Category"].ToString();
        products.Add(product);
      }

      stopwatch.Stop();

      var properties = new Dictionary<string, string> { { "productId", productId } };
      var metrics = new Dictionary<string, double> { { "elapsedTime", stopwatch.ElapsedMilliseconds } };

      _telemetryClient.TrackEvent("DataAccess", properties, metrics);

      // TODO: Warn on more than one product returned

      return products.FirstOrDefault<Product>();
    }

    private string GetProductQuery()
    {
      var query = @"SELECT p.ProductID, p.Name, p.ProductNumber, p.Color, p.ListPrice, cat.Name as 'Category'
                      FROM SalesLT.Product as p
                      INNER JOIN SalesLT.ProductCategory as cat on p.ProductCategoryID = cat.ProductCategoryID
                      WHERE p.ProductID = @ProductId";

      return query;

    }

    private string GetDelayedProductQuery()
    {
      var query = @"WAITFOR DELAY '00:00:10'
                    SELECT p.ProductID, p.Name, p.ProductNumber, p.Color, p.ListPrice, cat.Name as 'Category'
                      FROM SalesLT.Product as p
                      INNER JOIN SalesLT.ProductCategory as cat on p.ProductCategoryID = cat.ProductCategoryID
                      WHERE p.ProductID = @ProductId";

      return query;

    }

  }
}
