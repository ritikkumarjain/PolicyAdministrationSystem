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
using QuotesMicroservice.Service;
using QuotesMicroservice.Repository;
using Microsoft.EntityFrameworkCore;
using QuotesMicroservice.Models;

namespace QuotesMicroservice
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
            services.AddControllersWithViews();
            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<DatabaseContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:QuoteDB"]));
            services.AddTransient<IQuotesRepo, QuotesRepo>();
            services.AddTransient<IQuotesService, QuotesService>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "QuotesMaster Web Api Service",
                    Version = "v2",
                });
            });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins("https://localhost:44357")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2/swagger.json", "QuotesMicroservice"));

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseRouting();

            loggerFactory.AddLog4Net();

            app.UseAuthorization();

            app.UseCors("AllowOrigin");

            app.UseCors(builder => builder
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseCors(options => options.WithOrigins("https://localhost:44357/").AllowAnyMethod());


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
