﻿namespace Microsoft.AspNet.OData.Extensions.ODataQueryMapper
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.OData;
    using System.Web.OData.Query;

    /// <summary>An attribute that only validates OData queries, and leaves the processing to others.</summary>
    public class ValidateQueryAttribute : EnableQueryAttribute
    {
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            // Store validation settings on request
            actionContext.Request.Properties.Add("odata.ValidationSettings", this.GetValidationSettings());

            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        public override IQueryable ApplyQuery(IQueryable queryable, ODataQueryOptions queryOptions)
        {
            return queryable;
        }

        private ODataValidationSettings GetValidationSettings()
        {
            var settings = new ODataValidationSettings()
            {
                AllowedQueryOptions = this.AllowedQueryOptions,
                AllowedArithmeticOperators = this.AllowedArithmeticOperators,
                AllowedFunctions = this.AllowedFunctions,
                AllowedLogicalOperators = this.AllowedLogicalOperators,
                MaxAnyAllExpressionDepth = this.MaxAnyAllExpressionDepth,
                MaxExpansionDepth = this.MaxExpansionDepth,
                MaxNodeCount = this.MaxNodeCount,
                MaxOrderByNodeCount = this.MaxOrderByNodeCount,
                MaxSkip = this.MaxSkip,
                MaxTop = this.MaxTop
            };

            if (!string.IsNullOrEmpty(this.AllowedOrderByProperties))
            {
                foreach (var s in this.AllowedOrderByProperties.Split(','))
                {
                    settings.AllowedOrderByProperties.Add(s);
                }
            }

            return settings;
        }
    }
}