namespace Coworking.Web.Extensions
{
    public static class ApiHealthExtensions
    {
        public static IApplicationBuilder UseApiHealthCheck(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/" && !await IsApiOnline(context.RequestServices))
                {
                    context.Response.Redirect("/Home/ApiOffline");
                    return;
                }

                await next();
            });
        }

        private static async Task<bool> IsApiOnline(IServiceProvider services)
        {
            try
            {
                var factory = services.GetRequiredService<IHttpClientFactory>();
                var client = factory.CreateClient("HealthCheck");

                client.Timeout = TimeSpan.FromSeconds(2);

                var response = await client.GetAsync("/health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
