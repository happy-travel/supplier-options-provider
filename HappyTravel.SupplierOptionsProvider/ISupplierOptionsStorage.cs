using System.Collections.Generic;

namespace HappyTravel.SupplierOptionsProvider
{
    public interface ISupplierOptionsStorage
    {
        List<Supplier> GetAll();
        Supplier GetById(int id);
        void Set(List<Supplier> suppliers);
    }
}