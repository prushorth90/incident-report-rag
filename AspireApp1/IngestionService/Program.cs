using IngestionService;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();
builder.AddOllamaApiClient("embeddings").AddEmbeddingGenerator();
var sqliteConnectionString = builder.Configuration.GetConnectionString("vector-store");
builder.Services.AddSqliteVectorStore(_ => sqliteConnectionString ?? throw new InvalidOperationException("Missing connection string for vector store."));
var host = builder.Build();
host.Run();
