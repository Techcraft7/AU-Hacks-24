using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SpaceCatan.Components;
using SpaceCatan.Endpoints;
using SpaceCatan.Lobbies;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddDbContext<SpaceCatanContext>();
builder.Services.AddScoped<IUserStore, EFCoreUserStore>();
builder.Services.AddSingleton<ILobbyStore, InMemoryLobbyStore>();

builder.Services.AddAuth0WebAppAuthentication(o =>
{
	o.Domain = builder.Configuration["Auth0:Domain"]!;
	o.ClientId = builder.Configuration["Auth0:ClientID"]!;
});
builder.Services.AddOptions<OpenIdConnectOptions>(Auth0Constants.AuthenticationScheme)
	.Configure<IServiceProvider>((o, sp) =>
	{
		o.Events.OnTokenValidated = async (ctx) =>
		{
			if (ctx.Principal?.Identity?.IsAuthenticated is null or false)
			{
				return;
			}
			if (ctx?.Principal.FindFirst("id")?.Value is not string userID)
			{
				return;
			}

			using IServiceScope scope = sp.CreateScope();
			(User user, Exception error) = await scope.ServiceProvider.GetRequiredService<IUserStore>().CreateUser(userID, default);
		};
	});

WebApplication app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();
app.MapLoginEndpoints();

app.Run();
