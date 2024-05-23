using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ValiBot.Commands;
using ValiBot.Commands.BotServices;
using ValiBot.Entities;
using ValiBot.Repository;
using ValiBot.Repository.Interfaces;
using ValiBot.Services;
using ValiBot.Services.Interfaces;

namespace ValiBot
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<DataContext>(opt => 
                opt.UseNpgsql(_configuration.GetConnectionString("Db")),ServiceLifetime.Singleton);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ValiBot API", Version = "v1" });
            });
            
            services.AddScoped<TelegramBot>();
            services.AddScoped<ICommandExecutor, CommandExecutor>();
            services.AddScoped<IOperationService, OperationService>();
            services.AddScoped<IFillFormService, FillFormService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAnalyticService, AnalyticService>();
            services.AddScoped<BaseCommand, StartCommand>();
            services.AddScoped<BaseCommand, FillFormCommand>();
            services.AddScoped<IBaseRepository<AppUser>, BaseRepository<AppUser>>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ValiBot API"));
            serviceProvider.GetRequiredService<TelegramBot>().GetBot().Wait();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}