using Xunit;
using Microsoft.AspNetCore.Mvc;
using ShoppingSystemWeb.Controllers;
using ShoppingSystemWeb.Data;
using ShoppingSystemWeb.Models;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingSystemWeb.Services;
using Microsoft.EntityFrameworkCore;
using ShoppingSystemWebTest;

namespace ShoppingSystemWeb.Tests;

public class ProductsControllerTest
{
    private static readonly Product _testProduct = new Product()
    {
        Id = 123,
        Title = "test title",
        Category = "test category",
        ExpiredDate = DateTime.Now,
        Price = 13.13m                                                                                  
    };

    [Fact]
    public async Task Index_ReturnsAViewResult_WithAListOfProducts()
    {
        // Arrange
        var mockContext = new Mock<IShoppingSystemWebContext>();
        var mockDbSet = new Mock<DbSet<Product>>();
        var entities = new List<Product>
        {
            _testProduct
        };
        mockContext.Setup(c => c.Product).Returns(DbContextMock.GetQueryableMockDbSet<Product>(entities));
        var controller = new ProductsController(mockContext.Object);

        // Act
        var result = await controller.Index(null, null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PaginatedList<Product>>(viewResult.ViewData.Model);
        Assert.Equal(1, model.Count);
    }

    [Fact]
    public async Task Details_ReturnsNotFoundResult_WhenIdIsNull()
    {
        // Arrange
        var mockContext = new Mock<IShoppingSystemWebContext>();
        var controller = new ProductsController(mockContext.Object);

        // Act
        var result = await controller.Details(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsRedirectToActionResult_WhenModelStateIsValid()
    {
        // Arrange
        var mockContext = new Mock<IShoppingSystemWebContext>();
        var mockDbSet = new Mock<DbSet<Product>>();
        mockContext.Setup(c => c.Product).Returns(mockDbSet.Object);

        var controller = new ProductsController(mockContext.Object);

        // Act
        var result = await controller.Create(new Product
        {
            Id = 123,
            Title = "test title",
            Category = "test category",
            ExpiredDate = DateTime.Now,
            Price = 13.13m
        });

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }
    [Fact]
    public async Task Create_AddsProductToMock_WhenModelStateIsValid()
    {
        // Arrange
        var mockContext = new Mock<IShoppingSystemWebContext>();
        var mockDbSet = new Mock<DbSet<Product>>();
        mockContext.Setup(c => c.Product).Returns(mockDbSet.Object);

        var controller = new ProductsController(mockContext.Object);
        var testProduct = new Product
        {
            Id = 123,
            Title = "test title",
            Category = "test category",
            ExpiredDate = DateTime.Now,
            Price = 13.13m
        };

        // Act
        await controller.Create(testProduct);

        // Assert
        mockDbSet.Verify(dbSet => dbSet.Add(It.Is<Product>(p => p == testProduct)), Times.Once);
        mockContext.Verify(context => context.SaveChangesAsync(CancellationToken.None), Times.Once);
    }


    [Fact]
    public async Task Edit_ReturnsNotFoundResult_WhenIdIsNull()
    {
        // Arrange
        var mockContext = new Mock<IShoppingSystemWebContext>();
        var controller = new ProductsController(mockContext.Object);

        // Act
        var result = await controller.Edit(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
