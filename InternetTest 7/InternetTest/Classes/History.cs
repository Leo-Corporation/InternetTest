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
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InternetTest.Classes;

public class History
{
	public List<StatusHistory> StatusHistory { get; set; }
	public List<DownHistory> DownDetectorHistory { get; set; }
	public History()
	{
		StatusHistory = [];
		DownDetectorHistory = [];
	}
}

public class HistoryItem
{
	public int Date { get; set; }
	public string Icon { get; set; }
	public HistoryItem(int date, string icon)
	{
		Date = date;
		Icon = icon;
	}
}

public class StatusHistory : HistoryItem
{
	public bool Status { get; set; }

	[JsonConstructor]
	public StatusHistory() : base(0, "")
	{
	}

	public StatusHistory(int date, string icon, bool sucessful) : base(date, icon)
	{
		Status = sucessful;
	}
}

public class DownHistory : HistoryItem
{
	public int StatusCode { get; set; }
	public string? StatusText { get; set; }
	public string? Website { get; set; }

	[JsonConstructor]
	public DownHistory() : base(0, "")
	{
	}

	public DownHistory(int date, string icon, int statusCode, string msg, string website) : base(date, icon)
	{
		StatusCode = statusCode;
		StatusText = msg;
		Website = website;
	}
}