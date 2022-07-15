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
    public class StudentsControllerTest
    {
        #region Startup

        StudentsController _controller;
        Mock<ILogger<StudentsController>> _logger = new Mock<ILogger<StudentsController>>();
        Mock<IStudentsService> _services = new Mock<IStudentsService>();

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
            _controller = new StudentsController(_logger.Object, _services.Object);

            //Act
            var result = _controller.Get();
            //Assert
            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact]
        public void GetTest_WhenDataAreReturned()
        {
            //Arrange
            _controller = new StudentsController(_logger.Object, _services.Object);
            var student1 = new Students { Name = "Name1", Age = 1, FinalNote = 1, Observation = "Observacion1" };
            var student2 = new Students { Name = "Name2", Age = 2, FinalNote = 2, Observation = "Observacion2" };
            List<Students> listSource = new List<Students>();
            listSource.Add(student1);
            listSource.Add(student2);
            var dbSet = GetQueryableMockDbSet(listSource);

            //Act
            var CodeSnippets = _services.Setup(m => m.Get()).Returns(dbSet);
            var result = _controller.Get();

            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Students>>(objectResult.Value);
            Assert.Equal(2, model.Count());
        }

        #endregion

        #region GetByNameTest
        [Fact]
        public void GetByIdTest_WhenNoDataAreReturn()
        {
            //Arrange
            _controller = new StudentsController(_logger.Object, _services.Object);

            //Act
            var result = _controller.Get(null);

            //Assert 
            Assert.IsType<StatusCodeResult>(result);
        }
        [Fact]
        public void GetByIdTest_WhenDataAreReturned()
        {
            //Arrange
            _controller = new StudentsController(_logger.Object, _services.Object);
            var student1 = new Students { Name = "Name1", Age = 1, FinalNote = 1, Observation = "Observacion1" };
            var student2 = new Students { Name = "Name2", Age = 2, FinalNote = 2, Observation = "Observacion2" };
            List<Students> listSource = new List<Students>();
            listSource.Add(student1); listSource.Add(student2);
            var dbset = GetQueryableMockDbSet(listSource);
            _services.Setup(m => m.GetByName("Name1")).Returns(student1);

            //Act
            var result = _controller.Get("Name1");

            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<ObjectResult>(result);
            var model = Assert.IsAssignableFrom<Students>(objectResult.Value);
            Assert.Equal(1, model.Age);
        }
        #endregion

        #region PostUnitTest

        [Fact]
        public void PostTest_WhenNoData()
        {
            //Arrange
            _controller = new StudentsController(_logger.Object, _services.Object);

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
            _controller = new StudentsController(_logger.Object, _services.Object);
            var student1 = new Students { Name = "Name1", Age = 1, FinalNote = 1, Observation = "Observacion1" };
            var codeSnippedAdded = _services.Setup(m => m.Post(student1));
            //Act
            var result = _controller.Post(student1);

            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(Constants.CreateStudentsMessage, objectResult.Value);
            Assert.Equal(objectResult.StatusCode, (int)HttpStatusCode.Created);
        }

        #endregion

        #region PutUnitTest

        [Fact]
        public void PutTest_WhenNoData()
        {
            //Arrange
            _controller = new StudentsController(_logger.Object, _services.Object);
            var student1 = new Students { Name = "Name1", Age = 1, FinalNote = 1, Observation = "Observacion1" };
            _controller.Post(student1);

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
            _controller = new StudentsController(_logger.Object, _services.Object);
            var student1 = new Students { Name = "Name1", Age = 1, FinalNote = 1, Observation = "Observacion1" };
            _controller.Post(student1);
            student1.Name = "TestModify";
            var codeSnippedAdded = _services.Setup(m => m.Post(student1));

            //Act
            var result = _controller.Put(student1);


            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(Constants.UpdateStudentsMessage, objectResult.Value);
            Assert.Equal(objectResult.StatusCode, (int)HttpStatusCode.Accepted);
        }

        #endregion

        #region DeleteUnitTest

        [Fact]
        public void DeleteTest_WhenNoData()
        {
            //Arrange
            _controller = new StudentsController(_logger.Object, _services.Object);
            var student1 = new Students { Name = "Name1", Age = 1, FinalNote = 1, Observation = "Observacion1" };
            _controller.Post(student1);

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
            _controller = new StudentsController(_logger.Object, _services.Object);
            var myguid = Guid.NewGuid();
            var student1 = new Students { Id = myguid, Name = "Name1", Age = 1, FinalNote = 1, Observation = "Observacion1" };
            _controller.Post(student1);
            _services.Setup(m => m.Post(student1));
            var codeSnippedAdded = _services.Setup(m => m.Delete(student1));

            //Act
            var result = _controller.Delete(myguid);


            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(Constants.DeleteStudentsMessage, objectResult.Value);
            Assert.Equal(objectResult.StatusCode, (int)HttpStatusCode.OK);
        }

        #endregion
    }
}
