using ApiGateway.Models;
using ApiGateway.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<IdentityServerConfig>(builder.Configuration.GetSection("IdentityServer"));

builder.Services.AddScoped<UserService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration.GetSection("IdentityServer:Url").Get<string>();
        options.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AuthenticatedUsersOnly", policy =>
        policy.RequireAuthenticatedUser());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddSwaggerGen(options =>
{
    options.DocInclusionPredicate((name, api) => api.HttpMethod != null);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("swagger/v1/swagger.json", "Gateway API");
        options.SwaggerEndpoint($"{builder.Configuration.GetSection("ReverseProxy:Clusters:task-service:Destinations:destination1:Address").Get<string>()}swagger/v1/swagger.json", "Task API");
        options.RoutePrefix = string.Empty;
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.MapControllers();

app.Run();
