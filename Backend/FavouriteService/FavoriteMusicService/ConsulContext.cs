using Consul;

namespace FavoriteMusicService
{
    public static class ConsulContext
    {
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            //adding consul from appsettings details
            services.AddSingleton<IConsulClient, ConsulClient>(options => new ConsulClient(opt =>
            {
                //get the consul host details from appsettings file
                var consulHost = configuration.GetValue<string>("ServiceConfiguration:ConsulHost");
                opt.Address = new Uri(consulHost);

            }));
            return services;
        }
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IConfiguration configuration)
        {
            //register the microservice using service details from appsettings

            var register = new AgentServiceRegistration()
            {
                ID = configuration.GetValue<string>("ServiceConfiguration:ServiceName"),
                Name = configuration.GetValue<string>("ServiceConfiguration:ServiceName"),
                //Address =Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString(),
                Address = configuration.GetValue<string>("ServiceConfiguration:ServiceHost"),
                Port = configuration.GetValue<int>("ServiceConfiguration:ServicePort")
            };
            //get the consul reference 
            var consulServer = app.ApplicationServices.GetService<IConsulClient>();
            //register the api service with consul
            consulServer.Agent.ServiceDeregister(register.ID).ConfigureAwait(true);
            consulServer.Agent.ServiceRegister(register).ConfigureAwait(true);

            return app;
        }
    }
}
