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
            => SpinWait.SpinUntil(() => _isFilled, _supplierOptionsProviderConfiguration.StorageTimeout) 
                ? _suppliers 
                : Result.Failure<List<SlimSupplier>>("Supplier storage is not filled");


        public Result<SlimSupplier> Get(string code)
        {
            if (!SpinWait.SpinUntil(() => _isFilled, _supplierOptionsProviderConfiguration.StorageTimeout))
                return Result.Failure<SlimSupplier>("Supplier storage is not filled");

            var supplier = _suppliers.SingleOrDefault(s => s.Code == code);
            return supplier ?? Result.Failure<SlimSupplier>("Supplier not found");
        }
        
        
        public void Set(List<SlimSupplier> suppliers)
        {
            _suppliers = suppliers;
            _isFilled = true;
        }


        private readonly SupplierOptionsProviderConfiguration _supplierOptionsProviderConfiguration;
        
        private volatile List<SlimSupplier> _suppliers = new();
        private bool _isFilled;
    }
}