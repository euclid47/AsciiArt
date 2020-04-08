using System.Drawing;
using System.Threading.Tasks;

namespace Euclid47.AsciiArt.Lib.Interfaces
{
	internal interface ITransformer
	{
		Task<string> Convert(Bitmap image, int width);
	}
}