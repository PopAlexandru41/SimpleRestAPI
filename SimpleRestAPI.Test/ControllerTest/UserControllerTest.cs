using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SimpleRestAPI.Controllers;
using SimpleRestAPI.Models;
using SimpleRestAPI.Services.Interface;
using System.Net;

namespace SimpleRestAPI.Test.ControllerTest
{
    public class UserControllerTest
    {
        #region Startup

        UserController _controller;
        Mock<ILogger<UserController>> _logger = new Mock<ILogger<UserController>>();
        Mock<IUserService> _services = new Mock<IUserService>();

        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }
        #endregion 

        #region GetUnitTests
        [Fact]
        public void GetTest_WhenNoDataAreReturned()
        {
            //Arrange
            _controller = new UserController(_logger.Object, _services.Object);

            //Act
            var result = _controller.Get();
            //Assert
            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact]
        public void GetTest_WhenDataAreReturned()
        {
            //Arrange
            _controller = new UserController(_logger.Object, _services.Object);
            var user1 = new User { Name = "Name1", Password = "Password1" };
            var user2 = new User { Name = "Name2", Password = "Password2" };
            List<User> listSource = new List<User>();
            listSource.Add(user1);
            listSource.Add(user2);
            var dbSet = GetQueryableMockDbSet(listSource);

            //Act
            var CodeSnippets = _services.Setup(m => m.Get()).Returns(dbSet);
            var result = _controller.Get();

            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(objectResult.Value);
            Assert.Equal(2, model.Count());
        }

        #endregion

        #region GetByNameTest
        [Fact]
        public void GetByIdTest_WhenNoDataAreReturn()
        {
            //Arrange
            _controller = new UserController(_logger.Object, _services.Object);

            //Act
            var result = _controller.Get(null);

            //Assert 
            Assert.IsType<StatusCodeResult>(result);
        }
        [Fact]
        public void GetByIdTest_WhenDataAreReturned()
        {
            //Arrange
            _controller = new UserController(_logger.Object, _services.Object);
            var user1 = new User { Name = "Name1", Password = "Password1" };
            var user2 = new User { Name = "Name2", Password = "Password2" };
            List<User> listSource = new List<User>();
            listSource.Add(user1);
            listSource.Add(user2);
            var dbSet = GetQueryableMockDbSet(listSource);
            _services.Setup(m => m.GetByName("Name1")).Returns(user1);

            //Act
            var result = _controller.Get("Name1");

            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var model = Assert.IsAssignableFrom<User>(objectResult.Value);
            Assert.Equal("Password1", model.Password);
        }
        #endregion

        #region PostUnitTest

        [Fact]
        public void PostTest_WhenNoData()
        {
            //Arrange
            _controller = new UserController(_logger.Object, _services.Object);

            //Act
            var result = _controller.Post(null);

            //Assert
            var resultStatusCode = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(resultStatusCode.StatusCode, (int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void PostTest_WhenSendData()
        {
            //Arrange
            _controller = new UserController(_logger.Object, _services.Object);
            var user1 = new User { Name = "Name1", Password = "Password1" };
            var codeSnippedAdded = _services.Setup(m => m.Post(user1));
            //Act
            var result = _controller.Post(user1);

            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(Constants.CreateUserMessage, objectResult.Value);
            Assert.Equal(objectResult.StatusCode, (int)HttpStatusCode.Created);
        }

        #endregion

        #region PutUnitTest

        [Fact]
        public void PutTest_WhenNoData()
        {
            //Arrange
            _controller = new UserController(_logger.Object, _services.Object);
            var user1 = new User { Name = "Name1", Password = "Password1" };
            _controller.Post(user1);

            //Act
            var result = _controller.Put(null);

            //Assert
            var resultStatusCode = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(resultStatusCode.StatusCode, (int)HttpStatusCode.NotFound);
        }

        [Fact]
        public void PutTest_WhenSendData()
        {
            //Arrange
            _controller = new UserController(_logger.Object, _services.Object);
            var user1 = new User { Name = "Name1", Password = "Password1" };
            _controller.Post(user1);
            user1.Name = "TestModify";
            var codeSnippedAdded = _services.Setup(m => m.Post(user1));

            //Act
            var result = _controller.Put(user1);


            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(Constants.UpdateUserMessage, objectResult.Value);
            Assert.Equal(objectResult.StatusCode, (int)HttpStatusCode.Accepted);
        }

        #endregion

        #region DeleteUnitTest

        [Fact]
        public void DeleteTest_WhenNoData()
        {
            //Arrange
            _controller = new UserController(_logger.Object, _services.Object);
            var user1 = new User { Name = "Name1", Password = "Password1" };
            _controller.Post(user1);

            //Act
            var result = _controller.Delete(Guid.Empty);

            //Assert
            var resultStatusCode = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(resultStatusCode.StatusCode, (int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void DeleteTest_WhenSendData()
        {
            //Arrange
            _controller = new UserController(_logger.Object, _services.Object);
            var myguid = Guid.NewGuid();
            var user1 = new User { Id = myguid, Name = "Name1", Password = "Password1" };
            _controller.Post(user1);
            _services.Setup(m => m.Post(user1));
            var codeSnippedAdded = _services.Setup(m => m.Delete(user1));

            //Act
            var result = _controller.Delete(myguid);


            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(Constants.DeleteUserMessage, objectResult.Value);
            Assert.Equal(objectResult.StatusCode, (int)HttpStatusCode.OK);
        }

        #endregion
    }
}
