using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsPortal.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NewsDb>(opt => opt.UseInMemoryDatabase("News"));
builder.Services.AddCors(opt => opt.AddDefaultPolicy(o =>
{
    o.AllowAnyOrigin();
    o.AllowAnyHeader();
    o.AllowAnyMethod();
}));

var app = builder.Build();

app.UseCors();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var GetAllNews = () => new List<NewsDto>
{
    new NewsDto {NewsId = 1,NewsTitle = "news 1", NewsContent = "news content 1" },
    new NewsDto {NewsId = 2,NewsTitle = "news 2", NewsContent = "news content 2" },
    new NewsDto {NewsId = 3,NewsTitle = "news 3", NewsContent = "news content 3" },
    new NewsDto {NewsId = 4,NewsTitle = "news 4", NewsContent = "news content 4" },
    new NewsDto {NewsId = 5,NewsTitle = "news 5", NewsContent = "news content 5" }
};

var news = () => "this is delegate";

app.MapGet("/news", async (NewsDb db) =>
{
    return Results.Ok(await db.News.Where(news => news.IsActive == true).ToListAsync());
});

app.MapGet("/news1", (HttpRequest request) =>
{
    return Results.Ok(new NewsDto { NewsId = 1, NewsTitle = "test title 1", NewsContent = "test content 1" });
});

app.MapGet("/news2", () =>
{
    return Results.Ok(GetAllNews.Invoke());
});


app.MapGet("/news/{id}", async (int id, NewsDb db) =>
{
    var entity = await db.News
    .Where(news => news.NewsId == id && news.IsActive == true)
    .FirstOrDefaultAsync();

    if (entity is null) return Results.NotFound();

    return Results.Ok(entity);
});


app.MapPost("/news", async (NewsDto inputDto, NewsDb db) =>
{
    var newsEntity = new News
    {
        IsActive = true,
        NewsContent = inputDto.NewsContent,
        NewsTitle = inputDto.NewsTitle
    };

    db.News.Add(newsEntity);
    return await db.SaveChangesAsync();
});


app.MapPut("/news/{id}", async (int id, NewsDto inputDto, NewsDb db) =>
{
    var newsEntity = db.News
    .Where(news => news.NewsId == id)
    .FirstOrDefault();

    if (newsEntity is null) return Results.NotFound();

    newsEntity.NewsTitle = inputDto.NewsTitle;
    newsEntity.NewsContent = inputDto.NewsContent;

    return Results.Ok(await db.SaveChangesAsync());

});

app.MapDelete("/news", async (int id, NewsDb db) =>
{
    var entity = await db.News
    .Where(news => news.NewsId == id)
    .FirstOrDefaultAsync();

    if (entity is null) return Results.NotFound();

    entity.IsActive = false;
    return Results.Ok(await db.SaveChangesAsync());

});


app.UseAuthorization();

await app.RunAsync();