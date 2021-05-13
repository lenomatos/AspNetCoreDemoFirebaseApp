using DemoFirebase.Infra.Models.Firestore;
using System.Collections.Generic;

namespace DemoFirebase.Infra.Repositories.Interfaces
{
    public interface IFirebaseService<TEntity> where TEntity : Base
    {
        TEntity Get(TEntity record);
        List<TEntity> GetAll();
        TEntity Add(TEntity record);
        bool Set(TEntity record);
        bool SetMerge(TEntity record);
        bool Update(TEntity record);
        bool Delete(TEntity record);
    }
}
