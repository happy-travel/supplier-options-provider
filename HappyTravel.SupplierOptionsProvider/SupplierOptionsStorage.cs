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
        
        
        public List<Supplier> All()
        {
            if (SpinWait.SpinUntil(() => _isFilled, _configuration.StorageTimeout))
                return _suppliers;

            throw new Exception("Supplier storage is not filled");
        }


        public Supplier GetById(int id)
        {
            if (SpinWait.SpinUntil(() => _isFilled, _configuration.StorageTimeout))
                return _suppliers.Single(s => s.Id == id);

            throw new Exception("Supplier storage is not filled");
        }
        

        public void Set(List<Supplier> suppliers)
        {
            _suppliers = suppliers;
            _isFilled = true;
        }


        private readonly Configuration _configuration;
        
        private volatile List<Supplier> _suppliers = new();
        private bool _isFilled;
    }
}