using System;
using System.IO;
using System.Reflection;
using System.Text;
using BookApi.Model;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shirley.Book.DataAccess;
using Shirley.Book.Service.AuthServices;
using Shirley.Book.Service.Domains;
using Shirley.Book.Service.UOW;
using Shirley.Book.Web.Infrastructure;
using StackExchange.Redis;

namespace BookApi
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
            services.AddTransient<HttpGlobalExceptionHandler>();
            services.AddControllers(option =>
            {
                option.Filters.Add<HttpGlobalExceptionHandler>();
            }).AddNewtonsoftJson();

            services.AddDbContext<BookContext>(option =>
            {
                option.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30),
                        ValidIssuer = Const.Domain,
                        ValidAudience = Const.Domain,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey))
                    };
                });

            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                var securityRequirement = new OpenApiSecurityRequirement();
                var securityScheme = new OpenApiSecurityScheme
                {
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                securityRequirement.Add(securityScheme, new[] {
                    JwtBearerDefaults.AuthenticationScheme});
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
                c.AddSecurityRequirement(securityRequirement);
                c.SwaggerDoc("v1",
                    new OpenApiInfo()
                    {
                        Version = "v1",
                        Title = "BookApi Title",
                    });
            });

            services.AddMediatR(typeof(IAuthService).Assembly);
            services.AddDomainServicesByConversion(typeof(IAuthService).Assembly);
            services.AddHostedService<StockIncrementService>();
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
            services.AddScoped<IDistributedLockProvder, RedisDistributedLockProvder>();
            services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();
            services.AddScoped<UnitOfWorkMiddleware>();
            services.AddScoped<UnitOfWorkFilterAttribute>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Api V1");
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

           // app.UseMiddleware<UnitOfWorkMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
