using BooksWebCore.FrameworkApi;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BooksManagementXUnitTest
{
    public class InvalidRefererIdentifierMiddlewareTests
    {
        [Fact]
        public async void emptyRefererIsInvalid()
        {
            var rule = new RefererRule()
            {
                EmptyRefererIsInvalid = true
            };


            //provide a delegate which immediately finishes
            //without doing anything
            RequestDelegate next = context=>Task.CompletedTask;
            
            
            var middleware = new InvalidRefererIdentifier(next, rule);
            var httpContext = new DefaultHttpContext();
            await middleware.Invoke(httpContext);

            var invalidRefererHeader = httpContext.Request.Headers["invalid_referer"].FirstOrDefault();
            Assert.Equal(true.ToString(), invalidRefererHeader);
        }


        [Fact]
        public async void nonEmptyRefererIsValid()
        {
            var rule = new RefererRule()
            {
                EmptyRefererIsInvalid = true,
                ValidHost = "booksweb.org"
            };

            var httpContext = new DefaultHttpContext();

            //by default context Stream (where we write using Response or read using request)
            // is set to Stream.Empty
            // this stream fails silently for all read write.
            // do nothing dont complain.

            //if we need to capture the response.writeline
            //we must supply for our stream to HttpContext

            Stream mem = new MemoryStream();
            //TODO: Fix the Memory Stram to httpContext
            httpContext.Response.Body = mem; //when some one writes to body, write to memeory stream


            //provide a delegate which immediately finishes
            //without doing anything
            RequestDelegate next =async context =>
            {
                var status = context.Request.Headers["invalid_referer"].FirstOrDefault();
                await context.Response.WriteAsync($"invalid_refer : {status}");
            };


            var middleware = new InvalidRefererIdentifier(next, rule);
            
            
            httpContext.Request.Headers["Referer"] = "http://booksweb.org/authors";
            
            await middleware.Invoke(httpContext);

            //var invalidRefererHeader = httpContext.Request.Headers["invalid_referer"].FirstOrDefault();
            //Assert.Equal(false.ToString(), invalidRefererHeader);


            //Step1: we need to read the memory  stream from the begining So let us rewind it beging
            mem.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(mem);

            var text = reader.ReadToEnd();
           

            Assert.Equal("invalid_refer : False", text);

        }
    }
}
