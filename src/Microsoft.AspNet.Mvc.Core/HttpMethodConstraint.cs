// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Microsoft.AspNet.Mvc
{
    public class HttpMethodConstraint : IActionConstraint
    {
        private readonly IReadOnlyList<string> _methods;

        // Empty collection means any method will be accepted.
        public HttpMethodConstraint(IEnumerable<string> httpMethods)
        {
            if (httpMethods == null)
            {
                throw new ArgumentNullException("httpMethods");
            }

            var methods = new List<string>();

            foreach (var method in httpMethods)
            {
                if (string.IsNullOrEmpty(method))
                {
                    throw new ArgumentException("httpMethod cannot be null or empty");
                }

                methods.Add(method);
            }

            _methods = new ReadOnlyCollection<string>(methods);
        }

        public IEnumerable<string> HttpMethods
        {
            get
            {
                return _methods;
            }
        }

        public bool Accept(RequestContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (_methods.Count == 0)
            {
                return true;
            }

            var request = context.HttpContext.Request;

            return (HttpMethods.Any(m => m.Equals(request.Method, StringComparison.Ordinal)));
        }
    }
}
