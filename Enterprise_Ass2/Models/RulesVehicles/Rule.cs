using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enterprise_Ass2.Models.RulesVehicles
{
    public class Rule
    {
        public int ID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public State? State { get; set; }
        public String Type { get; set; }
        public string Creator { get; set; }
        
    }

    public enum State
    {
        Approved, Rejected, Pending
    }
}
