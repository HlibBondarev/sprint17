using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShoppingSystemWeb.Controllers;
using ShoppingSystemWeb.Data;
using ShoppingSystemWeb.Models;
using System.Net.Sockets;
using System.Reflection.Metadata;


namespace ShoppingSystemWebTest;

public class ProductsControllerTest
{
    // TODO: 2.a) Create the private Mocker class. It must encapsulate  construction and configuration 
    //            of all mocks and the system under test:
    //            - Mock<ShoppingSystemWebContext> _mockContext;
    //            - Mock<DbSet<Product>> _mockSet;

    private class Mocker
    {


        private readonly ProductsController _productsController;
        private readonly Mock<ShoppingSystemWebContext> _mockContext =
            new Mock<ShoppingSystemWebContext>(new Mock<DbContextOptionsBuilder<ShoppingSystemWebContext>>().Object);
        Mock<DbSet<Product>> _mockSet = new Mock<DbSet<Product>>();
        public Mock<ProductsController> MockProductsController { get; set; }
        public Mock<ShoppingSystemWebContext> MockProductsContext { get; set; }

        public Mocker()
        {
            var data = new List<Product> { _testProduct }.AsQueryable();

            _mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _mockContext.Setup(m => m.Product).Returns(_mockSet.Object);
            MockProductsContext = _mockContext;
            MockProductsController = new Mock<ProductsController>();
            _productsController = new ProductsController(_mockContext.Object);
        }

        public ProductsController Build()
        {
            return _productsController;
        }
    }

    private static readonly int? ID = 123;
    private static readonly Product _testProduct = new Product()
    {
        Id = ID ?? 0,
        Title = "test title",
        Category = "test category",
        ExpiredDate = DateTime.Now,
        Price = 13.13m
    };


    [Fact]
    // Checks the returned result
    public async Task CreateTest_Returns_RedirectToActionResult()
    {
        // Arrange
        var moker = new Mocker();
        var sut = moker.Build();

        // Act
        var result = await sut.Create(_testProduct);
        var resultType = Assert.IsType<RedirectToActionResult>(result).ToString();

        // Assert
        Assert.Equal(result.GetType().ToString(), resultType);
    }

    [Fact]
    // Tests calls to the Add and SaveChangesAsync methods
    public async Task CreateTest_saves_a_Product_via_context()
    {
        // Arrange
        var moker = new Mocker();
        var sut = moker.Build();

        // Act
        var result = await sut.Create(_testProduct);

        // Assert
        moker.MockProductsContext.Verify(m => m.Add(It.IsAny<Product>()), Times.Once());
        moker.MockProductsContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }


    [Fact]
    // Checks the returned result
    public async Task DetailsTest_Returns_RedirectToActionResult()
    {
        // Arrange
        var moker = new Mocker();
        var sut = moker.Build();

        // Act

        var result = await sut.Details(ID); // EXEPTION!!!

        var resultType = Assert.IsType<RedirectToActionResult>(result).ToString();

        // Assert
        Assert.Equal(result.GetType().ToString(), resultType);
    }

}


