﻿/*
MIT License

Copyright (c) 2020 Jeff Campbell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Text;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// Helper methods for the Package Manifest Tools
	/// </summary>
	internal static class PackageManifestTools
	{
		private static readonly StringBuilder JSON_STRING_BUILDER
			= new StringBuilder(8192);

		// Package Json Properties
		private const string OPEN_BRACES = "{";
		private const string OPEN_BRACKET = "[";
		private const string CLOSED_BRACES = "}";
		private const string CLOSED_BRACKET = "]";
		private const string COMMA = ",";

		private const string NAME = @"""name"":""{0}""";
		private const string DISPLAY_NAME = @"""displayName"":""{0}""";
		private const string PACKAGE_VERSION = @"""version"":""{0}""";
		private const string UNITY_VERSION = @"""unity"":""{0}""";
		private const string DESCRIPTION = @"""description"":""{0}""";
		private const string KEYWORDS = @"""keywords"":";
		private const string KEYWORD_CHILD = @"""{0}""";
		private const string DEPENDENCIES = @"""dependencies"":";
		private const string DEPENDENCY_CHILD_FORMAT = @"""{0}"":""{1}""";
		private const string CATEGORY = @"""category"":""{0}""";
		private const string AUTHOR = @"""author"":{{""name"":""{0}"",""mail"":""{1}"",""url"":""{2}""}}";
		private const string SAMPLES = @"""samples"":[{{""displayName"":""{0}"",""description"":""{1}"",""path"":""{2}""}}]";

		/// <summary>
		/// Returns a Json <see cref="string"/> representation of the <see cref="PackageManifestConfig"/>
		/// <paramref name="packageManifest"/>.
		/// </summary>
		/// <param name="packageManifest"></param>
		/// <returns></returns>
		public static string GenerateJson(PackageManifestConfig packageManifest)
		{
			// Clear string builder
			JSON_STRING_BUILDER.Length = 0;

			JSON_STRING_BUILDER.Append(OPEN_BRACES);
			JSON_STRING_BUILDER.AppendFormat(NAME, packageManifest.packageName);
			JSON_STRING_BUILDER.Append(COMMA);
			JSON_STRING_BUILDER.AppendFormat(DISPLAY_NAME, packageManifest.displayName);
			JSON_STRING_BUILDER.Append(COMMA);
			JSON_STRING_BUILDER.AppendFormat(PACKAGE_VERSION, packageManifest.packageVersion);
			JSON_STRING_BUILDER.Append(COMMA);
			JSON_STRING_BUILDER.AppendFormat(UNITY_VERSION, packageManifest.unityVersion);
			JSON_STRING_BUILDER.Append(COMMA);
			JSON_STRING_BUILDER.AppendFormat(DESCRIPTION, packageManifest.description);
			JSON_STRING_BUILDER.Append(COMMA);

			// Add the keywords if any exist.
			if (packageManifest.keywords != null &&
				packageManifest.keywords.Length > 0)
			{
				JSON_STRING_BUILDER.Append(KEYWORDS);
				JSON_STRING_BUILDER.Append(OPEN_BRACKET);

				for (var i = 0; i < packageManifest.keywords.Length; i++)
				{
					var keyword = packageManifest.keywords[i];

					JSON_STRING_BUILDER.AppendFormat(KEYWORD_CHILD, keyword);

					if (i != packageManifest.keywords.Length - 1)
					{
						JSON_STRING_BUILDER.Append(COMMA);
					}
				}

				JSON_STRING_BUILDER.Append(CLOSED_BRACKET);
				JSON_STRING_BUILDER.Append(COMMA);
			}

			JSON_STRING_BUILDER.AppendFormat(CATEGORY, packageManifest.category);
			JSON_STRING_BUILDER.Append(COMMA);

			JSON_STRING_BUILDER.AppendFormat(AUTHOR, packageManifest.authorName, packageManifest.authorMail, packageManifest.authorURL);

			if (!packageManifest.samplesSourcePath.Equals(""))
			{
				JSON_STRING_BUILDER.Append(COMMA);
				JSON_STRING_BUILDER.AppendFormat(SAMPLES, "Sample", $"Set of use examples for {packageManifest.displayName}", "Samples~");
			}

			// Add the dependencies block if any exist.
			if (packageManifest.dependencies != null &&
				packageManifest.dependencies.Length > 0)
			{
				JSON_STRING_BUILDER.Append(COMMA);
				JSON_STRING_BUILDER.Append(DEPENDENCIES);
				JSON_STRING_BUILDER.Append(OPEN_BRACES);

				for (var i = 0; i < packageManifest.dependencies.Length; i++)
				{
					var dependency = packageManifest.dependencies[i];
					if (string.IsNullOrEmpty(dependency.packageName) ||
						string.IsNullOrEmpty(dependency.packageVersion))
					{
						continue;
					}

					JSON_STRING_BUILDER.AppendFormat(
						DEPENDENCY_CHILD_FORMAT,
						dependency.packageName,
						dependency.packageVersion);

					if (i != packageManifest.dependencies.Length - 1)
					{
						JSON_STRING_BUILDER.Append(COMMA);
					}
				}

				JSON_STRING_BUILDER.Append(CLOSED_BRACES);
			}

			JSON_STRING_BUILDER.Append(CLOSED_BRACES);

			return JSON_STRING_BUILDER.ToString();
		}
	}
}
