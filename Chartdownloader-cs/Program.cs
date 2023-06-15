// See https://aka.ms/new-console-template for more information
using HtmlAgilityPack;

namespace Chartdownloader_cs
{
	class Program
	{
		public static void Main(string[] args) 
		{
			string url = "https://en.wikipedia.org/wiki/List_of_SpongeBob_SquarePants_episodes";
			var web = new HtmlWeb();
			// downloading to the target page
			// and parsing its HTML content
			var document = web.Load(url);
			var nodes = document.DocumentNode.SelectNodes("//*[@id='mw-content-text']/div[1]/table[position()>1 and position()<15]/tbody/tr[position()>1]");
			
			List<ChartData> episodes = new List<ChartData>();
			// looping over the nodes 
			// and extract data from them
			foreach (var node in nodes)
			{
				// add a new Episode instance to 
				// to the list of scraped data
				episodes.Add(new ChartData()
				{
					OverallNumber = HtmlEntity.DeEntitize(node.SelectSingleNode("th[1]").InnerText),
					Title = HtmlEntity.DeEntitize(node.SelectSingleNode("td[2]").InnerText),
					Directors = HtmlEntity.DeEntitize(node.SelectSingleNode("td[3]").InnerText),
					WrittenBy = HtmlEntity.DeEntitize(node.SelectSingleNode("td[4]").InnerText),
					Released = HtmlEntity.DeEntitize(node.SelectSingleNode("td[5]").InnerText)
				});
			}

			//Console.WriteLine(episodes[0].GetStringList());
		}
	}
}
