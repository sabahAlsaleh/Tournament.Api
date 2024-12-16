using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using System.Text;
using Tournament.Api.Extensions;
using Tournament.Core.Entities;
using Tournament.Core.Repository;
using Tournament.Data.Data;
using Tournament.Data.Repositories;
using Tournament.Services;

namespace Tournament.Api
{
    public  class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TournamentContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("TournamentContext")
            ?? throw new InvalidOperationException("Connection string 'TournamentContext' not found.")));


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TournamentContext>()
                .AddDefaultTokenProviders();

            // services to the container.
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddScoped<IGameService, GameService>();



            // These extensions help us map against Json and Xml.
            builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable= true)
                .AddNewtonsoftJson()
                .AddXmlDataContractSerializerFormatters();
            
          /* 
           builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true)
           .AddNewtonsoftJson(options =>
           {
               options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
           })
           .AddXmlDataContractSerializerFormatters();

            */
            builder.Services.AddAutoMapper(typeof(TournamentMappings));

            //  authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    var secretkey = builder.Configuration["secretkey"];

                    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
                    ArgumentNullException.ThrowIfNull(nameof(jwtSettings));
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey))

                    };
                }
            );



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    // Seed Application Data
                    var context = services.GetRequiredService<TournamentContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                        
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                   
                    await SeedData.SeedAsync(context);
                    await SeedData.SeedUsersAsync(userManager, roleManager);
                }
            }

            app.UseHttpsRedirection();
            app.UseAuthentication(); // authentication middleware
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
