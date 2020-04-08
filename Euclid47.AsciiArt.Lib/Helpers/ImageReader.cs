using Euclid47.AsciiArt.Lib.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Euclid47.AsciiArt.Lib.Helpers
{
	internal class ImageReader : IImageFetcher
	{
		private readonly ILogger<ImageReader> _logger;

		public ImageReader(ILogger<ImageReader> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public Task<Bitmap> Get(string imageLocation)
		{
			if (string.IsNullOrWhiteSpace(imageLocation))
				throw new ArgumentNullException(nameof(imageLocation));

			if (!File.Exists(imageLocation))
				throw new FileNotFoundException($"The file, {imageLocation}, could not be found.");

			try
			{
				var fileData = File.ReadAllBytes(imageLocation);

				using(var ms = new MemoryStream(fileData))
				{
					return Task.FromResult(new Bitmap(ms));
				}
			}
			catch(Exception e)
			{
				_logger.LogError($"[{MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
			}

			return null;
		}

		public Task<bool> IsValid(string imageLocation)
		{
			return Task.FromResult(!string.IsNullOrWhiteSpace(imageLocation) && File.Exists(imageLocation));
		}
	}
}
