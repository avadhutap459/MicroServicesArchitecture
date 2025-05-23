using AutoMapper;
using Mango.MessageBus;
using Mango.Service.ShoppingCartAPI;
using Mango.Service.ShoppingCartAPI.Data;
using Mango.Service.ShoppingCartAPI.Extension;
using Mango.Service.ShoppingCartAPI.Service;
using Mango.Service.ShoppingCartAPI.Service.IService;
using Mango.Service.ShoppingCartAPI.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Add services to the container.

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IMessageBus,MessageBus>();
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<BackendApiAuthencationHttpClientHandler>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddHttpClient("Product", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));
builder.Services.AddHttpClient("Coupon", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"]));


//builder.Services.AddHttpClient("Product", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"])).AddHttpMessageHandler<BackendApiAuthencationHttpClientHandler>();
//builder.Services.AddHttpClient("Coupon", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"])).AddHttpMessageHandler<BackendApiAuthencationHttpClientHandler>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following : `Bearer Generated-JWT-Token`",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            },new string[]{}
        }
    });
});


WebApplicationBuilderExtension.AddAppAuthencation(builder);

builder.Services.AddAuthorization();

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
ApplyMigration();
app.Run();



void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}
