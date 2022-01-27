using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Options;

namespace HappyTravel.SunpuClient
{
    public class Storage : IStorage
    {
        public Storage(IOptions<Configuration> configuration)
        {
            _configuration = configuration.Value;
        }
        
        
        public List<Supplier> Get()
        {
            if (SpinWait.SpinUntil(() => _isFilled, TimeSpan.FromSeconds(_configuration.StorageTimeoutInSeconds)))
                return _suppliers;

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