using Auth0.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using SpaceCatan.Components;
using SpaceCatan.Endpoints;
using SpaceCatan.Lobbies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddDbContext<SpaceCatanContext>(o =>
{
    o.UseSqlite();
});
builder.Services.AddScoped<IUserStore, EFCoreUserStore>();
builder.Services.AddSingleton<ILobbyStore, InMemoryLobbyStore>();

builder.Services.AddAuth0WebAppAuthentication(o =>
{
    o.Domain = builder.Configuration["Auth0:Domain"]!;
    o.ClientId = builder.Configuration["Auth0:ClientID"]!;
});

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapLoginEndpoints();

app.Run();
