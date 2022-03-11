using System.Collections.Generic;
using CSharpFunctionalExtensions;
using HappyTravel.SupplierOptionsClient.Models;

namespace HappyTravel.SupplierOptionsProvider
{
    public interface ISupplierOptionsStorage
    {
        Result<List<SlimSupplier>> GetAll();
        void Set(List<SlimSupplier> suppliers);
        Result<SlimSupplier> Get(string code);
    }
}