using EmployeeManagementMVCAPP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EmployeeDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDbConnectionString")));
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployeeManagementMVCAPP", Version = "v1" });
});

builder.Services.AddCors(o=>o.AddPolicy("MyPloicy", builder=>
builder.WithOrigins("https://localhost:7067","http://localhost:5067")
.SetIsOriginAllowedToAllowWildcardSubdomains()
.AllowAnyOrigin()
.AllowAnyMethod()
));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Employee/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployeeManagementMVCAPP.API v1");
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//app.MapControllers();
app.UseAuthorization();
app.UseCors(builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=Index}/{id?}");

app.Run();
