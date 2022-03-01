using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        
        
        public List<SlimSupplier> GetAll()
        {
            if (SpinWait.SpinUntil(() => _isFilled, _supplierOptionsProviderConfiguration.StorageTimeout))
                return _suppliers;

            throw new Exception("Supplier storage is not filled");
        }

        
        public SlimSupplier Get(string code)
        {
            if (SpinWait.SpinUntil(() => _isFilled, _supplierOptionsProviderConfiguration.StorageTimeout))
                return _suppliers.Single(s => s.Code == code);

            throw new Exception("Supplier storage is not filled");
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