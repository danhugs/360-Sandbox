public static class UtilitiesString {

	/// <summary>
	/// Converts sentence or string to CamelCase
	/// </summary>
	public static string ToCamelCase(this string input) {
		if (input == "") {
			return "";
		}
		if (input.IndexOf(" ") == 0) {
			input = input.Substring(1);
		}

		string[] words = input.Split(' ');
		System.Text.StringBuilder sb = new System.Text.StringBuilder();

		for (int i = 0; i < words.Length; i++) {
			string s = words[i];

			if (s.Length > 0) {
				string firstLetter = s.Substring(0, 1);
				string rest = s.Substring(1, s.Length - 1);
				if (i == 0) {
					sb.Append(firstLetter + rest);//DON'T MODIFY FIRST CHARACTER
				} else {
					sb.Append(firstLetter.ToUpper() + rest);
				}
				sb.Append(" ");
			}
		}

		return (sb.ToString().Substring(0, sb.ToString().Length - 1)).Replace(" ", "");
	}

	public static string ToPascalCase(this string input) {
		if (input.Length > 0) {
			string camelCase = input.ToCamelCase();
			return camelCase.Substring(0, 1).ToUpper() + camelCase.Substring(1);
		}
		return "";
	}

	/// <summary>
	/// Strips numbgers out of the string
	/// </summary>
	public static string OnlyAlphabetical(this string input) {
		return System.Text.RegularExpressions.Regex.Replace(input, "[0-9]*", string.Empty);
	}

	public static string FormatVariables(string template, params System.Object[] items) {
		string copy = template;

		for (int i = 0; i < items.Length; i++) {
			copy = copy.Replace("{" + i + "}", items[i].ToString());
		}

		return copy;
	}
}