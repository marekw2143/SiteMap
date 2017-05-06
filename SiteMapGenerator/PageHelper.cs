using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SiteMapGenerator
{
	public class PageHelper
	{
		static List<string> stuffExtensions = new List<string> { "jpg", "jpeg", "png", "gif" };

		public static List<string> StuffExtensions
		{
			get
			{
				return stuffExtensions;
			}
		}

		public static String GetPageConent(string url)
		{
			try
			{
				var webRequest = WebRequest.Create(url);

				using (var response = webRequest.GetResponse())
				{
					using (var content = response.GetResponseStream())
					{
						using (var reader = new StreamReader(content))
						{
							return reader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex)
			{
				return String.Empty;
			}
		}

		public static List<string> GetUrls(string content, bool aOnly = true)
		{
			string pattern = "<" + (aOnly ? "a" : "img") + "\\s+(?:[^>]*?\\s+)?href=([\"'])(.*?)\\1";

			List<string> ret = new List<string>();
			foreach (Match m in Regex.Matches(content, pattern))
			{
				ret.Add(m.Groups[2].Value);
			}
#if DEBUG
			Logger.Log("urls:");
			foreach (var url in ret)
			{
				Logger.Log("\t " + url);
			}
#endif

			return ret;
		}

		internal static bool UrlIsValid(string url)
		{
			return true;
		}

		public static bool IsStuffUrl(string url)
		{
			return !IsPageUrl(url);
		}

		internal static bool IsPageUrl(string url)
		{
			foreach (var ext in StuffExtensions)
			{
				if (url.EndsWith(ext, StringComparison.Ordinal))
				{
					return false;
				}
			}

			return true;
		}
	}
}
