using InvvardDev.Ifttt.Service.Api.Trigger;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var webApp = builder.Build();

Configure(webApp, builder.Environment);

webApp.Run();

void ConfigureServices(IServiceCollection services)
{
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    
    services.AddTriggers();
}

void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
{
    if (environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
}
