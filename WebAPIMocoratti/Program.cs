using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using WebAPIMocoratti.Context;
using WebAPIMocoratti.DTOs.Mappings;
using WebAPIMocoratti.Filter;
using WebAPIMocoratti.Repository;
using WebAPIMocoratti.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//Corrigir erro de referencia c�clica
builder.Services.AddControllers().AddJsonOptions(options =>
options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
//Configura��es para adicionar o JWT no swegger
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "APICatalogo",
        Version = "v1",
        Description = "Curso de Web API com professor Macoratti",
        Contact = new OpenApiContact
        {
            Name = "Victor",
            Email = "victoremannuel1156@gmail.com",

        }
    });
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Header  de autoriza��o do JWT usando o esquema Bearer.\r\n\r\nInforme'Bearer[espa�o]' e o seu token."
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    s.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<ApiLogginFilter>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

builder.Services.AddCors(opt => opt.AddPolicy("PermitirAPIRequest", otherOpt => otherOpt.AllowAnyOrigin().WithMethods("GET")));


IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
string stringConection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(stringConection);
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
        ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
    };
});
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
});
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API CAT�LOGO");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();
