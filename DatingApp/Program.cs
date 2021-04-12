using DatingApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp
{
    public class Program
    {
        public static async Task Main(string[] args) //ce se intampla aici are loc inainte ca aplicatia sa porneasca
        {
            var host= CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope(); // ne aducem serviciul datacontext pentru asta ne  folosim de scope
            var services = scope.ServiceProvider;//avem neovie de un scope pentru serviciile noastre
            try //aici suntem inafara middleware-ului nostru asa ca trb sa folosim try&catch
            {
                var context = services.GetRequiredService<DataContext>();
                await context.Database.MigrateAsync(); //cand pornim aplicatia are loc migrarea db-ului.,daca dam drop la db doar dam rr la aplicatie si se recreeaza
                await Seed.SeedUsers(context);
            }
            catch(Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An eeror ocurred during migration");
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
