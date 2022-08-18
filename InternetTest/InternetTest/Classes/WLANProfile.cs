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
using System.Xml.Serialization;

namespace InternetTest.Classes;
[XmlRoot(ElementName = "SSID")]
public class SSID
{
	[XmlElement(ElementName = "hex")]
	public string? Hex { get; set; }

	[XmlElement(ElementName = "name")]
	public string? Name { get; set; }
}

[XmlRoot(ElementName = "SSIDConfig")]
public class SSIDConfig
{
	[XmlElement(ElementName = "SSID")]
	public SSID? SSID { get; set; }
}

[XmlRoot(ElementName = "authEncryption")]
public class AuthEncryption
{
	[XmlElement(ElementName = "authentication")]
	public string? Authentication { get; set; }

	[XmlElement(ElementName = "encryption")]
	public string? Encryption { get; set; }

	[XmlElement(ElementName = "useOneX")]
	public bool UseOneX { get; set; }
}

[XmlRoot(ElementName = "sharedKey")]
public class SharedKey
{
	[XmlElement(ElementName = "keyType")]
	public string? KeyType { get; set; }

	[XmlElement(ElementName = "protected")]
	public bool Protected { get; set; }

	[XmlElement(ElementName = "keyMaterial")]
	public string? KeyMaterial { get; set; }
}

[XmlRoot(ElementName = "security")]
public class Security
{
	[XmlElement(ElementName = "authEncryption")]
	public AuthEncryption? AuthEncryption { get; set; }

	[XmlElement(ElementName = "sharedKey")]
	public SharedKey? SharedKey { get; set; }
}

[XmlRoot(ElementName = "MSM")]
public class MSM
{

	[XmlElement(ElementName = "security")]
	public Security? Security { get; set; }
}

[Serializable, XmlRoot(ElementName = "WLANProfile", Namespace = "http://www.microsoft.com/networking/WLAN/profile/v1")]
[XmlType("WLANProfile")]
public class WLANProfile
{
	[XmlElement(ElementName = "name")]
	public string? Name { get; set; }

	[XmlElement(ElementName = "SSIDConfig")]
	public SSIDConfig? SSIDConfig { get; set; }

	[XmlElement(ElementName = "connectionType")]
	public string? ConnectionType { get; set; }

	[XmlElement(ElementName = "connectionMode")]
	public string? ConnectionMode { get; set; }

	[XmlElement(ElementName = "MSM")]
	public MSM? MSM { get; set; }

	public override string ToString() => $"{Properties.Resources.Authentication}: {MSM?.Security?.AuthEncryption?.Authentication}\n" +
		$"{Properties.Resources.Key}: {MSM?.Security?.SharedKey?.KeyMaterial}\n" +
		$"{Properties.Resources.Encryption}: {MSM?.Security?.AuthEncryption?.Encryption}\n" +
		$"{Properties.Resources.ConnectionMode}: {(ConnectionMode == "auto" ? Properties.Resources.Automatic : ConnectionMode)}\n" +
		$"{Properties.Resources.ConnectionType}: {ConnectionType switch { "ESS" => Properties.Resources.InfrastructureNetwork, "IBSS" => Properties.Resources.AdHocNetwork, _ => ConnectionType }}";
}
