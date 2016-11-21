﻿using System;
using System.Net;

namespace Unity3dAzure.AppServices
{
	public interface IRestResponse<T>
	{
		bool IsError { get; }

		string ErrorMessage { get; }

		string Url { get; }

		HttpStatusCode StatusCode { get; }

		string Content { get; }

		T Data { get; }
	}
}

