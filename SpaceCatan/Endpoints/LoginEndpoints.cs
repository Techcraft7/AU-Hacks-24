using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SpaceCatan.Endpoints;

public static class LoginEndpoints
{
	public static void MapLoginEndpoints(this WebApplication app)
	{
		app.MapGet("/login", async (string? redirectUri, HttpContext ctx) =>
		{
			var properties = new LoginAuthenticationPropertiesBuilder()
				.WithRedirectUri(redirectUri ?? "/")
				.Build();

			await ctx.ChallengeAsync(Auth0Constants.AuthenticationScheme, properties);
		});
		app.MapGet("/signup", async (string? redirectUri, HttpContext ctx) =>
		{
			var properties = new LoginAuthenticationPropertiesBuilder()
				.WithRedirectUri(redirectUri ?? "/")
				.WithParameter("screen_hint", "signup")
				.Build();

			await ctx.ChallengeAsync(Auth0Constants.AuthenticationScheme, properties);
		});
		app.MapGet("/logout", async (HttpContext ctx) =>
		{
			var properties = new LoginAuthenticationPropertiesBuilder()
				.WithRedirectUri("/")
				.Build();

			await Task.WhenAll(
				ctx.SignOutAsync(Auth0Constants.AuthenticationScheme, properties),
				ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme));
		});
	}
}
