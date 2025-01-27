using Backend.Models;
using Backend.Services;
using Backend.Services.Impl;
using Backend.Validations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HRDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddScoped<IMedicalRepository, MedicalRepositoryImpl>();
builder.Services.AddScoped<IMedicalServices, MedicalServicesImpl>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateMedicalDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DeleteMedicalDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateMedicalDtoValidator>();

builder.Services.AddAutoMapper(typeof(Program));

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
