using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Create database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoContext>();
    db.Database.EnsureCreated();
}

// GET /api/todos - Get all todos
app.MapGet("/api/todos", async (TodoContext db) =>
{
    return await db.TodoItems.ToListAsync();
})
.WithName("GetAllTodos");

// GET /api/todos/{id} - Get a specific todo
app.MapGet("/api/todos/{id}", async (int id, TodoContext db) =>
{
    var todo = await db.TodoItems.FindAsync(id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
})
.WithName("GetTodoById");

// POST /api/todos - Create a new todo
app.MapPost("/api/todos", async (TodoItem todo, TodoContext db) =>
{
    if (string.IsNullOrWhiteSpace(todo.Title))
    {
        return Results.BadRequest(new { error = "Title is required" });
    }
    
    todo.CreatedAt = DateTime.UtcNow;
    db.TodoItems.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/api/todos/{todo.Id}", todo);
})
.WithName("CreateTodo");

// PUT /api/todos/{id} - Update a todo
app.MapPut("/api/todos/{id}", async (int id, TodoItem updatedTodo, TodoContext db) =>
{
    if (string.IsNullOrWhiteSpace(updatedTodo.Title))
    {
        return Results.BadRequest(new { error = "Title is required" });
    }
    
    var todo = await db.TodoItems.FindAsync(id);
    if (todo is null) return Results.NotFound();

    var wasCompleted = todo.IsCompleted;
    
    todo.Title = updatedTodo.Title;
    todo.Description = updatedTodo.Description;
    todo.IsCompleted = updatedTodo.IsCompleted;
    
    if (updatedTodo.IsCompleted && !wasCompleted)
    {
        todo.CompletedAt = DateTime.UtcNow;
    }
    else if (!updatedTodo.IsCompleted)
    {
        todo.CompletedAt = null;
    }

    await db.SaveChangesAsync();
    return Results.Ok(todo);
})
.WithName("UpdateTodo");

// DELETE /api/todos/{id} - Delete a todo
app.MapDelete("/api/todos/{id}", async (int id, TodoContext db) =>
{
    var todo = await db.TodoItems.FindAsync(id);
    if (todo is null) return Results.NotFound();

    db.TodoItems.Remove(todo);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteTodo");

app.Run();
