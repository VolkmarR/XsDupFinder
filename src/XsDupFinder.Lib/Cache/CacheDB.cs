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

        class Version
        {
            public static string DefaultID = "Version";
            public string Id { get; set; } = DefaultID;
            public int Analyzer { get; set; } = 0;
        }

        readonly LiteDatabase DB;
        readonly ILiteCollection<CacheItem> CacheItemCollection;
        readonly int CurrentAnalyzerVersion = 2;

        public CacheDB(string fileName)
        {
            DB = new LiteDatabase(fileName);

            var version = DB.GetCollection<Version>().FindById(Version.DefaultID) ?? new Version();
            if (version.Analyzer != CurrentAnalyzerVersion)
            {
                DB.DropCollection(nameof(Version));
                DB.DropCollection(nameof(CacheItem));
                DB.GetCollection<Version>().Insert(new Version() { Analyzer = CurrentAnalyzerVersion });
                DB.Rebuild();
            }

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
