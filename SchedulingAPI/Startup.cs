using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SchedulingAPI.Data;
using SchedulingAPI.Data.Repositories.ClassRepository;
using SchedulingAPI.Data.Repositories.RegistrationRepository;
using SchedulingAPI.Data.Repositories.StudentRepository;
using SchedulingAPI.Data.UnitOfWork;
using SchedulingAPI.Middleware;
using SchedulingAPI.Services.ClassService;
using SchedulingAPI.Services.RegistrationService;
using SchedulingAPI.Services.StudentService;

namespace SchedulingAPI
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
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<SchedulingDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));

            services.AddTransient<IClassRepository, ClassRepository>();
            services.AddTransient<IRegistrationRepository, RegistrationRepository>();
            services.AddTransient<IStudentRepository, StudentRepository>();

            services.AddTransient<IClassService, ClassService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddTransient<IStudentService, StudentService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Scheduling API",
                    Description = "A Super Simple Scheduling System"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scheduling API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
