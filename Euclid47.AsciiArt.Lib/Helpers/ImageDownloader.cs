using Euclid47.AsciiArt.Lib.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Euclid47.AsciiArt.Lib.Helpers
{
	internal class ImageDownloader : IImageFetcher
	{
		public ILogger<ImageDownloader> _logger;

		public ImageDownloader(ILogger<ImageDownloader> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<Bitmap> Get(string imageLocation)
		{
			if (string.IsNullOrWhiteSpace(imageLocation))
				throw new ArgumentNullException(nameof(imageLocation));

			if (!Uri.TryCreate(imageLocation, UriKind.RelativeOrAbsolute, out var uri))
				throw new ArgumentException($"The url, {imageLocation}, is not valid.");

			try
			{
				using (var request = new WebClient())
				{
					var data = await request.DownloadDataTaskAsync(uri);

					if (data != null && data.Length > 0)
					{
						using (var stream = new MemoryStream(data))
						{
							return new Bitmap(stream);
						}
					}
				}
			}
			catch (Exception e)
			{
				_logger.LogError($"[{MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
			}

			return null;
		}

		public Task<bool> IsValid(string imageLocation)
		{
			return Task.FromResult(!string.IsNullOrWhiteSpace(imageLocation) && Uri.IsWellFormedUriString(imageLocation, UriKind.RelativeOrAbsolute));
		}
	}
}
