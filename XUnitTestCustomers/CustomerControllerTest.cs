using CustomersAPI;
using CustomersAPI.Controllers;
using CustomersAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace XUnitTestCustomers
{
    
    public class CustomerControllerTest
    {
        CustomerController _controller;
         ApiContext _context ;

        DbContextOptions<ApiContext> options;
        
        public CustomerControllerTest ()
        {
            var builder = new DbContextOptionsBuilder<ApiContext>();
            builder.UseInMemoryDatabase("TestDatabase");
            options = builder.Options;
            _context = new ApiContext(options );
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            AddTestData(_context);
            _controller = new CustomerController(_context);
    
        }
       
        [Fact]
        //testing 200 HTTP Code Response
        public void Get_ReturnsOkResult()
        {
            var okResult = _controller.GetCustomers();
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        //testing if returned object contain list of customers
        public  void Get_ReturnsAllItems()
        {
          
                // Act

                var customers = _controller.GetCustomers().Result  as OkObjectResult ;

                var _result = Assert.IsType<List<Customer>>(customers.Value );
                Assert.Equal(3, _result.Count);
           
           
        }
        [Fact]
        //testing getbyId when id could not be found in db 
        public void GetById_UnknownIdPassed_ReturnsNotFoundResult()
        {
           
            // Arrange
            int id = 2123;
            // Act
            var notFoundResult = _controller.GetCustomer(id);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result );
        }

        [Fact]
        //testing getbyId when id is found in db 
        public void GetById_ExistingIdPassed_ReturnsRightCustomer()
        {
            // Arrange
            var id = 1001;

            // Act
            var okResult = _controller.GetCustomer(id);

            // Assert
          var customer=  Assert.IsType<OkObjectResult>(okResult.Result );
            Assert.Equal(1001, (customer.Value as Customer).Id);
        }
        [Fact]
        public void  Can_AddCustomer()
        {
            //Arrange
            var testCustomer= new Customer
            {
                Id = 1006,
                FirstName = "Richard",
                LastName = "Amani",
                DOB = DateTime.Parse("1953.03.03")
            };
            //Act
            var createdResponse = _controller.AddCustomer(testCustomer);
            //Assert
            Assert.IsType<OkResult>(createdResponse);

        }

        [Fact]
        public void Can_DeleteCustomer()
        {
            //Arrange
            int id = 1003;
           
            //Act
            var deletedResponse = _controller.DeleteCustomer (id);
            //Assert
            Assert.IsType<OkObjectResult>(deletedResponse);
            var customers = _controller.GetCustomers().Result as OkObjectResult;

            var _result = Assert.IsType<List<Customer>>(customers.Value);
            Assert.Equal(2, _result.Count );

        }
        [Fact]
        public void Can_UpdateCustomer()
        {
            //Arrange
            int id = 1002;
            var testCustomer = new Customer();
            testCustomer.FirstName = "Richard";
            testCustomer.LastName = "Lion";
            testCustomer.DOB = DateTime.Parse("1953.03.03");
           
            //Act
            var updatedResponse = _controller.UpdateCustomer(id, testCustomer);

            //Assert
            Assert.IsType<OkObjectResult>(updatedResponse);


        }
        [Fact]
        //testing searchcustomersByName
        public void Search_CustomersByName()
        {
            //Arrange 
            string name = "ma";

            // Act

            var customers = _controller.SearchCustomersByName(name).Result as OkObjectResult;

            var _result = Assert.IsType<List<Customer>>(customers.Value);

            //Assert
            Assert.Equal(2, _result.Count);


        }
        private void AddTestData(ApiContext context)
        {


            var testCustomer1 = new Customer
            {
                Id = 1001,
                FirstName = "Jane",
                LastName = "Ford",
                DOB = DateTime.Parse("2001.01.03")
            };

            var testCustomer2 = new Customer
            {
                Id = 1002,
                FirstName = "Ali",
                LastName = "Karimi martini",
                DOB = DateTime.Parse("2002.04.03")
            };

            var testCustomer3 = new Customer
            {
                Id = 1003,
                FirstName = "Martin",
                LastName = "Montir",
                DOB = DateTime.Parse("1980.01.03")
            };



            context.Customers.Add(testCustomer1);
            context.Customers.Add(testCustomer2);
            context.Customers.Add(testCustomer3);
            context.SaveChanges();
           

        }
    }
}
