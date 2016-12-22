using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EFWCoreLib.WcfFrame.Utility.Mongodb
{
    /// <summary>
    /// MongoDB操作类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongoHelper<T>
    {
        public string conn;
        public string dbName;
        public string collectionName;

        private IMongoCollection<T> collection;

        public MongoHelper(string _conn, string _dbName, string _collectionName)
        {
            conn = _conn;
            dbName = _dbName;
            collectionName = _collectionName;
            SetCollection();
        }

        /// <summary>
        /// 设置你的collection
        /// </summary>
        public void SetCollection()
        {
            MongoClient client = new MongoClient(conn);
            //var server = client.GetServer();
            var database = client.GetDatabase(dbName);
            collection = database.GetCollection<T>(collectionName);
        }


        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public T Find(string id)
        {
            ObjectId objid = new ObjectId(id);
            var filter = Builders<T>.Filter.Eq("_id", objid);
            return this.collection.Find(filter).FirstAsync().Result;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<T> FindAll(Expression<Func<T, bool>> filter)
        {
            return this.collection.Find(filter).ToListAsync().Result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public long Update(string id, string fieldname, object val)
        {
            ObjectId objid = new ObjectId(id);
            var filter = Builders<T>.Filter.Eq("_id", objid);
            var update = Builders<T>.Update.Set(fieldname, val);
            return this.collection.UpdateOne(filter, update).ModifiedCount;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Insert(T model)
        {
            this.collection.InsertOne(model);
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long Delete(string id)
        {
            ObjectId objid = new ObjectId(id);
            var filter = Builders<T>.Filter.Eq("_id", objid);
            return this.collection.DeleteOne(filter).DeletedCount;
        }

        /// 将 Stream 转成 byte[]
        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// 将 byte[] 转成 Stream
        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }

    public abstract class AbstractMongoModel
    {
        public ObjectId id { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        //public DateTime created_at { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        //public DateTime updated_at { get; set; }
    }
}
