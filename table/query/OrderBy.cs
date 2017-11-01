// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using UnityEngine;

namespace Azure.AppServices {

  public enum SortDirection {
    asc,
    desc
  }

  public class OrderBy {
    private string column;
    private SortDirection? columnSortDirection;

    public OrderBy(string column, SortDirection? sortDirection = null) {
      this.column = WWW.EscapeURL(column);
      this.columnSortDirection = sortDirection;
    }

    public override string ToString() {
      if (!string.IsNullOrEmpty(column)) {
        string sort = (columnSortDirection != null) ? "+" + columnSortDirection : "";
        return column + sort;
      }
      return "";
    }

  }
}
