using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppInsightsDemo.API.Models
{
  public class Product
  {
    public string ProductId { get; set; }
    public string Name { get; set; }
    public string ProductNumber { get; set; }
    public string Color { get; set; }
    public string ListPrice { get; set; }
    public string Category { get; set; }
  }
}
