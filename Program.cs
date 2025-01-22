using EmailService.Configuration;
using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Services;
using PaymentService.Domain.Interfaces;
using PaymentService.Infrastructure.Configuration;
using PaymentService.Infrastructure.Context;
using PaymentService.Infrastructure.Data;
using EmailService.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext
builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<EmailConfiguration>(
    builder.Configuration.GetSection("EmailConfiguration"));

// Registro dos serviços
builder.Services.AddScoped<IPaymentService, PaymentServiceCore>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.Configure<CieloSettings>(builder.Configuration.GetSection("CieloIntegrationSettings"));
builder.Services.AddHttpClient<CieloIntegrationService>();
builder.Services.AddScoped<CieloIntegrationService>();
builder.Services.AddTransient<IEmailService, EmailService.Application.Services.EmailService>();

// Adiciona suporte a controladores e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração do pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
