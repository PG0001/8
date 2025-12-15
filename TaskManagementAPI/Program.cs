using Library8;
using Library8.Models;
using Library8.Models.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TaskManagementAPI.BackgroundJobs;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Hubs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;
using TaskManagementAPI.Services.Interfaces;
using TaskManagementAPI.Services.Interfaces;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("defaultConnection")
    )
);
// -------------------- SERVICES --------------------

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// ✅ Swagger with JWT Support
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer {your_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3000", 
                "http://localhost:5173"  
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // REQUIRED for SignalR
    });
});

// -------------------- DI --------------------
// -------------------- DI --------------------

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskCommentRepository, TaskCommentRepository>();
builder.Services.AddScoped<ITaskTimelineRepository, TaskTimelineRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();

// Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddHostedService<TaskDueReminderWorker>();

// (optional for next step)
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();


// -------------------- AUTHENTICATION --------------------

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    // ✅ Prevents automatic claim remapping
    options.MapInboundClaims = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        ),

        // ✅ REQUIRED for Role-based auth
        RoleClaimType = "role"
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/hubs"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };

});

builder.Services.AddAuthorization();

var app = builder.Build();


// -------------------- PIPELINE --------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<TaskHub>("/hubs/tasks");
app.MapHub<ChatHub>("/hubs/chat");
app.MapHub<NotificationHub>("/hubs/notifications");


app.Run();
