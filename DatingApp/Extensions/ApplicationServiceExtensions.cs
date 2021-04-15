using DatingApp.Data;
using DatingApp.Helper;
using DatingApp.Interfaces;
using DatingApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
        {

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings")); //ii spunem configurari de unde sa ia configurarea efectiva
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);//trb sa stie unde sa gaseasca in proiect
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddDbContext<DataContext>(options =>//options este parametrul pe care il dam in brackets de mai jos
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
