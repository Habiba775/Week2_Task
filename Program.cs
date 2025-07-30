using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Duende.IdentityServer;
using week2_Task.Data;
using week2_Task.Models;
using week2_Task.Repositories;
using week2_Task.Services;
using week2_Task.MiddleWares;
using week2_Task.Exceptions;
using week2_Task;

var builder = WebApplication.CreateBuilder(args);

// ========== Serilog Setup ==========
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.MSSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true })
    .CreateLogger();

builder.Host.UseSerilog();

builder.Logging.AddJsonConsole(options =>
{
    options.JsonWriterOptions = new System.Text.Json.JsonWriterOptions { Indented = true };
});

// ========== Services ==========
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ========== CORS ==========
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// ========== DbContext ==========
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ========== Identity ==========
builder.Services.AddIdentity<APPUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

// ========== IdentityServer ==========
builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddAspNetIdentity<APPUser>()
    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
    .AddDeveloperSigningCredential();

builder.Services.AddLocalApiAuthentication(); 

// ========== Authorization ==========
builder.Services.AddAuthorization(); // No custom policy needed

// ========== Custom Exception & Middleware ==========
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
});

// ========== Dependency Injection ==========
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<LinqDemoService>();
builder.Services.AddSingleton<TokenService>();

var app = builder.Build();

// ========== Middleware ==========
app.UseHttpLogging();
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseExceptionHandler();

// ========== Controller Routing ==========
app.MapControllers();

// ========== DB Test ==========
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    if (context.Database.CanConnect())
        app.Logger.LogInformation("Database connected at {Time}", DateTime.UtcNow);
    else
        app.Logger.LogError("Database connection failed.");
}

// ========== Seed Roles ==========
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

app.Run();


