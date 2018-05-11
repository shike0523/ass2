using Enterprise_Ass2.Models.RulesVehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enterprise_Ass2.Data
{
    public static class DbInitializer
    {
        public static void Initialize(VehicleContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Rules.Any())
            {
                return;   // DB has been seeded
            }

            var rules = new Rule[]
            {
            new Rule{Question="what is % type",Answer="Type",State=State.Pending, Type="DataDriven", Creator="Editor@editor.com"},
            new Rule{Question="what is % desc",Answer="description",State=State.Pending, Type="DataDriven", Creator="Editor@editor.com"},
            new Rule{Question="what is % 's capacity",Answer="capacity",State=State.Approved, Type="DataDriven", Creator="Editor@editor.com"},
            new Rule{Question="when the %  was released",Answer="ReleaseDate",State=State.Approved, Type="DataDriven", Creator="Editor@editor.com"},
            new Rule{Question="what the release year of % is",Answer="ReleaseDate",State=State.Rejected, Type="DataDriven", Creator="Editor@editor.com"},
            new Rule{Question="Where is HONDA from",Answer="Japan",State=State.Approved, Type="Simple", Creator="Editor2@editor.com"},
            new Rule{Question="How many doors does a Coupe have",Answer="2",State=State.Rejected, Type="Simple", Creator="Editor2@editor.com"},
            new Rule{Question="what is % brand",Answer="brand",State=State.Pending, Type="DataDriven", Creator="Editor2@editor.com"}
            };
            foreach (Rule s in rules)
            {
                context.Rules.Add(s);
            }
            context.SaveChanges();

            var vehicles = new Vehicle[]
            {
            new Vehicle{Model="H001",VType=VType.CUV,Brand=Brand.Hyundai,Capacity=2,Description="perfect car here",ReleaseDate=DateTime.Parse("2005-09-01"),ModifyBy="data@data.com"},
            new Vehicle{Model="H001",VType=VType.CUV,Brand=Brand.Hyundai,Capacity=2,Description="perfect car here",ReleaseDate=DateTime.Parse("2005-09-01"),ModifyBy="data@data.com"}
            };
            foreach (Vehicle c in vehicles)
            {
                context.Vehicles.Add(c);
            }
            context.SaveChanges();           
        }
    }
}
