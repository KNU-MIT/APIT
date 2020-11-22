using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ICollectedData<in TKey, out TData, in TBaseData>
    {
        IEnumerable<TData> GetAll();

        TData GetById(TKey id);

        // IEnumerable<TData> GetAll();
        void Create(TBaseData entity);
        void Update(TBaseData entity);
        void Delete(TBaseData entity);
        bool IsExist(TKey id);
        void SaveChanges();
    }
}