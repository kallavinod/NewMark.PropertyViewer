using NewMark.WebApi.Model;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders(); // Clear default providers if needed
builder.Logging.AddConsole(); // Add console logging
builder.Logging.AddDebug();   // Optional: Add debug logging (useful for development)
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAngularApp",
      builder =>
      {
        builder.WithOrigins("http://127.0.0.1:4200") // Angular app origin
                 .AllowAnyHeader()
                 .AllowAnyMethod();
      });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AzBlob>(builder.Configuration.GetSection("AzureBlob"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
