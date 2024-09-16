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

using System.Text.Json.Serialization;
using System.Text.Json;

namespace InternetTest.Classes;

public partial class DnsCacheInfo
{
	[JsonPropertyName("TTL")]
	public long Ttl { get; set; }

	[JsonPropertyName("Caption")]
	public object Caption { get; set; }

	[JsonPropertyName("Description")]
	public object Description { get; set; }

	[JsonPropertyName("ElementName")]
	public string ElementName { get; set; }

	[JsonPropertyName("InstanceID")]
	public object InstanceId { get; set; }

	[JsonPropertyName("Data")]
	public string Data { get; set; }

	[JsonPropertyName("DataLength")]
	public long DataLength { get; set; }

	[JsonPropertyName("Entry")]
	public string Entry { get; set; }

	[JsonPropertyName("Name")]
	public string Name { get; set; }

	[JsonPropertyName("Section")]
	public long Section { get; set; }

	[JsonPropertyName("Status")]
	public long Status { get; set; }

	[JsonPropertyName("TimeToLive")]
	public long TimeToLive { get; set; }

	[JsonPropertyName("Type")]
	public long Type { get; set; }

	[JsonPropertyName("PSComputerName")]
	public object PsComputerName { get; set; }
}

public partial class DnsCacheInfo
{
	public static DnsCacheInfo[] FromJson(string json) => JsonSerializer.Deserialize<DnsCacheInfo[]>(json);
}