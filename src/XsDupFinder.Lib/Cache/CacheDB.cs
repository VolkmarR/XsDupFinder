using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Parser;

namespace XsDupFinder.Lib.Cache
{
    public class CacheDB : IDisposable
    {
        class CacheItem
        {
            public string Id { get; set; }
            public CodeInfo Data { get; set; }
        }

        readonly LiteDatabase DB;
        readonly ILiteCollection<CacheItem> CacheItemCollection;

        public CacheDB(string fileName)
        {
            DB = new LiteDatabase(fileName);

            CacheItemCollection = DB.GetCollection<CacheItem>();
        }

        public void Dispose() => DB.Dispose();

        public bool TryGetValue(SourceCodeFile key, out CodeInfo value)
        {
            value = null;
            var id = new BsonValue(key.FileName.ToLower());
            var cacheItem = CacheItemCollection.FindById(id);
            if (cacheItem != null)
            {
                if (cacheItem.Data.HashCode == key.HashCode)
                    value = cacheItem.Data;
                else
                    CacheItemCollection.Delete(id);
            }
            return value != null;
        }

        public void Add(CodeInfo value)
        {
            var id = new BsonValue(value.FileName.ToLower());
            var item = CacheItemCollection.FindById(id);
            if (item == null)
            { 
                CacheItemCollection.Insert(new CacheItem { Id = id.AsString, Data = value }); 
            }
            else
            {
                item.Data = value;
                CacheItemCollection.Update(item);
            }
        }
    }
}
