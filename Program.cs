using System.Globalization;
using Microsoft.AspNetCore.HttpOverrides;
using wt_lab4_4mvc_ostashevskaya.Infrastructure;

// Единая культура для форматирования цен и дат в интерфейсе.
var culture = new CultureInfo("ru-RU");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

var builder = WebApplication.CreateBuilder(args);

// MVC: контроллеры + Razor Views.
builder.Services.AddControllersWithViews(options =>
{
    // Цена принимается и с точкой, и с запятой.
    options.ModelBinderProviders.Insert(0, new InvariantDecimalModelBinderProvider());
});

// Render передаёт порт через переменную окружения PORT.
// По умолчанию используется 10000 — стандартный порт Web Service на Render.
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Render завершает HTTPS на балансировщике и передаёт запрос по HTTP.
// Эти заголовки позволяют приложению корректно определить исходную схему.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseStatusCodePagesWithReExecute("/Home/Status", "?code={0}");
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
