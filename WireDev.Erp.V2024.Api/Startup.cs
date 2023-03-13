using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WireDev.Erp.V1.Api.Context;

namespace WireDev.Erp.V1.Api
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
            _ = services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
            });

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "WireDev ERP 2024",
                        Version = "v1"
                    }
                 );
                option.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "WireDev.Erp.V1.Api.xml"));
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                            {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // For Entity Framework
            //_ = services.AddDbContext<ApplicationUserDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("UserConnStr")));
            //_ = services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ProductConnStr")));

            _ = services.AddDbContext<ApplicationUserDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("UserDatabase")));
            _ = services.AddDbContext<ApplicationDataDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DataDatabase")));


            // For Identity
            _ = services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationUserDbContext>()
                .AddDefaultTokenProviders();

            // Adding Authentication
            _ = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            _ = services.AddAuthorization(opts =>
            {
                opts.AddPolicy("admin", policy =>
                {
                    _ = policy.RequireRole("Admin");
                });
                opts.AddPolicy("manager", policy =>
                {
                    _ = policy.RequireRole("Manager");
                });
                opts.AddPolicy("seller", policy =>
                {
                    _ = policy.RequireRole("Seller");
                });
                opts.AddPolicy("analyst", policy =>
                {
                    _ = policy.RequireRole("Analyst");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI();
            }

            _ = app.UseRouting();
            _ = app.UseHttpsRedirection();

            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            _ = app.UseEndpoints(endpoints =>
            {
                _ = endpoints.MapControllers();
            });
        }
    }
}
