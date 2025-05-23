using API.Business_Layer.Infrastructure;
using API.Business_Layer.Services;
using API.Data_Layer;
using API.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(x => x
     .AllowAnyHeader()
     .AllowAnyMethod()
     .WithOrigins("http://localhost:4200", "https://localhost:4200")
     );
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
