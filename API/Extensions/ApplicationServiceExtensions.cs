using API.Business_Layer.Infrastructure;
using API.Business_Layer.Interfaces;
using API.Business_Layer.Services;
using API.Data_Layer;
using API.Data_Layer.Repositories;
using API.Helpers;
using API.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices
            (this IServiceCollection services,
            IConfiguration config)
        {
            // Add services to the container.

            services.AddControllers();
            services.AddDbContext<DataDbContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            return services;
        }
    }
}
