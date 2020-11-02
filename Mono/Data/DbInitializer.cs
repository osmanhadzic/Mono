using Mono.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace Mono.Data
{
    public class DbInitializer
    {
        public static void Initialize(VehicleContext context)
        {
            context.Database.EnsureCreated();

            if (context.vehicleMakes.Any())
            {
                return;
            }

            var vehicleMake = new VehicleMake[]
            {
                new VehicleMake{ id=1,name="Audi",abrv="Audi"}
        };

            foreach (VehicleMake v in vehicleMake)
            {
                context.vehicleMakes.Add(v);
            }

            var vehicleModel = new VehicleModel[]
            {
                new VehicleModel{id=1,name="audi",abrv="audi"}
        };

            foreach (VehicleModel a in vehicleModel)
            {
                context.vehicleModels.Add(a);
            }

            context.SaveChanges();

        }
    }
}
