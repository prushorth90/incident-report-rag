var builder = WebApplication.CreateBuilder(args);

 
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.AddOllamaApiClient("chat").AddChatClient();
builder.AddServiceDefaults(); 
builder.Services.ConfigureHttpClientDefaults(http =>
{
    http.AddStandardResilienceHandler(options =>
    {
        // Give Ollama up to 5 minutes to generate a response
        options.AttemptTimeout.Timeout = TimeSpan.FromMinutes(5);
        options.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(5);
        
        // Satisfy the validation rule: SamplingDuration must be >= 2x AttemptTimeout
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromMinutes(10);
    });
});
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
 