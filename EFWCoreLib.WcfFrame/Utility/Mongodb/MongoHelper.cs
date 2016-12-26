using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace EFWCoreLib.WcfFrame.Utility.Mongodb
{
    public class MongoHelper<T>
    {
        public string conn;
        public string dbName;
        public string collectionName;

        private MongoCollection<T> collection;

        public MongoHelper(string _conn, string _dbName, string _collectionName)
        {
            conn = _conn;
            dbName = _dbName;
            collectionName = _collectionName;
            SetCollection();
        }

        public MongoHelper()
        {
            conn = WcfGlobal.MongoConnStr;
            dbName = "EMRStore";
            collectionName = typeof(T).Name;
            SetCollection();
        }

        /// <summary>
        /// 设置你的collection
        /// </summary>
        public void SetCollection()
        {
            MongoClient client = new MongoClient(conn);
            var server = client.GetServer();
            var database = server.GetDatabase(dbName);
            collection = database.GetCollection<T>(collectionName);
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public T Find(IMongoQuery query)
        {
            return this.collection.FindOne(query);
        }

        /**
         * 条件查询集合
         * */
        public List<T> FindAll(IMongoQuery query)
        {
            return this.collection.Find(query).ToList();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long Update(T model)
        {
            BsonDocument doc = BsonExtensionMethods.ToBsonDocument(model);
            WriteConcernResult res = this.collection.Update(Query.EQ("_id", (model as AbstractMongoModel).id), new UpdateDocument(doc));
            return res.DocumentsAffected;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Insert(T model)
        {
            WriteConcernResult res = this.collection.Insert(model);
            return res.Ok;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Delete(T model)
        {
            WriteConcernResult res = this.collection.Remove(Query.EQ("_id", (model as AbstractMongoModel).id));
            return res.Ok;
        }
    }

    public abstract class AbstractMongoModel
    {
        public ObjectId id { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        //public DateTime created_at { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        //public DateTime updated_at { get; set; }

        public string getstring_id()
        {
            if (id != null)
                return id.ToString();
            return null;
        }
    }
}
