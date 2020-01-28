using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppInsightsDemo.API.Tests
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void ShouldAlwaysPass()
    {
      Assert.IsTrue(true);
    }

    [TestMethod]
    public void ShouldAlwaysFail()
    {
      Assert.IsTrue(false);
    }
  }
}
