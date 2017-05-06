using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace SiteMapGenerator
{
	public class Helper
	{
		static List<string> stuffExtensions = new List<string> { "jpg", "jpeg", "png", "gif" };

		public static List<string> StuffExtensions
		{
			get
			{
				return stuffExtensions;
			}
		}

		public static String GetContentFromUrl(string url)
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

		public static IEnumerable<string> GetStuffAddresses(string content)
		{
			string pattern = "<a\\s+(?:[^>]*?\\s+)?href=([\"'])(.*?)\\1";

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

			return ret.Where(u => !u.StartsWith("#", StringComparison.Ordinal))
							 .Select(RemoveCarret);
		}

public static IEnumerable<string> GetImageUrls(string content)
{
	string pattern = "<img\\s+(?:[^>]*?\\s+)?src=([\"'])(.*?)\\1";

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

	return ret.Where(u => !u.StartsWith("#", StringComparison.Ordinal))
					 .Select(RemoveCarret);
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

		static string RemoveCarret(string arg)
		{
			if (arg.Contains("#"))
			{
				return arg.Substring(0, arg.IndexOf("#", StringComparison.Ordinal));
			}
			return arg;
		}
	}
}
