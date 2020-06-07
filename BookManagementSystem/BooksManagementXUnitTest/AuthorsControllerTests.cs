using BooksWebCore.Controllers;
using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BooksManagementXUnitTest
{
    public class AuthorsControllerTests
    {

        IList<Author> dummyAuthors;

        public AuthorsControllerTests()
        {
            dummyAuthors = new List<Author>()
            {
                new Author(){Id="vivek",Name="Vivek Dutta Mishra"},
                new Author(){Id="dinkar",Name="Ramdhari Singh Dinkar"}
            };
        }

        [Fact]
        public void GetAllAuthors_ReturnsOkResultWithAuthors()
        {
            //with real manager, you are not doing a real unit test
            //test may fail because of controller or manager or repository

            //var repository=//create some repository
            //var manager = new SimpleAuthorManager(rep);

            //Soution-1 We should be using a Mock object to handle this

            var mock = new Mock<IAuthorManager>();
            mock
                .Setup(m => m.GetAllAuthors())
                .Returns(dummyAuthors);

            var authorManager = mock.Object;

            var controller = new AuthorsController(authorManager);

            var result = controller.GetAllAuthors();
            Assert.IsType<OkObjectResult>(result);

            var data = (result as OkObjectResult).Value;

           

            var list = data as IList<Author>;

            Assert.NotNull(list);
            Assert.Equal(2, list.Count);
            Assert.Equal(dummyAuthors, list);
        }
    
    
        [Fact]
        public void AddAuthor_ReturnsBadResultForInvalidAuthor()
        {
            //Arrange
            String key = "Name";
            String errorMessage = "Required";

            var mock = new Mock<IAuthorManager>();

            mock.Setup(m => m.AddAuthor(It.IsAny<Author>()))
                .Returns("something").Verifiable();

            var manager = mock.Object;

            var controller = new AuthorsController(manager);
            controller.ModelState.AddModelError(key,errorMessage);


            //Act
            Author author = new Author();
            var result = controller.AddAuthor(author);

            //ASSERT

            Assert.IsType<BadRequestObjectResult>(result);
            var badResult = result as BadRequestObjectResult;

            Assert.IsType<SerializableError>(badResult.Value);

            var errors = badResult.Value as SerializableError;

            Assert.Equal(errorMessage, ((string[])errors[key])[0]);


            //verify that because of validation error
            //manager.Add was never called with any parameter
            mock.Verify(m => m.AddAuthor(author), Times.Never);
        }

        [Fact]
        public void AddAuthor_ReturnsCreatedResult_WithValidData()
        {
            //Arrange
            string expectedId = "someid";
            var mock = new Mock<IAuthorManager>();

            mock.Setup(m => m.AddAuthor(It.IsAny<Author>()))
                .Returns((Author author) =>
                {
                    author.Id = expectedId;
                    return expectedId;
                }).Verifiable();

            var manager = mock.Object;
            var controller = new AuthorsController(manager);


            var author = new Author() { Name = "Vivek", Id=null };
            Assert.Null(author.Id);
            //Act

            var result = controller.AddAuthor(author);

            //ASSERT

            Assert.IsType<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;

            //created result has action name set
            Assert.Equal("GetAuthorById", createdResult.ActionName);

            //created result has route value set
            Assert.Equal(expectedId, createdResult.RouteValues["Id"]);

            var model = createdResult.Value as Author;

            //ceated result return the same object as passed
            Assert.Same(author, model);

            //id field is set by the Create Action
            Assert.Equal(expectedId, model.Id);

            mock.Verify(); //make sure the mock function that are configured are actually called
        }

    }
}
