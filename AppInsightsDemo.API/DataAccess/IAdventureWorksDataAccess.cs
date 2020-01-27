using AppInsightsDemo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppInsightsDemo.API.DataAccess
{
  public interface IAdventureWorksDataAccess
  {
    Task<Product> GetProductForId(string productId);
  }
}
