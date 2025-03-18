using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TestWebApplication.Controllers;
using TestWebApplication.Models;

namespace UnitTests
{
    public class CompanyItemControllerTests
    {
        private CompanyContext _context;
        private CompanyItemController _controller;        

        public CompanyItemControllerTests()
        {
            var options = new DbContextOptionsBuilder<CompanyContext>()
                .UseInMemoryDatabase(databaseName: "DemoDatabase")
                .Options;

            _context = new CompanyContext(options);
            Mock<ILogger<CompanyItemController>> mockLogger = new();
            _controller = new CompanyItemController(_context, mockLogger.Object);
        }

        [Fact]
        public async Task GetCompanyItems_ReturnsAllItems()
        {           
            // Arrange
            var companyItems = new List<CompanyItem>
            {
                new CompanyItem { Name = "Company A", Isin = "ISIN12345678", StockTicker = "aa", Exchange="ss"},
                new CompanyItem { Name = "Company B", Isin = "ISIN12345678", StockTicker = "aa", Exchange="ss"}
            };

            await _context.CompanyItems.AddRangeAsync(companyItems);
            var result1 = await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetCompanyItems();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<CompanyItem>>>(result);
            var returnValue = Assert.IsType<List<CompanyItem>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetCompanyItemByIsin_ReturnsItem()
        {
            // Arrange
            var companyItem = new CompanyItem { Name = "Company A", Isin = "ISIN12345678", StockTicker = "aa", Exchange = "ss" };
            await _context.CompanyItems.AddAsync(companyItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetCompanyItemByIsin("ISIN12345678");

            // Assert
            var actionResult = Assert.IsType<ActionResult<CompanyItem>>(result);
            var returnValue = Assert.IsType<CompanyItem>(actionResult.Value);
            Assert.Equal("Company A", returnValue.Name);
        }

        [Fact]
        public async Task GetCompanyItemByIsin_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetCompanyItemByIsin("InvalidISIN");

            // Assert
            var actionResult = Assert.IsType<ActionResult<CompanyItem>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetCompanyItemById_ReturnsItem()
        {
            var companyItem = new CompanyItem
            { CompanyID = 4, Name = "Company A", Isin = "ISIN001", StockTicker = "aa", Exchange = "ss" };
            await _context.CompanyItems.AddAsync(companyItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetCompanyItem(4);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CompanyItem>>(result);
            var returnValue = Assert.IsType<CompanyItem>(actionResult.Value);
            Assert.Equal("Company A", returnValue.Name);
        }

        [Fact]
        public async Task GetCompanyItemByIs_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetCompanyItem(0);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CompanyItem>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task PostCompanyItem_CreatesNewItem()
        {
            var options = new DbContextOptionsBuilder<CompanyContext>()
                .UseInMemoryDatabase(databaseName: "OtherDB")
                .Options;
            _context = new CompanyContext(options);
            Mock<ILogger<CompanyItemController>> mockLogger = new();
            _controller = new CompanyItemController(_context, mockLogger.Object);

            // Arrange
            var companyItem = new CompanyItem { Name = "Company A", Isin = "ISIN12345677", StockTicker = "aa", Exchange = "ss" };

            // Act
            var result = await _controller.PostCompanyItem(companyItem);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CompanyItem>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<CompanyItem>(createdAtActionResult.Value);
            Assert.Equal("Company A", returnValue.Name);
        }

        [Fact]
        public async Task PostCompanyItem_CreatesNewItemWithInvalidIsinLength()
        {
            // Arrange
            var companyItem = new CompanyItem { Name = "Company A", Isin = "ISIN12", StockTicker = "aa", Exchange = "ss" };

            // Act
            var result = await _controller.PostCompanyItem(companyItem);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CompanyItem>>(result);
            var createdAtActionResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<ErrorModel>(createdAtActionResult.Value);

            Assert.Equal(400, createdAtActionResult.StatusCode);

            Assert.Equal("Bad Request:This field must have a length of 12 characters.", returnValue.ErrorDetails[0].Details);
        }

        [Fact]
        public async Task PostCompanyItem_CreatesNewItemWithInvalidIsin()
        {
            // Arrange
            var companyItem = new CompanyItem { Name = "Company A", Isin = "123456789000", StockTicker = "aa", Exchange = "ss" };

            // Act
            var result = await _controller.PostCompanyItem(companyItem);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CompanyItem>>(result);
            var createdAtActionResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<ErrorModel>(createdAtActionResult.Value);

            Assert.Equal(400, createdAtActionResult.StatusCode);

            Assert.Equal("Bad Request:The company Item must have an Isin field starting with two letters", returnValue.ErrorDetails[0].Details);
        }

        [Fact]
        public async Task PostCompanyItem_EmptyModel()
        {           
            // Act
            var result = await _controller.PostCompanyItem(new CompanyItem());

            // Assert
            var actionResult = Assert.IsType<ActionResult<CompanyItem>>(result);           
            var createdAtActionResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);           
        }
    }
}