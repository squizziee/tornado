using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Tornado.API.Extensions;
using Tornado.Application.AutoMapperProfiles;
using Tornado.Application.UseCases;
using Tornado.Application.UseCases.Auth;
using Tornado.Application.UseCases.Interfaces;
using Tornado.Application.UseCases.Interfaces.Auth;
using Tornado.Application.UseCases.Interfaces.Profile;
using Tornado.Application.UseCases.Profile;
using Tornado.Infrastructure.Data;
using Tornado.Infrastructure.Data.Repositories;
using Tornado.Infrastructure.Interfaces;
using Tornado.Infrastructure.Interfaces.Repositories;
using Tornado.Infrastructure.Services;
using Tornado.Infrastructure.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 500 * 1024 * 1024);
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 500 * 1024 * 1024;
});

builder.Services.AddDbContext<ApplicationDatabaseContext>(options => 
    options.UseNpgsql(connectionString: builder.Configuration.GetConnectionString("DefaultDatabaseConnectionString"))
);

builder.Services.AddScoped<Mapper>();

builder.Services.AddAutoMapper(typeof(UserMapperProfile));
builder.Services.AddAutoMapper(typeof(UserProfileMapperProfile));

// binding appsettings.json sections
builder.Services.Configure<ImageUploadSettings>(builder.Configuration.GetSection("ImageUpload"));

// unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// repos
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IUserCommentRepository, UserCommentsRepository>();
builder.Services.AddScoped<IUserRatingsRepository, UserRatingsRepository>();

// auth and policies
builder.Services.AddJwtAuthentication(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUploadVideoUseCase, UploadVideoUseCase>();
builder.Services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
builder.Services.AddScoped<ILoginWithEmailAndPasswordUseCase, LoginWithEmailAndPasswordUseCase>();
builder.Services.AddScoped<IRefreshTokensUseCase, RefreshTokensUseCase>();
builder.Services.AddScoped<IGetUserInfoUseCase, GetUserInfoUseCase>();

builder.Services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
builder.Services.AddScoped<IUpdateUserProfileUseCase, UpdateUserProfileUseCase>();

builder.Services.AddSingleton<IVideoUploadService, VideoUploadService>();
builder.Services.AddSingleton<IImageUploadService, ImageUploadService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHashingService, PasswordHashingService>();

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

IWebHostEnvironment a;