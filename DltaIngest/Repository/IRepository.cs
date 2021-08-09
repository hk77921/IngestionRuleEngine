using System.Collections.Generic;

namespace DltaIngest.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // To get the data from collection(s)

        public void InsertRecords<T>(string table, List<T> records);
        public void InsertRecord<T>(string table, T record);
        public List<T> LoadRecords<T>(string table);
        public T FindRecoredByID<T>(string table, string FullyQualifiedDomainName);
    }
}
