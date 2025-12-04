using Microsoft.EntityFrameworkCore;
using VentasProject.Api.Data.Context;
using VentasProject.Api.Data.Interface;
using VentasProject.Api.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICustomerApiRepository, CustomerApiRepository>();
builder.Services.AddScoped<IProductApiRepository, ProductApiRepository>();
builder.Services.AddDbContext<CustomerContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDbContext<ProductContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
