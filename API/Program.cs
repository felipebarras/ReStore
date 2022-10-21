using System;
using API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope(); // with using, it's going to automatically dispose of any resources that we're using as part of this
            var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>(); //this will show if an error occur in the logger part
            // to 
            try
            {
                context.Database.Migrate(); // Applies any pending migrations for the context to the database. Will create the database if it does not already exist.
                DbInitializer.Initialize(context); // it'll have access to the database context which needs to use to run that method to at the products into our application
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Problem migrating data");
            }
            
            
            host.Run(); // it's going to go ahead and create a database, create  the tables in a database and then it's gonna add all of those products in the CS class (DbInitializer class)
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
