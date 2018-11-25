using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RESTfulODataService.Extensions;
using RESTfulODataService.Sample.Data;
using RESTfulODataService.Sample.Data.Repositories;
using RESTfulODataService.Sample.Services.JsonConverters;
using System.Collections.Generic;

namespace RESTfulODataService.Sample
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
            services.AddScoped<LibraryDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IReaderRepository, ReaderRepository>();
            services.AddDbContext<LibraryDbContext>(b => b.UseInMemoryDatabase("Library_DB"));
            services.AddRESTfulODataService();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters = new List<JsonConverter>
                {
                    new StringEnumConverter { CamelCaseText = true },
                    new BookJsonConverter(),
                    new ReaderJsonConverter(),
                    new AuthorJsonConverter()
                };
                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
