using Euclid47.AsciiArt.Lib.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Euclid47.AsciiArt.Example
{
	internal class App : IApp
	{
		private readonly ILogger<App> _logger;
		private readonly IGenerator _generator;

		public App(ILogger<App> logger, IGenerator generator)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_generator = generator ?? throw new ArgumentNullException(nameof(generator));
		}

		public async Task Run(string imageLocation)
		{
			_logger.LogInformation("Started Run");

			try
			{
				var result = await _generator.Transform(imageLocation,125);
				Console.WriteLine(result);
			}
			catch (Exception e)
			{
				_logger.LogError($"[{MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
			}
		}
	}
}
