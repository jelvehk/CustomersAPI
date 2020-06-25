using CustomersAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomersAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace CustomersAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ApiContext _context;
        public CustomerController(ApiContext context)
        {
            _context = context;
        }
        [HttpGet("/api/customers")]
        public ActionResult<List<Customer>> GetCustomers()
        {
            return Ok(_context.Customers.ToList());


        }

        [HttpGet("/api/customers/{id}")]
        public ActionResult<Customer> GetCustomer(int id)
        {
            var dbCustomer = _context.Customers.Find(id);

            if (dbCustomer == null)
            {
                return NotFound();
            }

            return Ok(dbCustomer);
        }
        [HttpGet("/api/customers/Search/{name}")]
        public ActionResult<List<Customer>> SearchCustomersByName(string name)
        {
            var dbCustomers = _context.Customers.Where<Customer>(x => x.FirstName.ToLower().Contains(name.Trim()) || x.LastName.ToLower().Contains(name.Trim())).ToList();

            if (dbCustomers == null)
            {
                return NotFound();
            }

            return Ok(dbCustomers);
        }

        [HttpPost("/api/customers")]
        public OkResult AddCustomer(Customer customer)
        {
            using (_context)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();

                return Ok();
            }

        }
        [HttpPut("/api/customers/{id}")]
        public IActionResult UpdateCustomer(int id, Customer customer)
        {
            customer.Id = id;

            var _dbCustomer = _context.Customers.Find(id);
            _context.Entry(_dbCustomer).State = EntityState.Detached;
            _dbCustomer.FirstName = customer.FirstName;
            _dbCustomer.LastName = customer.LastName;
            _dbCustomer.DOB = customer.DOB;
            _context.Entry(_dbCustomer).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Updated OK");
        }

        [HttpDelete("/api/customers/{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            _context.SaveChanges();

            return Ok("Deleted OK");
        }
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}






