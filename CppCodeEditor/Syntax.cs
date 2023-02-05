using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace CppCodeEditor
{
	public class KeywordInfo
	{
		public List<string> keywords { get; set; }
		public string color { get; set; }
	}
	public class SpecialsInfo
	{
		public List<string> specials { get; set; }
		public string color { get; set; }
	}
	public class Wrapper
	{
		public string start { get; set; }
		public string end { get; set; }
		public string color { get; set; }
	}
	public class SyntaxContainer
	{
		public string textColor { get; set; }
		public KeywordInfo keywordInfo { get; set; }
		public SpecialsInfo specialsInfo { get; set; }
		public Dictionary<string, Wrapper> wrappers { get; set; }
	}
	public static class Syntax
	{
		public static String syntaxConfigPath = @"..\..\syntaxConfig.json";
		public static SyntaxContainer syntax = new SyntaxContainer();
		public static void loadSyntax()
		{
			String syntaxJson = "";
			using (StreamReader sr = File.OpenText(syntaxConfigPath))
			{
				syntaxJson = sr.ReadToEnd();
			}
			syntax = JsonSerializer.Deserialize<SyntaxContainer>(syntaxJson);
		}
	}
}
