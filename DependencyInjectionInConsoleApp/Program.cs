namespace DIInConsoleApp
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using StructureMap;
    using System;
    class Program
    {
        static void Main(string[] args)
        {
            #region 内置依赖注入
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IFooService, FooService>()
                .AddSingleton<IBarService, BarService>()
                .BuildServiceProvider();

            serviceProvider.GetService<ILoggerFactory>().AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            logger.LogInformation("Start application.");

            var bar = serviceProvider.GetService<IBarService>();
            bar.DoSomeRealWork();

            logger.LogInformation("All done!");

            Console.ReadLine();

            #endregion

            #region StuctureMap

            var services = new ServiceCollection().AddLogging();

            // add StructureMap
            var container = new Container();
            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(Program));
                    _.WithDefaultConventions();
                });

                config.Populate(services);
            });

            var serviceProvider2 = container.GetInstance<IServiceProvider>();

            serviceProvider2.GetService<ILoggerFactory>().AddConsole(LogLevel.Debug);

            logger = serviceProvider2.GetService<ILoggerFactory>().CreateLogger<Program>();
            logger.LogInformation("Start application.");

            bar = serviceProvider2.GetService<IBarService>();
            bar.DoSomeRealWork();

            logger.LogInformation("All done!");
            Console.Read();

            #endregion


            Console.Read();
        }
    }
}
