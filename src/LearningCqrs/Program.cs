using LearningCqrs.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreDatabase();
builder.Services.AddCore();
builder.Services.AddCoreAuthentication();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCoreSwaggerGeneration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Requirement to add the logic
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();