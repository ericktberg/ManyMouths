using Microsoft.EntityFrameworkCore;

using MySql.Data.MySqlClient;

using PriceCheck.DB.FoodCenter;
using PriceCheck.DB.ORM;

namespace PriceCheck.DB
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddDbContext<ManyMouthsContext>(
                options =>
                {
                    string password = services.BuildServiceProvider()
                        .GetRequiredService<SecretsFile>()
                        .GetSecret("db.Password.ManyMouths");

                    var sb = new MySqlConnectionStringBuilder
                    {
                        Database = "many_mouths",
                        Server = "localhost",
                        Port = 3306,
                        UserID = "root",
                        Password = password
                    };

                    string connectionString = sb.ToString();
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                });

            services.AddSingleton<HttpClient>();
            services.AddSingleton<SecretsFile>();
            services.AddTransient<FoodCenterConnection>();

            return services;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllers(); // Only controllers, no Razor Views
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigins",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:8081")  // Expo localhost
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
            });

            builder.Services.RegisterServices();
            var app = builder.Build();

            // Development/Prod config
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            // Must come **before routing if you want to enforce auth early**
            app.UseHttpsRedirection();
            app.UseCors("AllowMyOrigins");

            // Routing comes first
            app.UseRouting();

            // **Authentication MUST come before Authorization**
            //app.UseAuthentication();
            //app.UseAuthorization();

            // Map controllers AFTER authentication/authorization
            app.MapControllers();

            app.Run();

        }
    }
}