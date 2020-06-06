using BooksWebCore.Controllers;
using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BooksManagementXUnitTest
{
    public class AuthorControllerTest
    {

        [Fact]
        public void Create_GetCallReturnsAViewResultForEmptyAuthorObject()
        {
            //passing a fake value
            //it will do nothing. just to meet semtanic requirement
            //useful if the objects are not used inside
            var controller = new AuthorController(null, null, null);

            var result = controller.Create();

            Assert.IsType<ViewResult>(result);

            var vr = result as ViewResult;

            //done by Controller by looking it HttpContext and other elements
            //Assert.Equal("Create", vr.ViewName);
            
            Assert.IsType<Author>(vr.Model);

            var author = vr.Model as Author;
            Assert.Null(author.Id);
            Assert.Null(author.Name);
            
        }


        //[Fact(Skip ="We Need to Fix HttpContext here")]
        [Fact]
        public void Create_PostCallWithInvalidDataReturnsViewResultWithBadStatus()
        {
            var controller = new AuthorController(null, null, null);

            //Problem#1 This controller action uses Response object from HttpContext
            //Unless we supply that it won't
            //Now System provides me a predefined  Mock Object for HttpContext
            var httpContext = new DefaultHttpContext();

            //controoler.HttpContext is readonly
            //controller.HttpContext = httpContext;

            //It is part of ControllerContext
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = httpContext;


            //Calling Create directly will not run validation and will not
            //Create ModelState Object. This is done by the Mvc Validation Framework
            //And we are not running validation framework
            //I am not testing validation framework
            //My goal is to see, if there is Validation Error,
            //what will be my controllers reaction

            //So we add our own validation error
            controller.ModelState.AddModelError("Name", "Required");

            var result = controller.Create(new Author() { });

            Assert.IsType<ViewResult>(result);

            Assert.Equal(400, controller.Response.StatusCode);

        }
    }
}
