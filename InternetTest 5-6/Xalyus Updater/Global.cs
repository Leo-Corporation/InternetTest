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
using System;

namespace Xalyus_Updater
{
	/// <summary>
	/// A class that contains informations about the update status.
	/// </summary>
	public static class Global
	{
		/// <summary>
		/// The progress of the update installation.
		/// </summary>
		public static int UpdateProgress { get; set; }

		/// <summary>
		/// The link of the file to be updated.
		/// </summary>
		public static string ZIPLink { get; set; }

		/// <summary>
		/// The directory where the file is downloaded.
		/// </summary>
		public static string Directory => AppDomain.CurrentDomain.BaseDirectory + @"\UpdatedInternetTestFiles.zip";

		/// <summary>
		/// The displayed message when the update is installed.
		/// </summary>
		public static string InstallMessage { get; set; }
	}
}
