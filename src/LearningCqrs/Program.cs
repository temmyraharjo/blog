using LearningCqrs.Core;

var builder = WebApplication.CreateBuilder(args);

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetService<IConfiguration>();

builder.Services.AddCoreDatabase(configuration);
builder.Services.AddCore();
builder.Services.AddCoreAuthentication(configuration);
builder.Services.AddCoreAutoMapper();

builder.Services.AddControllers()
    .AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCoreSwaggerGeneration();
builder.Services.AddCoreLogging(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
var enableSwagger = bool.Parse(configuration.GetConnectionString("EnableSwagger"));
if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(opt => opt.DefaultModelsExpandDepth(-1));
}

app.UseHttpsRedirection();

// Requirement to add the logic
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();