using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Enterprise_Ass2.Models.RulesVehicles
{
    public class Vehicle
    {
        public int ID { get; set; }
        public string Model { get; set; }
        [Display(Name = "Full Name")]
        public VType VType { get; set; }
        public Brand? Brand { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
        public string ModifyBy { get; set; }
    }

    public enum VType
    {
        Sedan, SUV, Hatchback, Coupe, CUV, MPV
    }

    public enum Brand
    {
        Hyundai, Tesla, BMW, Nissan, Honda
    }
}
