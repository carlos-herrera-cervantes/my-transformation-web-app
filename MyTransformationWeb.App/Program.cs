using Blazored.LocalStorage;

using MyTransformationWeb.Domain.Config;
using MyTransformationWeb.Services.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthenticationCore();
builder.Services.AddHttpClient("my-transformation", c =>
{
    c.BaseAddress = new Uri(ExternalServicesConfig.GatewayConfig.Host);
});
builder.Services.AddSingleton<IExerciseService, ExerciseService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
