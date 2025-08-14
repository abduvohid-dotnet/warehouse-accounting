using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WarehouseAccounting.Data;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2️⃣ Controller + JSON qo‘llab-quvvatlash
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// 3️⃣ Swagger sozlamalari
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Practice API",
        Version = "v1",
        Description = "My Practice Project API with PostgreSQL"
    });
});

var app = builder.Build();

// 4️⃣ Swagger UI faqat development rejimida
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Practice API v1");
        c.RoutePrefix = "swagger"; // Swagger asosiy sahifa bo‘lishi uchun
    });
}

app.UseHttpsRedirection();

// 5️⃣ Authorization (agar kerak bo‘lsa)
app.UseAuthorization();

// 6️⃣ Controller route mapping
app.MapControllers();

app.Run();
