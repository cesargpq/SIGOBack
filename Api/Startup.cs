using Api.Core.Interfaces;
using Api.Infrastructure;
using Api.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddDbContext<SoftwareContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("PruebasSoftware")));

            services.AddTransient<IRolRepository, RolRepository>();
            

            //services.AddTransient<IGenericRepository<TypePhone>, GenericRepository<TypePhone>>();   
            //services.AddScoped<IGenericRepository<Profiles>, GenericRepository<Profiles>>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Servicios Rest - Validación de Contratos", Version = "v1" });
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
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:4771"; // Change this to your Ocelot Gateway URL
                options.RequireHttpsMetadata = false; // Only for development
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("this is my custom Secret key for authenticatio")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });



            services.AddHttpClient();

            services.AddCors(options =>
            {
                var urlList = Configuration.GetSection("AloowedOrigin").GetChildren().Select(c => c.Value).ToArray();

                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(urlList).AllowAnyMethod().AllowAnyHeader();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseCors();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(options =>
            {
                options.MapControllers();
            });
        }
    }
}
