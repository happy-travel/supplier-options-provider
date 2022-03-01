using System.Collections.Generic;
using HappyTravel.SupplierOptionsClient.Models;

namespace HappyTravel.SupplierOptionsProvider
{
    public interface ISupplierOptionsStorage
    {
        List<SlimSupplier> GetAll();
        void Set(List<SlimSupplier> suppliers);
        SlimSupplier Get(string code);
    }
}