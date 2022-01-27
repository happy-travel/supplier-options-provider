using System.Collections.Generic;

namespace HappyTravel.SunpuClient
{
    public interface IStorage
    {
        List<Supplier> Get();
        void Set(List<Supplier> markupPolicies);
    }
}