using ApiTask.DataInfrastructure.Context;
using ApiTask.Services.Interfaces;
using ApiTask.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ApiTask.DataInfrastructure.Context.Interfaces;

namespace ApiTask
{
    public static class Config
    {
        public static WebApplication CreateBuilder(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            
            string? urlBaseGateway = builder.Configuration.GetSection("UrlBaseGateway").Get<string>();

            builder.Logging.ClearProviders(); 
            builder.Logging.AddConsole(); 
            builder.Logging.AddDebug(); 

            builder.Services.AddDbContext<ITaskContext, TaskDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetSection("ConnectionStringsSqlLite").Get<string>()));

            builder.Services.AddScoped<ITaskService, TaskService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    if (!string.IsNullOrEmpty(urlBaseGateway))
                        policy.WithOrigins(urlBaseGateway);
                });
            });
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddServer(new OpenApiServer
                {
                    Url = urlBaseGateway,
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Por favor insira o token JWT que é gerado na api de gateway",
                    Name = "Token de autorização",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            var app = builder.Build();
            return app;
        }
        public static void ConfigApp(WebApplication app) {          

            app.UseCors("AllowSpecificOrigin");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
                context.Database.Migrate();
            }

            app.Run();
        }
    }
}
