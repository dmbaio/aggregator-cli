using aggregator.Engine;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;
using System.Linq;

public class TestClass
{
    public string TestString()
    {
        
        WorkItemWrapper self = null;
        WorkItemStore store = null;
        WorkItemUpdate selfChanges = null;
        //var oldVal = selfChanges.Fields["System.Title"].OldValue;

        if (self.Id == 53429)
        {

            if (selfChanges.Fields.ContainsKey("System.State"))
            {
                var stateUpdate = selfChanges.Fields["System.State"];

                if (stateUpdate.OldValue.ToString() == "UAT Complete" && stateUpdate.NewValue.ToString() == "Released to Production")
                {
                    string message2 = "Child requirements closed.";
                    var allWorkItemLinks = self.RelationLinks;
                    foreach (var requirementLevelLink in allWorkItemLinks.Where(link => string.Equals("System.LinkTypes.Hierarchy-Forward", link.Rel)))
                    {
                        //load children and grandchildren from store
                        var requirementLevelItem = store.GetWorkItem(requirementLevelLink);
                        var taskLevelItems = requirementLevelItem.Children;

                        //close items
                        requirementLevelItem.State = "Closed";
                        foreach (var taskLevelItem in taskLevelItems.Where(item => !string.Equals("Closed", item.State)))
                        {
                            taskLevelItem["Microsoft.VSTS.Scheduling.CompletedWork"] = taskLevelItem["Microsoft.VSTS.Scheduling.OriginalEstimate"];
                            taskLevelItem["Microsoft.VSTS.Scheduling.RemainingWork"] = 0;
                            taskLevelItem.State = "Closed";
                        }
                    }
                    return message2;
                }
                else
                {
                    string message = "Child states updated to match parent state.";
                    var children = self.Children;

                    foreach (var child in children)
                    {
                        child.State = stateUpdate.NewValue.ToString();
                    }
                    return message;
                }
            }

            if (selfChanges.Fields.ContainsKey("Custom.Department"))
            {
                if (new[] { "Credit","Direct","Primary","Real Assets", "Secondary" }.Contains(self["Custom.Department"]))
                {
                    self.AssignedTo = new IdentityRef() { DisplayName = "Ander Belatgui" };
                }

                if (new[] { "Administrative Operations","Business Analysis","Business Intelligence","Custom Solutions","Enterprise Continuous Improvement","ESG","Executive","Firm-Wide","Human Resources","HVPE","Information Technology","Ireland Office Services","Office Services" }.Contains(self["Custom.Department"]))
                {
                    self.AssignedTo = new IdentityRef() { DisplayName = "Joshua Delekta" };
                }

                if (new[] { "Accounting","Accounting - Corporate","Accounting - Direct","Accounting - Investment Accounting","Finance - Admin","Quantitative Research","Treasury" }.Contains(self["Custom.Department"]))
                {
                    self.AssignedTo = new IdentityRef() { DisplayName = "Chelinde Edouard" };
                }

                if (new[] { "Compliance","Legal","Tax" }.Contains(self["Custom.Department"]))
                {
                    self.AssignedTo = new IdentityRef() { DisplayName = "Corey Horohoe" };
                }

                if (new[] { "Client Service","Commercial Operations","Investor Relations","Marketing","Marketing RFP","Private Client Group" }.Contains(self["Custom.Department"]))
                {
                    self.AssignedTo = new IdentityRef() { DisplayName = "James Marino" };
                }

                if (new[] { "Enterprise Data Office","Portfolio Analytics" }.Contains(self["Custom.Department"]))
                {
                    self.AssignedTo = new IdentityRef() { DisplayName = "Tyler Zolud" };
                }
            }

        }
    }
}