using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteMapGenerator
{
	public class SiteMap
	{
		Page _mainPage;



		public SiteMap(string url)
		{
			this._mainPage = new Page(url);
		}

		public List<Page> GetAllPages()
		{
			return this._mainPage.Flatten();
		}

		public void Generate()
		{
			FillSubpages(_mainPage);
		}

		public void FillSubpages(Page page)
		{
#if DEBUG
			Logger.Log("Page: " + page.Url);
#endif
			foreach (var url in page.GetFullChildUrls()
										.Where(PageHelper.IsPageUrl)
										.Distinct()
			         .ToList())
			{
#if DEBUG

				Logger.Log("checking url: " + url);
#endif
				if (!UrlIsValid(url))
				{
#if DEBUG
					Logger.Log("Url invalid");
#endif
					continue;
				}

#if DEBUG
				Logger.Log("Url is valid");
#endif
				var pageToAdd = new Page(url);


				if (!ContainsPage(pageToAdd))
				{
					page.AddChildPage(pageToAdd);
				}
			}

			foreach (var subpage in page.ChildPages)
			{
				FillSubpages(subpage);
			}
		}

		bool UrlIsValid(string url)
		{
			if (!PageHelper.UrlIsValid(url))
			{
				return false;
			}

			if (!url.StartsWith(this._mainPage.Url, StringComparison.Ordinal))
			{
				return false;
			}

			return true;
		}

		bool ContainsPage(Page page)
		{
			var ret = _mainPage.Flatten().Any(p => p.Url.Equals(page.Url, StringComparison.InvariantCultureIgnoreCase));

#if DEBUG
			Logger.Log("Url contained " + page.Url + " in whole system: " + ret.ToString());
#endif
			return ret;
		}
	}
}
