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
    /// The IP-API API wrapper class.
    /// </summary>
    public class IPAPIWrapper
    {
        /// <summary>
        /// <c>success</c> or <c>fail</c>.
        /// </summary>
        private string status { get; set; }

        /// <summary>
        /// Country name.
        /// </summary>
        private string country { get; set; }

        /// <summary>
        /// Two-letter country code ISO 3166-1 alpha-2.
        /// </summary>
        private string countryCode { get; set; }

        /// <summary>
        /// Region/state short code (FIPS or ISO).
        /// </summary>
        private string region { get; set; }

        /// <summary>
        /// Region/state.
        /// </summary>
        private string regionName { get; set; }

        /// <summary>
        /// City.
        /// </summary>
        private string city { get; set; }

        /// <summary>
        /// Zip code.
        /// </summary>
        private string zip { get; set; }

        /// <summary>
        /// Latitude.
        /// </summary>
        private string lat { get; set; }

        /// <summary>
        /// Longitude.
        /// </summary>
        private string lon { get; set; }

        /// <summary>
        /// Timezone (tz).
        /// </summary>
        private string timezone { get; set; }

        /// <summary>
        /// ISP name.
        /// </summary>
        private string isp { get; set; }

        /// <summary>
        /// Organization name.
        /// </summary>
        private string org { get; set; }

        /// <summary>
        /// IP used for the query.
        /// </summary>
        private string query { get; set; }
    }
}
