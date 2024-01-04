using InvvardDev.Ifttt.Service.Api.Trigger;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var webApp = builder.Build();

Configure(webApp);

webApp.Run();

void ConfigureServices(IServiceCollection services)
{
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddControllers();

    services.AddTriggers();
}

void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.MapControllers();
    
    app.ConfigureTriggers();
}
