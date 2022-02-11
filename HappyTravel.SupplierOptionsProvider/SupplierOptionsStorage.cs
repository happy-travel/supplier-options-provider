using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Options;

namespace HappyTravel.SupplierOptionsProvider
{
    public class SupplierOptionsStorage : ISupplierOptionsStorage
    {
        public SupplierOptionsStorage(IOptions<Configuration> configuration)
        {
            _configuration = configuration.Value;
        }
        
        
        public List<SlimSupplier> GetAll()
        {
            if (SpinWait.SpinUntil(() => _isFilled, _configuration.StorageTimeout))
                return _suppliers;

            throw new Exception("Supplier storage is not filled");
        }


        public SlimSupplier GetById(int id)
        {
            if (SpinWait.SpinUntil(() => _isFilled, _configuration.StorageTimeout))
                return _suppliers.Single(s => s.Id == id);

            throw new Exception("Supplier storage is not filled");
        }

        
        public void Set(List<SlimSupplier> suppliers)
        {
            _suppliers = suppliers;
            _isFilled = true;
        }


        private readonly Configuration _configuration;
        
        private volatile List<SlimSupplier> _suppliers = new();
        private bool _isFilled;
    }
}