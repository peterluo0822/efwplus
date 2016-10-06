//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Data.Common;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Practices.EnterpriseLibrary.Data;
using EFWCoreLib.CoreFrame.Orm;

namespace EFWCoreLib.CoreFrame.DbProvider
{
    [Serializable]
    public abstract class AbstractDatabase:ICloneable
	{
        /// <summary>
        /// 数据库连接
        /// </summary>
        protected DbConnection connection = null;			//数据库连接

        /// <summary>
        /// 数据库对象执行命令
        /// </summary>
        protected DbCommand command = null;

        /// <summary>
        /// 企业库数据库访问对象
        /// </summary>
        public Database database = null;

        #region 属性

        /// <summary>
        /// 数据库事务
        /// </summary>
        protected DbTransaction transaction = null;			//数据库事务

        protected string _connString;
        /// <summary>
        /// 返回数据库连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return _connString; }
        }
        /// <summary>
        /// 返回数据库连接对象
        /// </summary>
        public DbConnection Connection
        {
            get { return connection; }
        }
        /// <summary>
        /// 返回数据库事务对象
        /// </summary>
        public DbTransaction Transaction
        {
            get { return transaction; }
        }

        protected bool isInTransaction = false;				//是否在事务中
        /// <summary>
        /// 返回是否处于事务中
        /// </summary>
        protected bool IsInTransaction
        {
            get { return this.isInTransaction; }
        }

        public int WorkId { get; set; }
        //Entlib.config配置文件中配置的连接数据库的key
        public string DbKey { get; set; }
        //插件名称
        public string PluginName { get; set; }
        //数据库类型
        public DatabaseType DbType { get; set; }

        public abstract void TestDbConnection();

        #endregion
		
		/// <summary>
		/// 启动一个事务
		/// </summary>
        public abstract void BeginTransaction();
		/// <summary>
		/// 提交一个事务
		/// </summary>
        public abstract void CommitTransaction();
		/// <summary>
		/// 回滚一个事务
		/// </summary>
        public abstract void RollbackTransaction();
        /// <summary>
        /// 获取IDbCommand
        /// </summary>
        /// <param name="commandtext"></param>
        /// <returns></returns>
        public abstract IDbCommand GetCommand(string commandtext);
        

        #region 字节数据字段存储
        /// <summary>
        /// 获取字节数据
        /// </summary>
        /// <param name="commandtext"></param>
        /// <returns></returns>
        public abstract byte[] GetBlobData(string commandtext);
        /// <summary>
        /// 保存字节数据
        /// </summary>
        /// <param name="commandtext">参数名称必须@blob</param>
        /// <param name="blob"></param>
        /// <returns></returns>
        public abstract bool SaveBlobData(string commandtext, byte[] blob);
        #endregion

        #region 执行插入一条记录 适用于有 自动生成标识的列
        public abstract int InsertRecord(string commandtext);
		#endregion

		#region 返回一个DataTable
		public abstract DataTable GetDataTable(string commandtext);
        public abstract DataTable GetDataTable(IDbCommand cmd);
        #endregion

        #region 返回一个DataReader
        public abstract IDataReader GetDataReader(string commandtext);
        public abstract IDataReader GetDataReader(IDbCommand cmd);
        #endregion

        #region 执行一个语句，返回执行情况
        public abstract int DoCommand(string commandtext);
        
        public abstract int DoCommand(IDbCommand cmd);

        #endregion

        #region 执行一个命令返回一个数据结果
        public abstract object GetDataResult(string commandtext);
        public abstract object GetDataResult(IDbCommand cmd);
        #endregion

        #region Query
        public abstract IEnumerable<dynamic> Query(string sql, object param);
        public abstract IEnumerable<T> Query<T>(string sql, object param);


        public abstract IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param);

        public abstract IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param);

        public abstract IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param);

        public abstract SqlMapper.GridReader QueryMultiple(string sql, object param);
        #endregion

        #region 访问存储过程
        /// <summary>
        /// 获取存储过程DbCommand
        /// </summary>
        /// <param name="storeProcedureName"></param>
        /// <returns></returns>
        public abstract IDbCommand GetProcCommand(string storeProcedureName);
        /// <summary>
        /// 增加存储过程输入参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        public abstract void AddInParameter(DbCommand command,string name, System.Data.DbType dbType, object value);
        /// <summary>
        /// 增加存储过程输出参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        public abstract void AddOutParameter(DbCommand command,string name, System.Data.DbType dbType, int size);
        /// <summary>
        /// 获取存储过程输出参数的值
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract object GetParameterValue(System.Data.Common.DbCommand command, string name);

        public abstract DataTable GetDataTable(string storeProcedureName, params object[] parameters);
        public abstract int DoCommand(string storeProcedureName, params object[] parameters);
        public abstract object GetDataResult(string storeProcedureName, params object[] parameters);
        public abstract DataSet GetDataSet(string storeProcedureName, params object[] parameters);

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

    }



}
