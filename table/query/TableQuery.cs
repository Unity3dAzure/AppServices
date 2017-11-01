// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using UnityEngine;

/// <summary>
/// Implemention of Query records operation https://msdn.microsoft.com/en-us/library/azure/jj677199.aspx
/// By default there is a maximum of 50 records returned in a query - use top and skip params to return additional pages of results.
/// NB: The `$inlinecount` param (which returns total count of all items without paging applied) is not set here as it alters the data model shape.
/// Rather the '$inlinecount=allpages' param is automatically set when using the table's Query method and wrapping your data model with the NestedResults object.
/// </summary>
namespace Azure.AppServices {
  [Flags]
  public enum TableSystemProperty {
    nil = 0x0,
    createdAt = 0x1,
    updatedAt = 0x2,
    version = 0x4,
    deleted = 0x8
  }

  public class TableQuery {
    public string Filter;
    public OrderBy[] OrderBy;
    public string Select;
    public uint Skip;
    public uint Top;

    // other params
    public bool IncludeDeleted;
    public TableSystemProperty SystemProperties;
    public bool NoScript = false; // When a value of `true` is supplied, the execution of registered scripts is suppressed.
    // NB: To suppress script execution, you must also supply the service master key with the "X-ZUMO-MASTER" header.
    // For more info and security note refer to: https://msdn.microsoft.com/en-us/library/azure/jj677199.aspx

    public TableQuery(string filterPredicate = "", uint top = 0, uint skip = 0, string selectColumns = null, TableSystemProperty systemProperties = TableSystemProperty.nil, bool includeDeleted = false, params OrderBy[] orderBy) {
      // supports the following subset of the query option parameters defined by the Open Data Protocol (OData)
      this.Filter = filterPredicate; // return only rows that satisfy the specified filter predicate
      this.OrderBy = orderBy; // sort column by one or more columns: order can be specified in 'desc' or 'asc' order ('asc' is default)
      this.Select = selectColumns; // defines new projection of data by specifying the columns
      this.Skip = skip; // the n of records to skip (used for paging results)
      this.Top = top; // return the top n entities for any query

      this.IncludeDeleted = includeDeleted; // if table has soft delete enabled then deleted records will be included in the results
      this.SystemProperties = systemProperties; // list of system properties to be included in the response
    }

    public static TableQuery CreateWithOrderBy(params OrderBy[] orderBy) {
      return new TableQuery("", 0, 0, null, TableSystemProperty.nil, false, orderBy);
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("?");
      if (!string.IsNullOrEmpty(Filter)) {
        sb.Append(string.Format("$filter=({0})&", WWW.EscapeURL(Filter)));
      }
      if (OrderBy != null && OrderBy.Length > 0) {
        sb.Append(string.Format("$orderby="));
        foreach (OrderBy order in OrderBy) {
          sb.Append(order.ToString() + ",");
        }
        sb.Remove(sb.Length - 1, 1); // remove trailing ','
        sb.Append(string.Format("&"));
      }
      if (Top > 0) {
        sb.Append(string.Format("$top={0}&", Top.ToString()));
      }
      if (Skip > 0) {
        sb.Append(string.Format("$skip={0}&", Skip.ToString()));
      }
      if (!string.IsNullOrEmpty(Select)) {
        sb.Append(string.Format("$select={0}&", WWW.EscapeURL(Select)));
        if (SystemProperties != TableSystemProperty.nil) {
          // NB: setting `__systemproperties` param doesn't appear to do anything so I've added a workaround to append these values onto the 'select' param.
          sb.Remove(sb.Length - 1, 1); // remove trailing '&'
          sb.Append(string.Format(",{0}&", SystemPropertiesValues(this.SystemProperties)));
        }
      }
      if (SystemProperties != TableSystemProperty.nil) {
        sb.Append(string.Format("__systemproperties={0}&", SystemPropertiesValues(SystemProperties)));
      }
      if (this.IncludeDeleted) {
        sb.Append("__includeDeleted=true&");
      }
      if (this.NoScript) {
        sb.Append("noscript=true&");
      }
      sb.Remove(sb.Length - 1, 1); // remove trailing '&'
      return sb.ToString();
    }

    private string SystemPropertiesValues(TableSystemProperty systemProperties) {
      if (systemProperties == TableSystemProperty.nil) {
        return "";
      }
      return systemProperties.ToString().Replace(" ", ""); // remove spaces from string
    }
  }
}
