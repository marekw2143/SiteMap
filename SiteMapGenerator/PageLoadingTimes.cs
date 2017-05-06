using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteMapGenerator
{
	public class PageLoadingTimes
	{
		Page _page;

		public string Url
		{
			get
			{
				return _page.Url;
			}
		}

		public List<double> runningTimes;

		public PageLoadingTimes(Page page)
		{
			this._page = page;
			runningTimes = new List<double>();
		}

		public void TestLoadingTime(int numberOfTries)
		{
			for (int i = 0; i < numberOfTries; i++)
			{
				var startTime = DateTime.Now;

				PageHelper.GetPageConent(Url);

				foreach (var childUrl in _page.GetFullChildUrls(false)
							  .Where(PageHelper.IsStuffUrl)
							 .Distinct()
						.ToList())
				{
					PageHelper.GetPageConent(Url);
				}

				var endTime = DateTime.Now;

				var runningTime = endTime.Subtract(startTime).TotalSeconds;

				runningTimes.Add(runningTime);
			}

		}
	}
}

