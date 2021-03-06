﻿#region Copyright
// Copyright Hitachi Consulting
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Xigadee
{
    /// <summary>
    /// This base class adds a tracking correlation id to each incoming request to allow for tracking and tracing.
    /// </summary>
    public class WebApiCorrelationIdFilter: ActionFilterAttribute
    {
        /// <summary>
        /// This is the preferred HTTP key name for the correlation id.
        /// </summary>
        protected readonly string mCorrelationIdKeyName;
        private readonly bool mAddToClaimsPrincipal;

        /// <summary>
        /// This is the default constructor.
        /// </summary>
        /// <param name="correlationIdKeyName">Correlation Id key in the request/response header</param>
        /// <param name="addToClaimsPrincipal">Add the correlation key to the claims principal</param>
        public WebApiCorrelationIdFilter(string correlationIdKeyName = "X-CorrelationId", bool addToClaimsPrincipal = true)
        {
            mCorrelationIdKeyName = correlationIdKeyName;
            mAddToClaimsPrincipal = addToClaimsPrincipal;
        }

        #region CorrelationIdGet()
        /// <summary>
        /// This method creates a new correlation id.
        /// </summary>
        /// <returns>A unique string.</returns>
        protected virtual string CorrelationIdGet()
        {
            return Guid.NewGuid().ToString("N").ToUpperInvariant();
        }
        #endregion

        /// <summary>
        /// This method adds the correlation id to the request if one is not already found in the request headers.
        /// </summary>
        /// <param name="actionContext">The incoming action.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                var request = actionContext.Request;
                IEnumerable<string> correlationValues;
                var correlationId = CorrelationIdGet();
                if (!request.Headers.TryGetValues(mCorrelationIdKeyName, out correlationValues))
                {
                    actionContext.Request.Headers.Add(mCorrelationIdKeyName, correlationId);
                }
                else
                {
                    correlationId = correlationValues.FirstOrDefault() ?? correlationId;
                }

                IRequestOptions apiRequest = actionContext.ActionArguments.Values.OfType<IRequestOptions>().FirstOrDefault();

                if (apiRequest?.Options != null)
                    apiRequest.Options.CorrelationId = correlationId;

                // If we have a claims identity then add the correlation id to it (if component configured to do it)
                var claimsIdentity = actionContext.RequestContext?.Principal?.Identity as ClaimsIdentity;
                if (mAddToClaimsPrincipal && claimsIdentity !=null && !claimsIdentity.HasClaim(c => c.Type == JwtTokenAuthenticationHandler.ClaimProcessCorrelationKey))
                    claimsIdentity.AddClaim(new Claim(JwtTokenAuthenticationHandler.ClaimProcessCorrelationKey, correlationId));
            }
            catch (Exception)
            {
                // Don't prevent normal operation of the site where there is an exception
            }

            base.OnActionExecuting(actionContext);
        }

        /// <summary>
        /// This method adds the correlationid to the outgoing response if one is found in the request headers.
        /// </summary>
        /// <param name="actionExecutedContext">The request and response.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Async request.</returns>
        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>
            {
                base.OnActionExecutedAsync(actionExecutedContext, cancellationToken)
            };

            var request = actionExecutedContext?.Response?.RequestMessage;
            var response = actionExecutedContext?.Response;

            // Retrieve the correlation id from the request and add to the response
            IEnumerable<string> correlationValues = null;
            string correlationId = null;
            if ((request?.Headers?.TryGetValues(mCorrelationIdKeyName, out correlationValues) ?? false))
                correlationId = correlationValues?.FirstOrDefault();

            if (!string.IsNullOrEmpty(correlationId) && response != null && !response.Headers.Contains(mCorrelationIdKeyName))
                response.Headers.Add(mCorrelationIdKeyName, correlationId);

            await Task.WhenAll(tasks);
        }
    }
}
