using System.Collections.Generic;
using HappyTravel.SupplierOptionsClient.Models;

namespace HappyTravel.SupplierOptionsProvider
{
    public interface ISupplierOptionsStorage
    {
        List<SlimSupplier> GetAll();
        SlimSupplier GetById(int id);
        void Set(List<SlimSupplier> suppliers);
    }
}