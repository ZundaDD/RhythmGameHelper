using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Client.Utils
{
    internal class UrlBuilder
    {
        private bool _firstQuery = true;
        private bool _beginQuery = false;
        private string _url;

        internal string Url => _url;

        internal UrlBuilder(string baseUrl)
        {
            _url = baseUrl;
        }

        internal UrlBuilder AddSubUrl(string subUrl)
        {
            if (_beginQuery) throw new InvalidOperationException("Can't add subUrl after query began!");

            _url += "/";
            _url += subUrl; 
            return this;
        }

        internal UrlBuilder BeginQuery()
        {
            _beginQuery = true;
            _url += "?";
            return this;
        }

        internal UrlBuilder AddQuery(string key, string value)
        {
            if (!_beginQuery) throw new InvalidOperationException("Can't add query before query begins");

            if (_firstQuery) _firstQuery = false;
            else _url += "&";

            _url += $"{key}={value}";
            return this;
        }

    }
}
