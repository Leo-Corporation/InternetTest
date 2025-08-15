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
using PeyrSharp.Enums;

namespace InternetTest.Helpers;

public static class StorageUnitHelper
{
	public static (StorageUnits, double) GetStorageUnit(long size)
	{
		StorageUnits unit = size switch
		{
			long s when s >= Math.Pow(1024, 5) => StorageUnits.Petabyte,
			long s when s >= Math.Pow(1024, 4) => StorageUnits.Terabyte,
			long s when s >= 1073741824 => StorageUnits.Gigabyte,
			long s when s >= 1048576 => StorageUnits.Megabyte,
			long s when s >= 1024 => StorageUnits.Kilobyte,
			_ => StorageUnits.Byte,
		};

		double convertedSize = size / Math.Pow(1024, (int)unit);

		return (unit, convertedSize);
	}

	public static string UnitToString(StorageUnits unit)
	{
		try
		{
			string[] units = Properties.Resources.Units.Split(",");
			return units[(int)unit];
		}
		catch
		{
			return "";
		}
	}
}
