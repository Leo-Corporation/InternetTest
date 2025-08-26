/*
MIT License

Copyright (c) Léo Corporation

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
using InternetTest.Enums;
using PeyrSharp.Env;
using System.Diagnostics;

namespace InternetTest.Helpers;

public static class Context
{
	public static string Version => "9.0.0.2508-rc1";

#if PORTABLE
	public static string DefaultStoragePath => $@"{FileSys.CurrentDirectory}\InternetTest Pro\";
#else
	public static string DefaultStoragePath => $@"{FileSys.AppDataPath}\Léo Corporation\InternetTest Pro\";
#endif
	public static string UpdateVersionUrl => "https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/InternetTest/9.0/Version.txt";

	public static void ChangeLanguage(Language language)
	{
		var ci = language switch
		{
			Language.en_US => new System.Globalization.CultureInfo("en-US"),
			Language.fr_FR => new System.Globalization.CultureInfo("fr-FR"),
			Language.zh_CN => new System.Globalization.CultureInfo("zh-CN"),
			Language.it_IT => new System.Globalization.CultureInfo("it-IT"),
			_ => Thread.CurrentThread.CurrentUICulture
		};
		Thread.CurrentThread.CurrentUICulture = ci;
		Thread.CurrentThread.CurrentCulture = ci;
		Properties.Resources.Culture = ci;
	}


	public static async Task<string> RunPowerShellCommandAsync(string psCommand)
	{
		// Create a new process to run PowerShell
		ProcessStartInfo processInfo = new()
		{
			FileName = "powershell.exe",
			Arguments = $"-Command \"{psCommand}\"",
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			UseShellExecute = false,
			CreateNoWindow = true
		};

		// Start the PowerShell process
		using Process? process = Process.Start(processInfo);

		// Asynchronously read the output from the process
		if (process == null) return string.Empty;
		string output = await process.StandardOutput.ReadToEndAsync();

		// Wait for the process to complete asynchronously
		await process.WaitForExitAsync();

		// Return the JSON output
		return output;
	}
}
