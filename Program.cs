using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Chaos.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
            .AddMicrosoftIdentityConsentHandler();
//builder.Services.AddServerSideBlazor().AddHubOptions(o =>
//{
//    o.MaximumReceiveMessageSize = 10 * 1024 * 1024;
//});
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).
                    AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd")).
                    EnableTokenAcquisitionToCallDownstreamApi().
                    AddInMemoryTokenCaches();

//builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration.GetSection("AzureAd")
//                    .Enab

builder.Services.AddScoped<Api>();
builder.Services.AddAuthorization();
builder.Services.AddHttpClient("Chaos").AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddScoped<AuthenticationStateProvider, Chaos.ApplicationAuthenticationStateProvider>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<Chaos.SecurityService>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseHeaderPropagation();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();