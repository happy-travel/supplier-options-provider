using System.Collections.Generic;

namespace HappyTravel.SupplierOptionsProvider
{
    public interface ISupplierOptionsStorage
    {
        List<Supplier> All();
        Supplier GetById(int id);
        void Set(List<Supplier> suppliers);
    }
}