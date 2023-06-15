// See https://aka.ms/new-console-template for more information
using HtmlAgilityPack;
using System.ComponentModel.Design;
using System.Linq.Expressions;

namespace Chartdownloader_cs
{
	class Program
	{
		public static void Main() 
		{
			try
			{
				MainAsync().Wait();
			}
			catch (AggregateException)
			{
				return;
			}
		}
		public static async Task MainAsync()
		{
			var cdObj = new Chartdownloader();
			Task<List<ChartData>> task1 = cdObj.ScrapeChartData();
			while (cdObj.NextPage() != 0)
			{
				Task<List<ChartData>> taskwhile = cdObj.ScrapeChartData();
				await Task.WhenAll(taskwhile);
			}
			await Task.WhenAll(task1);
		}
	}
}
