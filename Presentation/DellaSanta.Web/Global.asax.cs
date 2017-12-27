using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Helpers;
using System.Security.Claims;
using Autofac;
using System.Configuration;
using DellaSanta.Services;
using DellaSanta.DataLayer;
using Autofac.Integration.Mvc;
using Dellasanta.Web.Common.Security;

namespace DellaSanta
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RegisterDependencies();
            AutoMapperConfiguration.Initialize();
            ConfigureAntiForgeryTokens();
           
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            //ConfigureLog4Net(builder, connectionString);

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // HttpContext
            //builder.Register(c => new HttpContextWrapper(HttpContext.Current) as HttpContextBase).As<HttpContextBase>().InstancePerLifetimeScope();
            //builder.Register(c => c.Resolve<HttpContextBase>().Request).As<HttpRequestBase>().InstancePerLifetimeScope();
            //builder.Register(c => c.Resolve<HttpContextBase>().Response).As<HttpResponseBase>().InstancePerLifetimeScope();
            //builder.Register(c => c.Resolve<HttpContextBase>().Server).As<HttpServerUtilityBase>().InstancePerLifetimeScope();
            //builder.Register(c => c.Resolve<HttpContextBase>().Session).As<HttpSessionStateBase>().InstancePerLifetimeScope();

            // Services
            //builder.RegisterType<ActiveDirectoryService>().As<IActiveDirectoryService>().InstancePerLifetimeScope();
            //builder.RegisterType<DateTimeAdapter>().As<IDateTime>().InstancePerLifetimeScope();
            //builder.RegisterType<DomainService>().As<IDomainService>().InstancePerLifetimeScope();
            //builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerLifetimeScope();
            //builder.RegisterType<EmailTemplateService>().As<IEmailTemplateService>().InstancePerLifetimeScope();
            //builder.RegisterType<LogService>().As<ILogService>().InstancePerLifetimeScope();
            //builder.RegisterType<MessageService>().As<IMessageService>().InstancePerLifetimeScope();
            //builder.RegisterType<MemoryCacheService>().As<ICacheService>().SingleInstance();
            builder.RegisterType<OwinAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            //builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            //builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();
            //builder.RegisterType<TraceLogService>().As<ITraceLogService>().InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().As<ApplicationDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();


            //// Trace listener
            //builder.RegisterType<DatabaseTraceListener>().As<ITraceListener>()
            //    .WithParameter("connectionString", connectionString)
            //    .InstancePerLifetimeScope();

            //// Respository
            //builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            //builder.Register<DbContext>(c => new AppDbContext(connectionString)).InstancePerLifetimeScope();

            //// Register controllers
            //builder.RegisterControllers(typeof(Global).Assembly);

            //// Register Mvc filters
            //builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            
        }

    private void ConfigureLog4Net(ContainerBuilder builder, string connectionString)
        {
            //Log4NetLogger.Configure(connectionString);
            //builder.Register(c => new LogManagerAdapter()).As<ILogManager>().InstancePerLifetimeScope();
        }

        private void ConfigureAntiForgeryTokens()
        {
            // We use Owin Cookie Authentication, so we set AntiForgery token to use Name as claim types.
            // If you use different claim type, you need to synchronize with 
            // AspNetMvcActiveDirectoryOwin.Web.Common.Security.OwinAuthenticationService.
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;

            // Anti-Forgery cookie requires SSL to be sent across the wire. 
            //AntiForgeryConfig.RequireSsl = true;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            LogException(exception);

            //if (exception is HttpAntiForgeryException)
            //{
            //    Response.Clear();
            //    Server.ClearError();
            //    Response.TrySkipIisCustomErrors = true;

            //    // Call target Controller and pass the routeData.
            //    IController controller = EngineContext.Current.Locator.GetInstance<CommonController>();

            //    var routeData = new RouteData();
            //    routeData.Values.Add("controller", "Common");
            //    routeData.Values.Add("action", "AntiForgery");

            //    var requestContext = new RequestContext(new HttpContextWrapper(Context), routeData);
            //    controller.Execute(requestContext);
            //}
            //else
            //{
            //    // Process 404 HTTP errors
            //    var httpException = exception as HttpException;
            //    if (httpException != null && httpException.GetHttpCode() == 404)
            //    {
            //        Response.Clear();
            //        Server.ClearError();
            //        Response.TrySkipIisCustomErrors = true;

            //        // Call target Controller and pass the routeData.
            //        IController controller = EngineContext.Current.Locator.GetInstance<CommonController>();

            //        var routeData = new RouteData();
            //        routeData.Values.Add("controller", "Common");
            //        routeData.Values.Add("action", "PageNotFound");

            //        var requestContext = new RequestContext(new HttpContextWrapper(Context), routeData);
            //        controller.Execute(requestContext);
            //    }
            //}

           
        }


        private void LogException(Exception ex)
        {
            if (ex == null)
                return;

            // Ignore 404 HTTP errors
            var httpException = ex as HttpException;
            if (httpException != null &&
                httpException.GetHttpCode() == 404)
                return;

            try
            {
                //var log = EngineContext.Current.Locator.GetInstance<ILogManager>().GetLog(typeof(Global));
                //log.Error(ex);
            }
            catch (Exception)
            {
                // Don't throw new exception if occurs
            }
        }

    }
}
