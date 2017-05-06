using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteMapGenerator
{
	public class Page
	{
		public string Url { get; set; }

		List<int> _loadingTimes;
		List<Page> _childPages;

		string _content;


		public string Content
		{
			get
			{
				if (_content == null)
				{
					_content = GetContent();
				}

				return _content;
			}
		}

		string GetContent()
		{
			return PageHelper.GetPageConent(Url);
		}

		public List<Page> ChildPages
		{
			get
			{
				if (_childPages == null)
				{
					_childPages = new List<Page>();
				}

				return _childPages;
			}
		}

		public List<int> LoadingTimes
		{
			get
			{
				return _loadingTimes;
			}
		}

		public List<Page> Flatten()
		{

			var flattenedSiteMap = new List<Page>();


			flattenedSiteMap.Add(this);

			foreach(var page in ChildPages)
			{

				flattenedSiteMap.AddRange(page.Flatten());
			}

			return flattenedSiteMap;
		}

		internal void AddChildPage(Page pageToAdd)
		{
			this._childPages.Add(pageToAdd);
		}

		public List<string> GetFullChildUrls()
		{
			return PageHelper.GetStuffAddresses(Content)				             
							 .Select(CombineUrl)
							 .Where(PageHelper.IsPageUrl)
							 .Distinct()
							 .ToList();
		}

		public List<string> GetImageUrls()
		{
			return PageHelper.GetImageUrls(Content)
							 .Select(CombineUrl)
				             .Where(PageHelper.IsStuffUrl)
							 .Distinct()
							 .ToList();
		}


		public Page(string url)
		{
			this.Url = url;
			_childPages = new List<Page>();
			_loadingTimes = new List<int>();
		}


		string CombineUrl(string url)
		{
			if (url == "/")
			{
				return string.Empty;
			}
			if (url.StartsWith("http://", StringComparison.Ordinal) || url.StartsWith("https://", StringComparison.Ordinal))// || url.StartsWith("www.", StringComparison.Ordinal))
			{
				return url;
			}

			var slashIdx = 0;
			var prefixLength = 0;
			if (this.Url.StartsWith("http://", StringComparison.Ordinal))
			{
				slashIdx = this.Url.Substring(7).IndexOf("/", StringComparison.Ordinal);
				prefixLength = "http://".Length;
			}
			else
			{
				slashIdx = this.Url.Substring(8).IndexOf("/", StringComparison.Ordinal);
				prefixLength = "https://".Length;
			}

			if (slashIdx > 0)
			{
				slashIdx += prefixLength;
				return this.Url.Substring(0, slashIdx) + "/" + url;
			}

			return this.Url + "/" + url;
		}
	}
}
