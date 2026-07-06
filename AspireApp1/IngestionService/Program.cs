using IngestionService;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();
builder.AddOllamaApiClient("embeddings").AddEmbeddingGenerator();
var host = builder.Build();
host.Run();
