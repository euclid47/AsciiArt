using System.Threading.Tasks;

namespace Euclid47.AsciiArt.Lib.Interfaces
{
	public interface IGenerator
	{
		Task<string> Transform(string imageLocation, int width);
	}
}