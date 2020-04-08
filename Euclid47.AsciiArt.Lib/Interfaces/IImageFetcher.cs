using System.Drawing;
using System.Threading.Tasks;

namespace Euclid47.AsciiArt.Lib.Interfaces
{
	internal interface IImageFetcher
	{
		Task<Bitmap> Get(string imageLocation);
		Task<bool> IsValid(string imageLocation);
	}
}