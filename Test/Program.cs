using Test.Services;
using TestTask;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<IProviderService, ProviderOneService>();
builder.Services.AddHttpClient<IProviderService, ProviderTwoService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IRouteFilterService, RouteFilterService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IRouteMapperService, RouteMapperService>();
builder.Services.AddScoped<IRouteMapperService, RouteMapperService>();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
