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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetTest.Classes
{
    /// <summary>
    /// This class contains an IP Information
    /// </summary>
    public class IPInfo
    {
        /// <summary>
        /// <c>success</c> or <c>fail</c>.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Country name.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Two-letter country code ISO 3166-1 alpha-2.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Region/state short code (FIPS or ISO).
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Region/state.
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// City.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Zip code.
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Latitude.
        /// </summary>
        public string Lat { get; set; }

        /// <summary>
        /// Longitude.
        /// </summary>
        public string Lon { get; set; }

        /// <summary>
        /// Timezone (tz).
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// ISP name.
        /// </summary>
        public string ISP { get; set; }

        /// <summary>
        /// Organization name.
        /// </summary>
        public string Org { get; set; }

        /// <summary>
        /// IP used for the query.
        /// </summary>
        public string Query { get; set; }

        public override string ToString()
        {
            return $"{Properties.Resources.Country}: {Country}\n" +
                $"{Properties.Resources.Region}: {RegionName}\n" +
                $"{Properties.Resources.City}: {City}\n" +
                $"{Properties.Resources.ZIPCode}: {Zip}\n" +
                $"{Properties.Resources.Latitude}: {Lat}\n" +
                $"{Properties.Resources.Longitude}: {Lon}\n" +
                $"{Properties.Resources.Timezone}: {TimeZone}\n" +
                $"{Properties.Resources.ISP}: {ISP}\n";
        }
    }
}
