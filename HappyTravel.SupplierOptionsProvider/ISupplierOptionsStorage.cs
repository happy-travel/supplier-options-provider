using System.Collections.Generic;

namespace HappyTravel.SupplierOptionsProvider
{
    public interface ISupplierOptionsStorage
    {
        List<SlimSupplier> GetAll();
        SlimSupplier GetById(int id);
        void Set(List<SlimSupplier> suppliers);
    }
}