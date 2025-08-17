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
using PeyrSharp.Env;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace InternetTest.Models;
[XmlRoot(ElementName = "SSID")]
public class Ssid
{
	[XmlElement(ElementName = "hex")]
	public string? Hex { get; set; }

	[XmlElement(ElementName = "name")]
	public string? Name { get; set; }
}

[XmlRoot(ElementName = "SSIDConfig")]
public class SsidConfig
{
	[XmlElement(ElementName = "SSID")]
	public Ssid? SSID { get; set; }
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
public class WlanProfile
{
	[XmlElement(ElementName = "name")]
	public string? Name { get; set; }

	[XmlElement(ElementName = "SSIDConfig")]
	public SsidConfig? SSIDConfig { get; set; }

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

	public static async Task<List<WlanProfile>> GetProfilesAsync()
	{
		try
		{
			// Check if the temp directory exists
			string path = FileSys.AppDataPath + @"\Léo Corporation\InternetTest Pro\WiFis";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			// Run "netsh wlan export profile key=clear" command
			Process process = new();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.Arguments = $"/c netsh wlan export profile key=clear folder=\"{path}\"";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.Start();
			await process.WaitForExitAsync();

			string[] files = Directory.GetFiles(path, "*.xml");

			List<WlanProfile> profiles = new();

			for (int i = 0; i < files.Length; i++)
			{
				XmlSerializer serializer = new(typeof(WlanProfile));
				StreamReader streamReader = new(files[i]); // Where the file is going to be read

				var profile = (WlanProfile?)serializer.Deserialize(streamReader);

				if (profile != null)
				{
					profiles.Add(profile);
				}
				streamReader.Close();
			}

			return profiles;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
			return [];
		}
	}
}