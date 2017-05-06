using System;
using System.Linq;
namespace SiteMapGenerator
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var url = @"";
			var Sm = new SiteMap(url);

			Sm.Generate();

#if DEBUG
			Logger.Log("Flattened pages:");
			foreach (var actUrl in Sm.GetAllPages().Select(p => p.Url))
			{
				Logger.Log("Url: " + actUrl);
			}
#endif

			var rt = Sm.GetAllPages()
			           .Take(11)
			           .Select(p => new PageLoadingTimes(p))
			           .ToList();

			foreach (var plt in rt)
			{
				plt.TestLoadingTime(3);
			}
			//var page = new Page(url);

			//foreach (var u in page.GetChildUrls())
			//{
			//	Console.WriteLine(u);
			//}
			Console.ReadLine();
		}
	}
}
