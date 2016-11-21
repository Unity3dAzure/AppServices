using System;
using System.Net;

namespace Unity3dAzure.AppServices
{
	public abstract class Response
	{
		private bool isError;
		public bool IsError { 
			get { 
				return this.isError;
			}
		}

		private string errorMessage;
		public string ErrorMessage { 
			get { 
				return this.errorMessage;
			}
		}

		private string url;
		public string Url { 
			get { 
				return this.url;
			}
		}

		private HttpStatusCode statusCode;
		public HttpStatusCode StatusCode { 
			get { 
				return this.statusCode;
			}
		}

		private string content;
		public string Content { 
			get { 
				return this.content;
			}
		}

		// success
		protected Response (string url, HttpStatusCode statusCode, string text)
		{
			this.isError = false;
			this.url = url;
			this.errorMessage = null;
			this.statusCode = statusCode;
			this.content = text;
		}

		// failure
		protected Response (string error, string url, HttpStatusCode statusCode, string text)
		{
			this.isError = true;
			this.url = url;
			this.errorMessage = error;
			this.statusCode = statusCode;
			this.content = text;
		}
	}

	public sealed class RestResponse : Response
	{
		// success
		public RestResponse (string url, HttpStatusCode statusCode, string text) : base(url, statusCode, text){}

		// failure
		public RestResponse (string error, string url, HttpStatusCode statusCode, string text) : base(error, url, statusCode, text){}
	}

	public sealed class RestResponse<T> : Response, IRestResponse<T>
	{
		private T data;
		public T Data {
			get {
				return this.data;
			}
		}

		// success
		public RestResponse (string url, HttpStatusCode statusCode, string text, T data) : base(url, statusCode, text) 
		{
			this.data = data;
		}

		// failure
		public RestResponse (string error, string url, HttpStatusCode statusCode, string text) : base(error, url, statusCode, text) 
		{
		}
	}

}

