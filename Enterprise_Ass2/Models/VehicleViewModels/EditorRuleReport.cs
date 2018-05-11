using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enterprise_Ass2.Models.VehicleViewModels
{
    public class EditorRuleReport
    {
        public string EditorUserName { get; set; }
        public int ApprovedRuleCount { get; set; }
        public int RejectedRuleCount { get; set; }
        public string SuccessRate { get; set; }
    }
}
