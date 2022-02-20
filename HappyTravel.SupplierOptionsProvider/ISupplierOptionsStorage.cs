using System;
using System.Collections.Generic;
using HappyTravel.SupplierOptionsClient.Models;

namespace HappyTravel.SupplierOptionsProvider
{
    public interface ISupplierOptionsStorage
    {
        List<SlimSupplier> GetAll();
        [Obsolete("Supplier id usage must be replaced by supplier code usage")]
        SlimSupplier GetById(int id);
        void Set(List<SlimSupplier> suppliers);
        SlimSupplier GetByCode(string code);
    }
}