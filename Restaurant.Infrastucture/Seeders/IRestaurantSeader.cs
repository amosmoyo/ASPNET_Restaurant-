using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Infrastucture.Seeders
{
    public interface IRestaurantSeeder
    {
         public Task Seed();
    }
}
