using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using week2_Task.Data;
using week2_Task.Models;
using week2_Task.Repositories;
using week2_Task.Services;
using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using week2_Task.MiddleWares;
using week2_Task.Exceptions;



var builder = WebApplication.CreateBuilder(args);






//serilog 

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

// Use Serilog for logging
builder.Host.UseSerilog();




///////////////////////////////////
//json format in console but parsed

builder.Logging.AddJsonConsole(options =>
{
    
    options.JsonWriterOptions = new System.Text.Json.JsonWriterOptions
    {
        Indented = true                 
    };
}); 
/////////////////////



Log.Logger = new LoggerConfiguration()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true 
        })
    .CreateLogger();
///http logging




builder.Services.AddHttpLogging(options => { });


// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.CommandTimeout(30);
            sqlOptions.EnableRetryOnFailure();
        }));

// Identity setup with roles
builder.Services.AddIdentity<APPUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]))
    };
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Dependency Injection
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<LinqDemoService>();
builder.Services.AddSingleton<TokenService>();










var app = builder.Build();
app.UseHttpLogging();




//example of db connection
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    

    if (context.Database.CanConnect())
    {
        app.Logger.LogInformation("Database setup completed at {Time}", DateTime.UtcNow);
    }
    else
    {
        app.Logger.LogError("Failed to connect ");
    }
}


//just msg


app.Logger.LogInformation(5, "db is ready");
app.Logger.LogWarning(5, "db is ready");

// Role seeding like the first register
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}


// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors("AllowAll");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

