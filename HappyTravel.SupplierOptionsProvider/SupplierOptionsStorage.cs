using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CSharpFunctionalExtensions;
using HappyTravel.SupplierOptionsClient.Models;
using Microsoft.Extensions.Options;

namespace HappyTravel.SupplierOptionsProvider
{
    public class SupplierOptionsStorage : ISupplierOptionsStorage
    {
        public SupplierOptionsStorage(IOptions<SupplierOptionsProviderConfiguration> configuration)
        {
            _supplierOptionsProviderConfiguration = configuration.Value;
        }
        
        
        public Result<List<SlimSupplier>> GetAll() 
            => SpinWait.SpinUntil(() => _suppliersAreFilled, _supplierOptionsProviderConfiguration.StorageTimeout) 
                ? _suppliers 
                : Result.Failure<List<SlimSupplier>>("Supplier storage is not filled");


        public Result<SlimSupplier> Get(string code)
        {
            if (!SpinWait.SpinUntil(() => _suppliersAreFilled, _supplierOptionsProviderConfiguration.StorageTimeout))
                return Result.Failure<SlimSupplier>("Supplier storage is not filled");

            var supplier = _suppliers.SingleOrDefault(s => s.Code == code);
            return supplier ?? Result.Failure<SlimSupplier>("Supplier not found");
        }

        
        public Result<SupplierPriorityByTypes> GetPriorities() 
            => SpinWait.SpinUntil(() => _supplierPrioritiesAreFilled, _supplierOptionsProviderConfiguration.StorageTimeout)
                ? _supplierPriorities
                : Result.Failure<SupplierPriorityByTypes>("Supplier storage is not filled");

        
        public void Set(List<SlimSupplier> suppliers)
        {
            _suppliers = suppliers;
            _suppliersAreFilled = true;
        }


        public void Set(SupplierPriorityByTypes priorities)
        {
            _supplierPriorities = priorities;
            _supplierPrioritiesAreFilled = true;
        }

        
        private readonly SupplierOptionsProviderConfiguration _supplierOptionsProviderConfiguration;
        
        private volatile List<SlimSupplier> _suppliers = new();
        private volatile SupplierPriorityByTypes _supplierPriorities = new();
        private bool _suppliersAreFilled;
        private bool _supplierPrioritiesAreFilled;
    }
}