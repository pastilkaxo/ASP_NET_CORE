
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "M01",
    pattern: "{controller=TMResearch}/{action=M01}/{id:int?}");

app.MapControllerRoute(
    name: "M02",
    pattern: "V2/{controller=TMResearch}/{action=M02}/{str?}",
    constraints: new { action = "M01|M02" });

app.MapControllerRoute(
    name: "M03",
    pattern: "V3/{controller=TMResearch}/{str?}/{action=M03}",
    constraints: new { action = "M01|M02|M03" });

app.MapControllerRoute(
    name: "MXX",
    pattern: "{*url}",
    defaults: new { controller = "TMResearch", action = "MXX" });

app.Run();
