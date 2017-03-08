using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.PolicyStatus;
using System.Collections.Generic;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Items
{
    public class PolicyStatus
    {
        public PolicyStatusEnum OverallStatus { get; set; }

        public string UpdatedAt { get; set; }

        public int InViolationCount { get; set; } = 0;
        public int InViolationOverriddenCount { get; set; } = 0;
        public int NotInViolationCount { get; set; } = 0;

        public PolicyStatus()
        {

        }

        public PolicyStatus(VersionBomPolicyStatusView policyView)
        {
            OverallStatus = policyView.OverallStatus;
            UpdatedAt = policyView.UpdatedAt;

            List<ViolationCountView> counts = policyView.ComponentVersionStatusCounts;
            foreach (ViolationCountView pair in counts)
            {
                PolicyStatusEnum status = new PolicyStatusEnum(pair.Name);
                if (status.Equals(PolicyStatusEnum.IN_VIOLATION))
                {
                    InViolationCount = pair.Count;
                }
                else if (status.Equals(PolicyStatusEnum.IN_VIOLATION_OVERRIDDEN))
                {
                    InViolationOverriddenCount = pair.Count;
                }
                else if (status.Equals(PolicyStatusEnum.NOT_IN_VIOLATION))
                {
                    NotInViolationCount = pair.Count;
                }
            }

        }
    }
}
