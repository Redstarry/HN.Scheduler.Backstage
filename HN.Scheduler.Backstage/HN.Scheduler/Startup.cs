using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HN.Scheduler.Repository;
using Quartz;
using Quartz.Impl;
using AutoMapper;
using System.IO;
using HN.Scheduler.Application.MapperData;

namespace HN.Scheduler
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
            services.AddAutoMapper(typeof(MapperProfile).Assembly);
            services.AddCors(option => option.AddPolicy("Domain", builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
            //API 文档的注册
            services.AddSwaggerGen(option =>
            {

                option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "My Api",
                    Version = "v1"
                });
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "Swagger.xml");
                option.IncludeXmlComments(xmlPath);
            });
            //var container = new ContainerBuilder();
            //container.Populate(services);
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<FarmerSchedulerRepository>().As<IFarmerSchedulerRepository>();
            builder.RegisterType<StdSchedulerFactory>().As<ISchedulerFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("Domain");
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(option => {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
            
    }
}
