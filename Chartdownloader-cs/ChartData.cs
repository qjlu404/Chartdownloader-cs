using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chartdownloader_cs
{
	public class ChartData
	{
		public string OverallNumber { get; set; }
		public string Title { get; set; }
		public string Directors { get; set; }
		public string WrittenBy { get; set; }
		public string Released { get; set; }

		public string GetStringList()
		{
			return OverallNumber + "\n" + Title + "\n" + Directors
							  + "\n" + WrittenBy + "\n" + Released + "\n";
		}
	}
}
