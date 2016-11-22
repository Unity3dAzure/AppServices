﻿using System;
using System.Net;

namespace Unity3dAzure.AppServices
{
	public abstract class Response
	{
		public bool IsError { get; internal set; }

		public string ErrorMessage { get; internal set; }

		public string Url { get; internal set; }

		public HttpStatusCode StatusCode { get; internal set; }

		public string Content { get; internal set; }

		protected Response (HttpStatusCode statusCode)
		{
			this.StatusCode = statusCode;
			this.IsError = !((int)statusCode >= 200 && (int)statusCode < 300);
		}

		// success
		protected Response (HttpStatusCode statusCode, string url, string text)
		{
			this.IsError = false;
			this.Url = url;
			this.ErrorMessage = null;
			this.StatusCode = statusCode;
			this.Content = text;
		}

		// failure
		protected Response (string error, HttpStatusCode statusCode, string url, string text)
		{
			this.IsError = true;
			this.Url = url;
			this.ErrorMessage = error;
			this.StatusCode = statusCode;
			this.Content = text;
		}
	}

	public sealed class RestResponse : Response
	{
		// success
		public RestResponse (HttpStatusCode statusCode, string url, string text) : base (statusCode, url, text)
		{
		}

		// failure
		public RestResponse (string error, HttpStatusCode statusCode, string url, string text) : base (error, statusCode, url, text)
		{
		}
	}

	public sealed class RestResponse<T> : Response, IRestResponse<T>
	{
		public T Data { get; internal set; }

		// success
		public RestResponse (HttpStatusCode statusCode, string url, string text, T data) : base (statusCode, url, text)
		{
			this.Data = data;
		}

		// failure
		public RestResponse (string error, HttpStatusCode statusCode, string url, string text) : base (error, statusCode, url, text)
		{
		}
	}

	internal sealed class RestResult<T> : Response
	{
		public T AnObject { get; internal set; }

		public T[] AnArrayOfObjects { get; internal set; }

		public RestResult (HttpStatusCode statusCode) : base (statusCode)
		{
		}
	}

}

