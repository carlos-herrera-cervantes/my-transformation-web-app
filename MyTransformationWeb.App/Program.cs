using Blazored.LocalStorage;

using MyTransformationWeb.Domain.Config;
using MyTransformationWeb.Services.Calories;
using MyTransformationWeb.Services.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthenticationCore();
builder.Services.AddHttpClient("my-transformation-core", c =>
{
    c.BaseAddress = new Uri(ExternalServicesConfig.GatewayConfig.CoreApiHost);
});
builder.Services.AddHttpClient("my-transformation-calories", c =>
{
    c.BaseAddress = new Uri(ExternalServicesConfig.GatewayConfig.CaloriesApiHost);
});
builder.Services.AddSingleton<IExerciseService, ExerciseService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUserProgressService, UserProgressService>();
builder.Services.AddSingleton<IFoodService, FoodService>();

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
