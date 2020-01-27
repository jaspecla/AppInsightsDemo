using AppInsightsDemo.API.Models;
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
    public AdventureWorksDataAccess(IDataAccess dataAccess)
    {
      _dataAccess = dataAccess;
    }

    public async Task<Product> GetProductForId(string productId)
    {
      var query = @"SELECT p.ProductID, p.Name, p.ProductNumber, p.Color, p.ListPrice, cat.Name as 'Category'
                      FROM SalesLT.Product as p
                      INNER JOIN SalesLT.ProductCategory as cat on p.ProductCategoryID = cat.ProductCategoryID
                      WHERE p.ProductID = @ProductId";

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
        product.Category = dataReader["Category"].ToString();
        products.Add(product);
      }

      // TODO: Warn on more than one product returned

      return products.FirstOrDefault<Product>();
    }
  }
}
