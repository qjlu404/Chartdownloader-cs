using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Chartdownloader_cs 
{

	public class Chartdownloader 
	{
		// TODO: include meta that indicates the cycle number, date downloaded
		private string _Cycle;
		private string _Ident;
		private string _Url;
		private int _PageNumber;

		private HtmlWeb web;
		private HtmlDocument document;
		private List<ChartData> Charts;
		HtmlNodeCollection nodes;

		HttpClient client;
		public Chartdownloader()
		{
			var todaysDate = DateTime.Now;
			todaysDate = todaysDate.Subtract(TimeSpan.FromDays(15));
			_Cycle = todaysDate.ToString("yyMM");
			_Ident = "khou";
			_PageNumber = 1;
			_Url = string.Format("https://www.faa.gov/air_traffic/flight_info/aeronav/digital_products/dtpp/search/results/?cycle={0}&ident={1}", _Cycle, _Ident);

			web = new HtmlWeb();
			document = web.Load(_Url);
			Charts = new();
			client = new();
			nodes = document.DocumentNode.SelectNodes("//*[@id='resultsTable']/tbody/tr");
		}
		// Uses pagination to detect page count
		public int NextPage() // some functions 
		{
			try
			{
				var test = HtmlEntity.DeEntitize(document.DocumentNode.SelectSingleNode("//*[@id='content']/ul/*[last()]/a").Attributes["href"].Value);
				Console.WriteLine("");
			}
			catch (Exception)
			{
				return 0;
			}
			_PageNumber++;
			_Url = string.Format("https://www.faa.gov/air_traffic/flight_info/aeronav/digital_products/dtpp/search/results/?cycle={0}&ident={1}&page={2}", _Cycle, _Ident, _PageNumber);
			// spent a few days trying to figure out why it's only loading the first page
			// turns out you actually have to re download the page and parse it again
			// (mind blown like it wasnt blown already)
			document = web.Load(_Url); // all it took was this line of code to fix it all

			nodes = document.DocumentNode.SelectNodes("//*[@id='resultsTable']/tbody/tr");
			return _PageNumber; // else
		}

		public async Task<List<ChartData>> ScrapeChartData()
		{
			List<Task> tasks = new List<Task>();

			foreach (var node in nodes)
			{

				// class ChartData {public Name; public Type; public Link; public Ident}
				ChartData chart = new ChartData()
				{
					Ident = HtmlEntity.DeEntitize(node.SelectSingleNode("td[4]").InnerText), //TODO: source Ident from webpage ("td[4]") all characters before the space
					Name = HtmlEntity.DeEntitize(node.SelectSingleNode("td[8]//a").InnerText),
					Type = HtmlEntity.DeEntitize(node.SelectSingleNode("td[last()-2]").InnerText),
					Link = HtmlEntity.DeEntitize(node.SelectSingleNode("td[8]//a").Attributes["href"].Value)
				};
				ChartPath chartPath = new ChartPath(chart, ChartPath.Mode.PDF, "./Charts");
				// TODO: ChartPath(Mode, path)
				// GetPath(chart);
				Console.WriteLine("Url: "  + _Url);
				Console.WriteLine("Ident: "+ chart.Ident);
				Console.WriteLine("Name: " + chart.Name);
				Console.WriteLine("Type: " + chart.Type);
				Console.WriteLine("Link: " + chart.Link);
				Console.WriteLine("\n");

				tasks.Add(DownloadPDF(chart, chartPath));
				Charts.Add(chart);
			}
			await Task.WhenAll(tasks);
			Console.WriteLine("Found {0} links", nodes.Count);
			return Charts;
		}

		public async Task DownloadPDF(ChartData Chart, ChartPath path)
		{
			// yes i know its obsolete buts its easy asf to use
			// Ill use HTTPClient when I need more control over the downloading processtry
			try
			{
				var uri = new Uri(Chart.Link);
				HttpClient client = new HttpClient();
				var response = await client.GetAsync(uri);
				using (var fs = new FileStream(path.FinalPath + Chart.Name + ".pdf", FileMode.CreateNew))
				{
					await response.Content.CopyToAsync(fs);
				}
			}
			catch (IOException Ex)
			{
				// should skip
			}
			catch (AggregateException Ex)
			{
				return;
			}
			/*
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				Console.WriteLine(ex.InnerException);
				Console.WriteLine(ex.StackTrace);
				return;
			}*/
		}
			
	}
}
