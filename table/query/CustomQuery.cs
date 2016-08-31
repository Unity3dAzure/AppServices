using System;
using RestSharp.Contrib;

namespace Unity3dAzure.AppServices
{
	[CLSCompliant(false)]
    public class CustomQuery
    {
        private string _filter;
        private string _orderBy;
		private UInt32 _top;

		public CustomQuery(string filter, string orderBy=null, UInt32 top=0)
        {
            _filter = filter; // return only rows that satisty the specified filter
			_orderBy = orderBy; // sort column by 'desc' or 'asc' order
			_top = top; // return the top n entities for any query,
        }

		public static CustomQuery OrderBy(string orderBy) {
			return new CustomQuery("", orderBy);
		}

        public override string ToString()
        {
            string queryString = "";
            string q = "?";
            if (!string.IsNullOrEmpty(_filter)){
                queryString += string.Format("{0}$filter=({1})", q, encode(_filter));
                q = "&";
            }
            if (!string.IsNullOrEmpty(_orderBy)){
				queryString += string.Format("{0}$orderby={1}", q, encode(_orderBy));
                q = "&";
            }
			if (_top > 0) {
				queryString += string.Format("{0}$top={1}", q, _top.ToString());
			}
            return queryString;
        }
        
        private string encode(string url)
        {
			url = url.Replace ("'", "%27"); // replace "'" with '%27'
			return HttpUtility.UrlPathEncode (url); // replaces " " with '%20' and not '+'
        }
        
    }
}