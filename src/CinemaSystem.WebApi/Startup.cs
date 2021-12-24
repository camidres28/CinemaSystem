using CinemaSystem.Models.Entities;
using CinemaSystem.Services.AccountServices;
using CinemaSystem.Services.ActorServices;
using CinemaSystem.Services.CinemaServices;
using CinemaSystem.Services.GenreServices;
using CinemaSystem.Services.MappersServices;
using CinemaSystem.Services.MovieReviewsServices;
using CinemaSystem.Services.MovieServices;
using CinemaSystem.Services.StorageServices;
using CinemaSystem.WebApi.CustomFilters;
using CinemaSystem.WebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.Text;

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
            services.AddScoped<ICinemaServices, CinemaServices>();
            services.AddScoped<IAccountServices, AccountServices>();
            services.AddScoped<IMovieReviewsServices, MovieReviewsServices>();
            services.AddScoped<ExistsMovieAttribute>();

            services.AddTransient<IFileStorageServices, AzureFileStorageServices>();
            services.AddAutoMapper(typeof(AutoMapperProfileServices));
            services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));
            services.AddSingleton(provider =>

                    new AutoMapper.MapperConfiguration(
                        config =>
                        {
                            GeometryFactory geometry = provider.GetRequiredService<GeometryFactory>();
                            config.AddProfile(new AutoMapperProfileServices(geometry));
                        }).CreateMapper()
                );
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptions => sqlServerOptions.UseNetTopologySuite())
                );
            services.AddControllers(options=>
            {
                options.Filters.Add(typeof(ErrorsFilter));
            }).AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CinemaSystem.WebApi", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                options =>
                options.TokenValidationParameters =
                new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["jwt:key"])),
                    ClockSkew = System.TimeSpan.Zero
                }
                );
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CinemaSystem.WebApi v1"));

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
