using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomersAPI.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace CustomersAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("TestDatabase"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var context = serviceProvider.GetService<ApiContext>();
            AddTestData(context);
            app.UseHttpsRedirection();
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private static void AddTestData(ApiContext context)
        {
            var testCustomer1 = new Customer
            {
                Id = 1001,
                FirstName = "Jane",
                LastName = "Ford",
                DOB= DateTime.Parse("2001.01.03")
            };

            var testCustomer2 = new Customer
            {
                Id = 1002,
                FirstName = "Ali",
                LastName = "Karimi",
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
