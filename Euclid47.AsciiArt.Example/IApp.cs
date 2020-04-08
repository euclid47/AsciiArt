using System.Threading.Tasks;

namespace Euclid47.AsciiArt.Example
{
	internal interface IApp
	{
		Task Run(string imageLocation);
	}
}