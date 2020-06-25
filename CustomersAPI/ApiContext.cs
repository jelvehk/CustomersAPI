using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomersAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CustomersAPI
{
    public class ApiContext :DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
           : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
    }

}
