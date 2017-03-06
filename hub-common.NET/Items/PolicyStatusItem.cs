using Com.Blackducksoftware.Integration.Hub.Common.Net.Constants;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Items
{
    public class PolicyStatusItem : Item
    {
        public PolicyStatus OverallStatus { get; set; }

        public string UpdatedAt { get; set; }

        public int InViolationCount { get; set; } = 0;
        public int InViolationOverriddenCount { get; set; } = 0;
        public int NotInViolationCount { get; set; } = 0;

        public PolicyStatusItem()
        {

        }

        public PolicyStatusItem(VersionBomPolicyStatusView policyView)
        {
            OverallStatus = new PolicyStatus(policyView.OverallStatus);
            UpdatedAt = policyView.UpdatedAt;

            List<ViolationCountView> counts = policyView.ComponentVersionStatusCounts;
            foreach (ViolationCountView pair in counts)
            {
                PolicyStatus status = new PolicyStatus(pair.Name);
                if (status.Equals(PolicyStatus.IN_VIOLATION))
                {
                    InViolationCount = pair.Count;
                }
                else if (status.Equals(PolicyStatus.IN_VIOLATION_OVERRIDDEN))
                {
                    InViolationOverriddenCount = pair.Count;
                }
                else if (status.Equals(PolicyStatus.NOT_IN_VIOLATION))
                {
                    NotInViolationCount = pair.Count;
                }
            }

        }
    }
}
