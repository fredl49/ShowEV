using System.Collections;

string[] strings = args;
if (strings.Length == 0) {
	strings = new string[1];
	strings[0] = "PATH";
}

IDictionary variables = Environment.GetEnvironmentVariables();
List<string> lines = new();

bool needNewLine = false;

foreach (string s in strings) {
	string name = s.ToUpper();
	bool found = false;

	foreach (DictionaryEntry entry in variables) {
		string entryKey = (string)entry.Key;
		if (entryKey.ToUpper().StartsWith(name)) {
			found = true;

			string value = (string)entry.Value;

			if (needNewLine)
				lines.Add("");
			else
				needNewLine = true;

			if (value is null) {
				lines.Add($"{entryKey}{Environment.NewLine}  (null)");

				continue;
			}

			lines.Add($"{entryKey}");

			string[] values = value.Split(';');
			foreach (string v in values) {
				lines.Add($"  {v}");
			}
		}
	}

	if (!found) {
		lines.Add($"No entries starting with {name}");
	}
}

WriteInPagedGroups(lines, Console.WindowHeight - 1);

static void WriteInPagedGroups(List<string> lines, int linesPerPage) {
	int currentLine = 0;
	foreach (string line in lines) {
		Console.WriteLine(line);
		if (++currentLine == linesPerPage - 1) {
			Console.Write("Press any key to continue . . . ");
			ConsoleKeyInfo keyInfo = Console.ReadKey();

			if (keyInfo.Key != ConsoleKey.Enter)
				Console.Write(Environment.NewLine);

			currentLine = 0;
		}
	}
}