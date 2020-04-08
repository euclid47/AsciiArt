using Euclid47.AsciiArt.Lib;
using Euclid47.AsciiArt.Lib.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Euclid47.AsciiArt.Example
{
	class Program
	{
		private static IServiceProvider serviceProvider;
		private static IConfiguration configuration;

		static async Task Main(string[] args)
		{
			SetConfiguration();
			ConfigureServices();
			var app = serviceProvider.GetService<IApp>();

			await app.Run("https://s3-wp-lyleprintingandp.netdna-ssl.com/wp-content/uploads/2018/01/09060054/cow-354428_1280.jpg");

			Console.WriteLine("Done. Press any key to continue.");
			Console.ReadKey();
			Environment.Exit(0);
		}

		private static void SetConfiguration()
		{
			configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", true, true)
				.Build();
		}

		private static void ConfigureServices()
		{
			serviceProvider = new ServiceCollection()
				.AddLogging(x => x.AddConsole())
				.AddSingleton<IGenerator, Generator>()
				.AddSingleton<IApp, App>()
				.BuildServiceProvider();
		}
	}
}
