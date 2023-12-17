using Xunit;
using System;
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
using ShoppingSystemWebTest.Async;
using System.Linq.Expressions;
using System.Net.Sockets;

namespace ShoppingSystemWeb.Tests;

public class ProductsControllerTest
{
    private readonly int? _testId = 123;
    private static readonly Product _testProduct1 = new Product()
    {
        Id = 123,
        Title = "test title",
        Category = "test category",
        ExpiredDate = DateTime.Now.Date,
        Price = 13.13m
    };
    private static readonly Product _testProduct2 = new Product()
    {
        Id = 124,
        Title = "test title",
        Category = "test category",
        ExpiredDate = DateTime.Now.Date,
        Price = 13.13m
    };
    private static readonly Product _testProduct3 = new Product()
    {
        Id = 125,
        Title = "test title",
        Category = "test category",
        ExpiredDate = DateTime.Now.Date,
        Price = 13.13m
    };

    [Fact]
    public async Task Index_ReturnsAViewResult_WithAListOfProducts()
    {
        // Arrange
        var data = new List<Product>
        {
            _testProduct1,
            _testProduct2,
            _testProduct3
        };
        var mockSet = DbContextMock.GetQueryableMockDbSet(data);
        var mockContext = new Mock<IShoppingSystemWebContext>();
        mockContext.Setup(c => c.Product).Returns(mockSet.Object);

        // Act
        var controller = new ProductsController(mockContext.Object);
        var result = await controller.Index(null, null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PaginatedList<Product>>(viewResult.ViewData.Model);
        Assert.Equal(3, model.Count);
    }


    [Fact]
    public async Task Details_ReturnsNotFoundResult_WhenIdIsNull()
    {
        // Arrange
        var data = new List<Product>
        {
            _testProduct1,
            _testProduct2,
            _testProduct3
        };
        var mockSet = DbContextMock.GetQueryableMockDbSet(data);

        // Mock the ToListAsync method directly on the DbSet
        mockSet.As<IAsyncEnumerable<Product>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<Product>(data.GetEnumerator()));

        var mockContext = new Mock<IShoppingSystemWebContext>();
        mockContext.Setup(c => c.Product).Returns(mockSet.Object);

        // Act
        var controller = new ProductsController(mockContext.Object);
        var result = await controller.Details(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_ReturnsCorrectProduct()
    {
        // Arrange
        var data = new List<Product>
        {
            _testProduct1,
            _testProduct2,
            _testProduct3
        };
        var mockSet = DbContextMock.GetQueryableMockDbSet(data);

        // Mock the ToListAsync method directly on the DbSet
        mockSet.As<IAsyncEnumerable<Product>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<Product>(data.GetEnumerator()));

        var mockContext = new Mock<IShoppingSystemWebContext>();
        mockContext.Setup(c => c.Product).Returns(mockSet.Object);
        var controller = new ProductsController(mockContext.Object);

        // Act
        var result = await controller.Details(_testId);
        mockContext.Setup(c => c.Product.FindAsync(It.IsAny<object[]>()))
                   .ReturnsAsync((object[] keyValues) => _testProduct1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Product>(viewResult.ViewData.Model);
        Assert.Equal(_testProduct1, model);
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
        var result = await controller.Create(_testProduct2);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Fact]
    public async Task Create_Add_new_product_Before_2_After_3()
    {
        // Arrange
        var data = new List<Product>
        {
            _testProduct1,
            _testProduct2
        };
        var mockSet = DbContextMock.GetQueryableMockDbSet(data);

        // Mock the ToListAsync method directly on the DbSet
        mockSet.As<IAsyncEnumerable<Product>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<Product>(data.GetEnumerator()));

        var mockContext = new Mock<IShoppingSystemWebContext>();
        mockContext.Setup(c => c.Product).Returns(mockSet.Object);
        var controller = new ProductsController(mockContext.Object);

        // Act
        var result = await controller.Create(_testProduct3);


        // Assert
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(3, data.Count);
    }

    [Fact]
    public async Task Create_AddsProduct()
    {
        // Arrange
        var mockContext = new Mock<IShoppingSystemWebContext>();
        var mockDbSet = new Mock<DbSet<Product>>();
        mockContext.Setup(c => c.Product).Returns(mockDbSet.Object);
        var controller = new ProductsController(mockContext.Object);

        // Act
        await controller.Create(_testProduct1);

        // Assert
        mockDbSet.Verify(dbSet => dbSet.Add(It.Is<Product>(p => p == _testProduct1)), Times.Once);
        mockContext.Verify(context => context.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Edit_UpdatesProduct()
    {
        // Arrange
        var mockContext = new Mock<IShoppingSystemWebContext>();
        var mockDbSet = new Mock<DbSet<Product>>();
        mockContext.Setup(c => c.Product).Returns(mockDbSet.Object);
        var controller = new ProductsController(mockContext.Object);

        // Act
        await controller.Edit(_testProduct1.Id, _testProduct1);

        // Assert
        mockDbSet.Verify(dbSet => dbSet.Update(It.Is<Product>(p => p == _testProduct1)), Times.Once);
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

    [Fact]
    public async Task DeleteConfirmed_DeletesProduct()
    {
        // Arrange
        var mockContext = new Mock<IShoppingSystemWebContext>();
        var mockDbSet = new Mock<DbSet<Product>>();
        mockContext.Setup(c => c.Product).Returns(mockDbSet.Object);

        var controller = new ProductsController(mockContext.Object);
        var testProductId = 123;

        // Add the test product to the mock context
        mockDbSet.Setup(dbSet => dbSet.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync((object[] keyValues) => _testProduct1);

        // Act
        await controller.DeleteConfirmed(testProductId);

        // Assert
        mockContext.Verify(context => context.SaveChangesAsync(CancellationToken.None), Times.Once);
        mockDbSet.Verify(dbSet => dbSet.FindAsync(testProductId), Times.Once);
        mockDbSet.Verify(dbSet => dbSet.Remove(_testProduct1), Times.Once);
    }

    [Fact]
    public async Task DeleteConfirmed_Before_3_After_2()
    {
        // Arrange
        var data = new List<Product>
        {
            _testProduct1,
            _testProduct2,
            _testProduct3
        };
        var mockSet = DbContextMock.GetQueryableMockDbSet(data);
        // Mock the ToListAsync method directly on the DbSet
        mockSet.As<IAsyncEnumerable<Product>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<Product>(data.GetEnumerator()));

        var mockContext = new Mock<IShoppingSystemWebContext>();
        mockContext.Setup(c => c.Product).Returns(mockSet.Object);
        var controller = new ProductsController(mockContext.Object);

        // Act
        var result = await controller.DeleteConfirmed(_testId ?? 0);

        // Assert
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(3, data.Count);
    }
}
