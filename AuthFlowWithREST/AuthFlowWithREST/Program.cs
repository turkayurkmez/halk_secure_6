using AuthFlowWithREST.Security;
using AuthFlowWithREST.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UserService>();

//Not: Basic Authentication Scheme:
//builder.Services.AddAuthentication("Basic")
//                .AddScheme<BasicOption, BasicHandler>("Basic", null);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = "halkbank.server",
                        ValidAudience = "halkbank.mobile",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aman-burasi-cok-onemli")),
                        ValidateIssuerSigningKey = true
                    };
                });

builder.Services.AddCors(option => option.AddPolicy("allow", builder =>
{
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    builder.AllowAnyOrigin();

    //http://www.halkbank.com.tr/contact/info
    //https://www.halkbank.com.tr
    //https://api.halkbank.com.tr
    //https://api.halkbank.com.tr:1246


}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("allow");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
