using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddControllers();
            builder.Services.AddRazorPages();
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // This line enables serving static files
            app.UseRouting(); // Adds routing middleware to the pipeline
            app.UseAuthorization();
            app.MapControllers();

            // Add an endpoint to serve the UserSelection.html file
            app.MapGet("/", async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync("wwwroot/UserSelection.html");
            });

            app.Run();
        }
    }
}