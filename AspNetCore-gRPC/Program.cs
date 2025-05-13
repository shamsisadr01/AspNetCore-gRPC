

using AspNetCore_gRPC.Context;
using AspNetCore_gRPC.Protos;
using AspNetCore_gRPC.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddDbContext<GRPCContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("GRPCConnectionString"));
});

var app = builder.Build();

app.MapGrpcService<ProductServiceGRPC>();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
