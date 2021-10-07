using CinemaSystem.Models.Entities;
using CinemaSystem.Services.ActorServices;
using CinemaSystem.Services.GenreServices;
using CinemaSystem.Services.MovieServices;
using CinemaSystem.Services.StorageServices;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.WebApi
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
            services.AddScoped<IGenreServices, GenreServices>();
            services.AddScoped<IActorServices, ActorServices>();
            services.AddScoped<IMovieServices, MovieServices>();
            services.AddTransient<IFileStorageServices, AzureFileStorageServices>();
            services.AddAutoMapper(typeof(Services.MappersServices.AutoMapperProfileServices));
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CinemaSystem.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CinemaSystem.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
