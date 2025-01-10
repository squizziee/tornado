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
					options => options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = configuration["JWT:issuer"],
						ValidAudience = configuration["JWT:audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]!))
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
