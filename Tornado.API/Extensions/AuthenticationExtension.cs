using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Tornado.API.Extensions
{
	public static class AuthenticationExtension
	{
		public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(
					options =>
					{
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration["Jwt:AccessToken:Issuer"],
                            ValidAudience = configuration["Jwt:AccessToken:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:AccessToken:Key"]!))
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                context.Request.Cookies.TryGetValue("accessToken", out var accessToken);
                                if (!string.IsNullOrEmpty(accessToken))
                                    context.Token = accessToken;
                                return Task.CompletedTask;
                            }
                        };
                    }
					
				);

			services.AddAuthorizationBuilder()
                .AddPolicy("AdminPolicy", policy =>
						policy.RequireRole("Admin"))
                .AddPolicy("ModeratorPolicy", policy =>
                        policy.RequireRole("Moderator"))
				.AddPolicy("ViewerPolicy", policy =>
                        policy.RequireRole("Viewer"));

			return services;
        }
	}
}
