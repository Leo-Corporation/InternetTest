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
using InternetTest.Helpers;
using System.IO;
using System.Xml.Serialization;

namespace InternetTest.Models;

public class ActivityHistory
{
	public List<Activity> Activity { get; set; }

	public ActivityHistory()
	{
		Activity = [];
	}

	public static async Task<ActivityHistory> LoadAsync()
	{
		// Load from XML File
		var filePath = $@"{Context.DefaultStoragePath}\Activity.xml";
		if (File.Exists(filePath))
		{
			return await XmlHelper.DeserializeAsync<ActivityHistory>(filePath) ?? new ActivityHistory();
		}
		else
		{
			var history = new ActivityHistory();
			history.Save();
			return history;
		}
	}

	public void Save()
	{
		var filePath = $@"{Context.DefaultStoragePath}\Activity.xml";

		XmlSerializer xmlSerializer = new(typeof(ActivityHistory));
		StreamWriter streamWriter = new(filePath);
		xmlSerializer.Serialize(streamWriter, this);

		streamWriter.Close();
		streamWriter.Dispose();
	}
}

public class Activity
{
	public string Name { get; init; }
	public string Result { get; init; }
	public bool? Success { get; init; }
	public DateTime Date { get; init; }

	public Activity(string name, string result, bool? success, DateTime date)
	{
		Name = name;
		Result = result;
		Success = success;
		Date = date;
	}

	public Activity()
	{
		Name = string.Empty;
		Result = string.Empty;
		Success = null;
		Date = DateTime.MinValue;
	}
}
