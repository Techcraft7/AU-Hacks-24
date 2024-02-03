using Auth0.AspNetCore.Authentication;
using SpaceCatan.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuth0WebAppAuthentication(o =>
{
    o.Domain = builder.Configuration["Auth0:Domain"]!;
    o.ClientId = builder.Configuration["Auth0:ClientID"]!;
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>();

app.Run();
