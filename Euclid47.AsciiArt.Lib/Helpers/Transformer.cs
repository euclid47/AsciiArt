using Euclid47.AsciiArt.Lib.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Euclid47.AsciiArt.Lib.Helpers
{
	internal class Transformer : ITransformer
	{
		private static string[] _asciiChars = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", " " };
		private readonly ILogger<Transformer> _logger;

		public Transformer(ILogger<Transformer> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public Task<string> Convert(Bitmap image, int width)
		{
			var toggle = false;
			var result = new StringBuilder();
			Color pixelColor;
			Color grayColor;
			var red = 0;
			var blue = 0;
			var green = 0;

			image = GetReSizedImage(image, width);

			for (int h = 0; h < image.Height; h++)
			{
				for (int w = 0; w < image.Width; w++)
				{
					pixelColor = image.GetPixel(w, h);
					//Average out the RGB components to find the Gray Color
					red = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
					green = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
					blue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
					grayColor = Color.FromArgb(red, green, blue);

					//Use the toggle flag to minimize height-wise stretch
					if (!toggle)
					{
						var index = (grayColor.R * 10) / 255;
						result.Append(_asciiChars[index]);
					}
				}

				if (!toggle)
				{
					result.Append(Environment.NewLine);
					toggle = true;
				}
				else
				{
					toggle = false;
				}
			}


			return Task.FromResult(result.ToString());
		}

		private static Bitmap GetReSizedImage(Bitmap inputBitmap, int asciiWidth)
		{
			var asciiHeight = (int)Math.Ceiling((double)inputBitmap.Height * asciiWidth / inputBitmap.Width);
			var result = new Bitmap(asciiWidth, asciiHeight);

			using(var g = Graphics.FromImage(result))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.DrawImage(inputBitmap, 0, 0, asciiWidth, asciiHeight);
			}

			return result;
		}
	}
}
