using Euclid47.AsciiArt.Lib.Helpers;
using Euclid47.AsciiArt.Lib.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;

namespace Euclid47.AsciiArt.Lib
{
	public class Generator : IGenerator
	{
		private readonly ILogger<Generator> _logger;
		private readonly ITransformer _transformer;
		private readonly IImageFetcher _web;
		private readonly IImageFetcher _filesystem;

		public Generator(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<Generator>();
			_transformer = new Transformer(loggerFactory.CreateLogger<Transformer>());
			_web = new ImageDownloader(loggerFactory.CreateLogger<ImageDownloader>());
			_filesystem = new ImageReader(loggerFactory.CreateLogger<ImageReader>());
		}

		public async Task<string> Transform(string imageLocation, int width)
		{
			Bitmap bitmap;

			if (await _web.IsValid(imageLocation))
			{
				try
				{
					bitmap = await _web.Get(imageLocation);
				}
				catch (Exception e)
				{
					_logger.LogError($"[{MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
					throw;
				}
			}
			else if (await _filesystem.IsValid(imageLocation))
			{
				try
				{
					bitmap = await _filesystem.Get(imageLocation);
				}
				catch (Exception e)
				{
					_logger.LogError($"[{MethodBase.GetCurrentMethod().Name}] {e.Message ?? ""}", e);
					throw;
				}
			}
			else
			{
				throw new ArgumentException($"The image location, {imageLocation}, is invalid. Must be an image online or on the local machine.");
			}

			return await _transformer.Convert(bitmap, width);
		}
	}
}
