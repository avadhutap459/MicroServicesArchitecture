using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Mango.Service.ProductAPI.Extension
{
    public static class WebApplicationBuilderExtension
    {
        public static WebApplicationBuilder AddAppAuthencation(this WebApplicationBuilder builder)
        {
            var settingSections = builder.Configuration.GetSection("ApiSettings");

            var secret = settingSections.GetValue<string>("Secret");
            var Issuer = settingSections.GetValue<string>("Issuer");
            var Audience = settingSections.GetValue<string>("Audience");

            var key = Encoding.ASCII
                .GetBytes(secret);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    ValidateAudience = true
                };
            });

            return builder;
        }
    }
}
