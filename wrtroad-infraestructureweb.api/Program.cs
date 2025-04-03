using wrtroad_infraestructureweb.api.core.infrastructure.extensions.authentication;
using wrtroad_infraestructureweb.api.core.infrastructure.extensions.authorization;
using wrtroad_infraestructureweb.api.core.infrastructure.extensions.automappers;
using wrtroad_infraestructureweb.api.core.infrastructure.extensions.injections;
using wrtroad_infraestructureweb.api.core.infrastructure.extensions.server;
using wrtroad_infraestructureweb.api.webapi.configurations;
using wrtroad_infraestructureweb.api.webapi.middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabaseContext(builder.Configuration);
builder.Services.AddDependencyInjection();
builder.Services.AddAutoMapper();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.AddCustomAuthorization();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())      
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "wrtroad.infraestructure.api V1.0");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "My API Documentation";
        //c.DefaultModelsExpandDepth(-1); 
    });
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.CustomMiddlewares();
app.MapControllers();

app.Run();
