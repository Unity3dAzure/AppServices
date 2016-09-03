using System;
using System.Text;
using UnityEngine;

/// <summary>
/// Query records operation https://msdn.microsoft.com/en-us/library/azure/jj677199.aspx
/// There is a maximum of 50 records returned in a query - use top and skip params to return additional pages of results.
/// NB: `$inlinecount` (which returns count of all items without paging applied) is not set here as it changes the data model and the way the REST decode callback works makes it intangible to decode. 
/// Rather the '$inlinecount=allpages' param is automically set when using the table's Query method and wrapping your data model with the NestedResults object wrapper.
/// </summary>
namespace Unity3dAzure.AppServices
{
	[Flags]
	public enum MobileServiceSystemProperty
	{
		nil = 0x0,
		createdAt = 0x1,
		updatedAt = 0x2,
		version = 0x4,
		deleted = 0x8
	}

	[CLSCompliant(false)]
    public class CustomQuery
    {
		// query option parameters defined by the Open Data Protocol (OData)
        private string _filter;
        private string _orderBy;
		private uint _top;
		private uint _skip;
		private string _select;
		// other params
		private MobileServiceSystemProperty _systemProperties;
		private bool _includeDeleted;

		public CustomQuery(string filter, string orderBy=null, uint top=0, uint skip=0, string select=null, MobileServiceSystemProperty systemProperties=MobileServiceSystemProperty.nil, bool includeDeleted=false)
        {
			_filter = filter; // return only rows that satisfy the specified filter predicate
			_orderBy = orderBy; // sort column by one or more columns: order can be specified in 'desc' or 'asc' order ('asc' is default)
			_top = top; // return the top n entities for any query
			_skip = skip; // the n of records to skip (used for paging results)
			_select = select; // defines new projection of data by specifying the columns
			_systemProperties = systemProperties; // list of system properties to be included in the response
			_includeDeleted = includeDeleted; // if table has soft delete enabled then deleted records will be included in the results 
        }

		public static CustomQuery OrderBy(string orderBy) {
			return new CustomQuery("", orderBy);
		}

        public override string ToString()
        {
            string queryString = "";
            string q = "?";
            if (!string.IsNullOrEmpty(_filter)) {
				queryString += string.Format("{0}$filter=({1})", q, _filter);
                q = "&";
            }
            if (!string.IsNullOrEmpty(_orderBy)) {
				queryString += string.Format("{0}$orderby={1}", q, _orderBy);
                q = "&";
            }
			if (_top > 0) {
				queryString += string.Format("{0}$top={1}", q, _top.ToString());
				q = "&";
			}
			if (_skip > 0) {
				queryString += string.Format("{0}$skip={1}", q, _skip.ToString());
				q = "&";
			}
			if (!string.IsNullOrEmpty(_select)) {
				queryString += string.Format("{0}$select={1}", q, _select);
				q = "&";
			}
			if (_systemProperties!=MobileServiceSystemProperty.nil) {
				// NB: setting __systemproperties param doesn't seem to do anything different as these properties are all included by default, but we can append values to the 'select' param.
				if (!string.IsNullOrEmpty (_select)) {
					queryString += string.Format (",{0}", SystemPropertiesValues (_systemProperties));
				}
				queryString += string.Format("{0}__systemproperties={1}", q, SystemPropertiesValues(_systemProperties)); 
				q = "&";
			}
			if (_includeDeleted) {
				queryString += string.Format("{0}__includeDeleted=true", q);
			}
			return EscapeURL(queryString);
        }

        private string EscapeURL(string query)
        {
			string q = WWW.EscapeURL (query);
			StringBuilder sb = new StringBuilder (q);
			sb.Replace ("+", "%20"); // NB: replace space with '%20' instead of '+'
			// keep query params: ?&=$
			sb.Replace ("%3f", "?"); 
			sb.Replace ("%26", "&");
			sb.Replace ("%3d", "=");
			sb.Replace ("%24", "$"); 
			return sb.ToString();
        }

		private string SystemPropertiesValues(MobileServiceSystemProperty systemProperties) 
		{
			if (systemProperties == MobileServiceSystemProperty.nil) {
				return "";
			}
			return systemProperties.ToString().Replace(" ",""); // remove spaces from string
		}
    }
}