using FD.TaskManager.API.Extentions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Siahroud.Logic.Services;
using System.Text;
using TaskManager.DataAccess.DbConnection;
using TaskManager.DataAccess.Repository;
using TaskManager.Logic.AppManager;
using TaskManager.Logic.Mapping;
using TaskManager.Logic.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<DataBaseConnection>();
builder.Services.AddSingleton<TenantRepository>();
builder.Services.AddSingleton<TenantManager>();
builder.Services.AddSingleton<TenantMapping>();
builder.Services.AddSingleton<AdminRepository>();
builder.Services.AddSingleton<AdminManager>();
builder.Services.AddSingleton<AdminMapping>();
builder.Services.AddSingleton<ManagerRepository>();
builder.Services.AddSingleton<MManager>();
builder.Services.AddSingleton<ManagerMapping>();
builder.Services.AddSingleton<MemberRepository>();
builder.Services.AddSingleton<MemberManager>();
builder.Services.AddSingleton<MemberMapping>();
builder.Services.AddSingleton<ProjectRepository>();
builder.Services.AddSingleton<ProjectManager>();
builder.Services.AddSingleton<ProjectMapping>();
builder.Services.AddSingleton<BoardRepository>();
builder.Services.AddSingleton<BoardManager>();
builder.Services.AddSingleton<BoardMapping>();
builder.Services.AddSingleton<TaskItemRepository>();
builder.Services.AddSingleton<TaskItemManager>();
builder.Services.AddSingleton<TaskItemMapping>();
builder.Services.AddSingleton<SubscriptionRepository>();
builder.Services.AddSingleton<SubPlanManager>();
builder.Services.AddSingleton<SubscriptionMapping>();
builder.Services.AddSingleton<PasswordHashing>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<EmailVerification>();
builder.Services.AddSingleton<SMSService>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();

// Jwt bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                var request = context.HttpContext.Request;
                if (request.Cookies.TryGetValue("auth-token", out var token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!))
        };
    });

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
    | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.MapScalarApiReference(options =>
    {
        options
        .WithTitle("FD.TaskManager.API")
        .WithTheme(ScalarTheme.Moon);
    });
    app.MapOpenApi();
//}

app.UseForwardedHeaders();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.UseMiddleware<ProfileMiddleware>();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
