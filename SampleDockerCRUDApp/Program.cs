using Microsoft.EntityFrameworkCore;
using SampleDockerCRUDApp;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/books", async (DataContext context) =>
    await context.Books.ToListAsync());

app.MapGet("/book/{id}", async (DataContext context, int id) =>
    await context.Books.FindAsync(id) is Book book ?
        Results.Ok(book) :
        Results.NotFound("Sorry, book not found. :("));

app.MapPost("/addBook", async (DataContext context, string title, string author) =>
{
    var book = new Book { Title = title, Author = author };
    context.Books.Add(book);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Books.ToListAsync());
});

app.MapPut("/book/{id}", async (DataContext context, int id, string title, string author) =>
{
    var book = await context.Books.FindAsync(id);
    if (book is null)
        return Results.NotFound("Sorry, this book doesn't exist.");

    book.Title = title;
    book.Author = author;
    await context.SaveChangesAsync();

    return Results.Ok(await context.Books.ToListAsync());
});

app.MapDelete("/book/{id}", async (DataContext context, int id) =>
{
    var book = await context.Books.FindAsync(id);
    if (book is null)
        return Results.NotFound("Sorry, this book doesn't exist.");

    context.Books.Remove(book);
    await context.SaveChangesAsync();

    return Results.Ok(await context.Books.ToListAsync());
});

app.Run();

public class Book
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
}