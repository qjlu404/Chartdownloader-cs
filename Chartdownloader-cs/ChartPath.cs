using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Chartdownloader_cs
{
	public class ChartPath
	{
		public enum Mode
		{
			PDF,
			PNG
		}
		public readonly Mode mode;

		public readonly string MacroPath;
		public readonly string FinalPath;

		public ChartPath(ChartPath pathObj)
		{
			this.MacroPath = pathObj.MacroPath;
			this.FinalPath = pathObj.FinalPath;
		}
		public ChartPath(ChartData chart, Mode mode, string path)
		{
			this.mode = mode;
			MacroPath = path;
			if (mode == Mode.PDF)
			{
				string TypePath = "";
				switch (chart.Type) 
				{
				case "APP.":
					TypePath = "Approach Plates";
					break;

				case "ARR.":
					TypePath = "STARs";
					break;

				case "DEP.":
					TypePath = "SIDs";
					break;

				default:
					TypePath = "Information";
					break;
				}
				FinalPath = string.Format("{0}/{1}/{2}/", MacroPath, chart.Ident,TypePath); 
			}
			if (mode == Mode.PNG)
			{
				FinalPath = string.Format("0/1/", MacroPath, chart.Ident);
			}
		}
	}
}
