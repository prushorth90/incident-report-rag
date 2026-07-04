var builder = WebApplication.CreateBuilder(args);

 
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.AddOllamaApiClient("chat").AddChatClient();
builder.AddServiceDefaults(); 
builder.Services.AddOllamaResilienceHandlers();
var app = builder.Build();
app.MapDefaultEndpoints(); // connec swagger to aspire dashboard
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
     });

}

app.UseHttpsRedirection();

 
app.MapControllers(); // <-- Make sure this is registered FIRST

app.Run();
 