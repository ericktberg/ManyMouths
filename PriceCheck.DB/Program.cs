using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using MySql.Data.MySqlClient;

using PriceCheck.DB.FoodCenter;
using PriceCheck.DB.ORM;

namespace PriceCheck.DB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>();
            var configuration = configBuilder.Build();

            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ManyMouthsContext>(
                options =>
                {
                    string password = configuration.GetConnectionString("ManyMouthsDB");
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
            builder.Services.AddSingleton<ManyMouthsDb>();
            builder.Services.AddSingleton<HttpClient>(new HttpClient());
            builder.Services.AddSingleton<SecretsFile>();
            builder.Services.AddTransient<FoodCenterConnection>();

            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                Console.WriteLine("Handling request: " + context.Request.Path);
                await next.Invoke();
                Console.WriteLine("Finished handling request.");
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // This line enables serving static files
            app.UseRouting(); // Adds routing middleware to the pipeline
            app.UseAuthorization();
            app.MapControllers();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}