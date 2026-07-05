using System.Reflection.Metadata.Ecma335;

var builder = DistributedApplication.CreateBuilder(args);
var ollama = builder.AddOllama("ollama").WithDataVolume();
var chatModel = ollama.AddModel("chat", "llama3.2");

//builder.AddContainer("open-webui", "ghcr.io/open-webui/open-webui", "main")
 //   .WithHttpEndpoint(port: 3000, targetPort: 8080, name: "http")
   // .WithEnvironment("OLLAMA_BASE_URL", ollama.GetEndpoint("http"))
 //   .WithLifetime(ContainerLifetime.Persistent)
//    .WaitFor(ollama);

builder.AddProject<Projects.IcmChatAPI>("icmchatapi")
    .WithReference(chatModel)
    .WaitFor(chatModel);
 
 builder.AddProject<Projects.IngestionService>("ingestionservice");

builder.Build().Run();
