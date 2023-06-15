using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chartdownloader_cs
{
	public class ChartData
	{
		enum ChartType
		{
			ARR,
			APP,
			DEP,
			INF
		}
		private ChartType _type;
		public string Type 
		{
			get 
			{ 
				switch (_type)
				{
					case ChartType.ARR:
						return "ARR.";
					case ChartType.APP:
						return "APP.";
					case ChartType.DEP:
						return "DEP.";
					case ChartType.INF:
						return "INF.";
					default:
						return "INF,";
				}
			}
			set 
			{
				switch (value) 
				{
					case "STAR":
						_type = ChartType.ARR;
						break;

					case "IAP":
						_type = ChartType.APP;
						break;

					case "DP":
						_type = ChartType.DEP;
						break;
					default:
						_type = ChartType.INF;
						break;
				}
			}
		}

		private string _name;
		public string Name 
		{
			get 
			{
				if (_name == null)
				{
					return " ";
				}
				else
				{
					return _name;
				}
			}
			set 
			{
				_name = value.Replace('/', '-').Replace(", CONT.", " Pg-");
				
			}
			
		}
		private string _link;
		public string Link 
		{ 
			get 
			{
				return _link;
			}
			set
			{
				_link = value;
			}
		}
		private string _ident;
		public string Ident 
		{
			get
			{
				return _ident;
			}
			set
			{
				var debug = value;
				try
				{
					_ident = value.Remove(value.IndexOf(" "));
				}
				catch (ArgumentOutOfRangeException)
				{
					
				}
				var point = 2 + 2;
			}
		}

		public ChartData()
		{
			Type  = "";
			Name  = "";
			_name = "";
			Link  = "";
			_ident = "";
			Ident = "";
		}
	}
}
