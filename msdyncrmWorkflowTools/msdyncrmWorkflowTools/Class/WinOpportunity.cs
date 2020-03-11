using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace msdyncrmWorkflowTools
{
    public class WinOpportunity : CodeActivity
    {
        #region "Parameter Definition"
        [RequiredArgument]
        [Input("Opportunity")]
        [ReferenceTarget("opportunity")]
        public InArgument<EntityReference> Opportunity { get; set; }
        #endregion

        protected override void Execute(CodeActivityContext executionContext)
        {

            #region "Load CRM Service from context"

            Common objCommon = new Common(executionContext);
            objCommon.tracingService.Trace("Load CRM Service from context --- OK");

            #endregion

            #region "Read Parameters"
            EntityReference opportunity = this.Opportunity.Get(executionContext);
            if (opportunity == null)
            {
                return;
            }

            objCommon.tracingService.Trace("OpportunityID=" + opportunity.Id);
            #endregion

            #region "WinOpportuntity Execution"
            var winOpportunityReq = new WinOpportunityRequest
            {
                Status = new OptionSetValue()
            };

            winOpportunityReq.OpportunityClose = new Entity
            {
                LogicalName = "opportunityclose"
            };

            winOpportunityReq.OpportunityClose.Attributes.Add("OpportunityId", new EntityReference(opportunity.LogicalName, opportunity.Id));

            var winOpportunityRes =
                (WinOpportunityResponse)objCommon.service.Execute(winOpportunityReq);
            Console.WriteLine("  Executed OK.");
            #endregion
        }
    }
}
