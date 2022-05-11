using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minimart.Core.Domain.Services;
using Minimart.Core.Services;
using Minimart.Core.Persistence.Context;
using Minimart.Core.Domain.Repositories;
using Minimart.Core.Persistence.Repositories;
using Minimart.WebApi.Mapping;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;

namespace Minimart.WebApi
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


            //Minimart Custom 
            var connectionString = Configuration.GetConnectionString("SqlConnection");
            services.AddSingleton(s => new DapperContext(connectionString));
            
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IStoreRepository, StoreRepository>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartRepository, CartRepository>();

            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Minimart API",
                    Description = "A simple example of a rest web api",
                    
                    Contact = new OpenApiContact
                    {
                        Name = "Juan Pablo",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/@juan_analian"),
                    } 
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });
            services.AddAutoMapper(typeof(ModelToResourceProfile));
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
