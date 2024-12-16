using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Service.Contracts;
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

            //  authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Identity.Application";
                options.DefaultChallengeScheme = "Identity.Application";
            });


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
