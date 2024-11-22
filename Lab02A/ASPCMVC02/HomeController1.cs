using Microsoft.AspNetCore.Mvc;

namespace ASPCMVC01
{
    public class HomeController1 : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public void ConfigureServices(IServiceCollection services) // регистрация сервисов
        {
            services.AddMvc();
        }


        public void Configure(IApplicationBuilder app , IWebHostEnvironment env, ILoggerFactory logger) // обработка запроса
        {
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapGet("/Index.html", async (context) => {
                    await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "Index.html"));
                });


                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }

    }
}
