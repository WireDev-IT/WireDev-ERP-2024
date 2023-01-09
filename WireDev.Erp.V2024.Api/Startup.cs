using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            _ = services.AddControllers();
            _ = services.AddSwaggerGen();

            // For Entity Framework
            //_ = services.AddDbContext<ApplicationUserDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("UserConnStr")));
            //_ = services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ProductConnStr")));

            _ = services.AddDbContext<ApplicationUserDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase")));
            _ = services.AddDbContext<ProductDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase")));
            _ = services.AddDbContext<CategoryDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase")));
            _ = services.AddDbContext<PurchaseDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase")));
            _ = services.AddDbContext<PriceDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase")));

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
                    _ = policy.RequireRole("Administrator");
                    _ = policy.RequireClaim("scopes", "ADMIN:RW");
                });
            });

            _ = services.AddAuthorization(options =>
            {
                options.AddPolicy("ADMIN:RW", policy => policy.RequireClaim("scope", "ADMIN:RW"));
                options.AddPolicy("PRODUCTS:RO", policy => policy.RequireClaim("scope", "PRODUCTS:RO"));
                options.AddPolicy("PRODUCTS:RW", policy => policy.RequireClaim("scope", "PRODUCTS:RW"));
                options.AddPolicy("PRICES:RO", policy => policy.RequireClaim("scope", "PRICES:RO"));
                options.AddPolicy("PRICES:RW", policy => policy.RequireClaim("scope", "PRICES:RW"));
                options.AddPolicy("CATEGORIES:RO", policy => policy.RequireClaim("scope", "CATEGORIES:RO"));
                options.AddPolicy("CATEGORIES:RW", policy => policy.RequireClaim("scope", "CATEGORIES:RW"));
                options.AddPolicy("PURCHASE:RO", policy => policy.RequireClaim("scope", "PURCHASE:RO"));
                options.AddPolicy("PURCHASE_BUY:RW", policy => policy.RequireClaim("scope", "PURCHASE_BUY:RW"));
                options.AddPolicy("PURCHASE_SELL:RW", policy => policy.RequireClaim("scope", "PURCHASE_SELL:RW"));
                options.AddPolicy("PURCHASE_CANCEL:RW", policy => policy.RequireClaim("scope", "PURCHASE_CANCEL:RW"));
                options.AddPolicy("PURCHASE_WITHDRAW:RW", policy => policy.RequireClaim("scope", "PURCHASE_WITHDRAW:RW"));
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
