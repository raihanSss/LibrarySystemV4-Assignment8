using System.Text;
using LibrarySystem.Application;
using LibrarySystem.Application.Services;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using LibrarySystem.Infrastructure;
using LibrarySystem.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SixLabors.ImageSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



// Konfigurasi koneksi ke PostgreSQL
var connectionStringPostgre = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<LibrarysystemDbContext>(options => options.UseNpgsql(connectionStringPostgre));


builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{

    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = true;

})
.AddEntityFrameworkStores<LibrarysystemDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])),
        };
    });

var emailConfig = builder.Configuration
.GetSection("MailSettings")
.Get<MailSettings>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.Configure<FormOptions>(Options =>
{
    Options.ValueLengthLimit = int.MaxValue;
    Options.MultipartBodyLengthLimit = int.MaxValue;
    Options.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.Configure<PenaltySettings>(builder.Configuration.GetSection("PenaltySettings"));

// Registrasi repository
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWorkflowRepository, WorkflowRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();  

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddTransient<IEmailService, EmailService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
