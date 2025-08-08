using ContosoPizza.Data;
using ContosoPizza.Repositories;
using ContosoPizza.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<PizzaRepository>();
builder.Services.AddScoped<PizzaService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")   // <-- exact origin of your Angular app
              .AllowAnyHeader()
              .AllowAnyMethod();
              // .AllowCredentials(); // only if you need cookies/auth — see note below
    });
});

var app = builder.Build();

app.UseCors("AllowAngularDev");
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


// Map a basic test endpoint

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Dapper CRUD API is running...");

app.Run();
