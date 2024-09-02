using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListWebApp.Controllers;
using TodoListWebApp.Data;
using TodoListWebApp.Models;
using Xunit;

namespace TodoListWebApp.Tests.Controllers
{
    public class TodoControllerTests
    {
        private readonly Mock<TodoListDbContext> _mockContext;
        private readonly TodoController _controller;

        public TodoControllerTests()
        {
            _mockContext = new Mock<TodoListDbContext>();
            _controller = new TodoController(_mockContext.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfTodoItems()
        {
            // Arrange
            var todos = new List<TodoModel>
            {
                new TodoModel { Id = 1, Description = "Task 1", Importance = "High", Completed = false },
                new TodoModel { Id = 2, Description = "Task 2", Importance = "Low", Completed = true }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<TodoModel>>();
            mockSet.As<IQueryable<TodoModel>>().Setup(m => m.Provider).Returns(todos.Provider);
            mockSet.As<IQueryable<TodoModel>>().Setup(m => m.Expression).Returns(todos.Expression);
            mockSet.As<IQueryable<TodoModel>>().Setup(m => m.ElementType).Returns(todos.ElementType);
            mockSet.As<IQueryable<TodoModel>>().Setup(m => m.GetEnumerator()).Returns(todos.GetEnumerator());

            _mockContext.Setup(c => c.TodoModel).Returns(mockSet.Object);

            // Act
            var result = await _controller.Index(null, null, null, null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<TodoModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }
    }
}
