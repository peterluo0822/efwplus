using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;
using EFWCoreLib.CoreFrame.EntLib;
using EFWCoreLib.CoreFrame.Orm;

namespace EFWCoreLib.CoreFrame.DbProvider
{
    /// <summary>
    /// EntLibDb
    /// </summary>
    public class EntLibDb : AbstractDatabase
    {


        public EntLibDb()
            : base()
        {
            database = ZhyContainer.CreateDataBase();
            _connString = database.ConnectionString;

            switch (database.GetType().Name)
            {
                case "SqlDatabase":
                    DbType = DatabaseType.SqlServer2005;
                    break;
                case "OracleDatabase":
                    DbType = DatabaseType.Oracle;
                    break;
                default:
                    DbType = DatabaseType.UnKnown;
                    break;
            }
        }

        public EntLibDb(string key)
            : base()
        {

            database = ZhyContainer.CreateDataBase(key);
            _connString = database.ConnectionString;

            switch (database.GetType().Name)
            {
                case "SqlDatabase":
                    DbType = DatabaseType.SqlServer2005;
                    break;
                case "OracleDatabase":
                    DbType = DatabaseType.Oracle;
                    break;
                default:
                    DbType = DatabaseType.UnKnown;
                    break;
            }
        }

        public override void TestDbConnection()
        {
            database.CreateConnection().Open();
        }

        public override void BeginTransaction()
        {
            try
            {
                if (isInTransaction == false)
                {
                    connection = database.CreateConnection();
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    isInTransaction = true;
                }
                else
                {
                    throw new Exception("事务正在进行，一个对象不能同时开启多个事务！");
                }
            }
            catch (Exception e)
            {
                connection.Close();
                isInTransaction = false;
                throw new Exception("事务启动失败，请再试一次！\n" + e.Message);
            }
        }
        public override void CommitTransaction()
        {
            if (transaction != null)
            {
                transaction.Commit();
                isInTransaction = false;
                connection.Close();
            }
            else

                throw new Exception("无可用事务！");
        }
        public override void RollbackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                isInTransaction = false;
                connection.Close();
            }
            else
                throw new Exception("无可用事务！");
        }

        public override byte[] GetBlobData(string commandtext)
        {
            Byte[] blob = null;

            IDataReader sdr = GetDataReader(commandtext);
            if (sdr.Read())
            {
                object obj = sdr.GetValue(0);
                if (obj != System.DBNull.Value)
                    blob = (byte[])obj;
            }
            sdr.Close();
            return blob;
        }

        public override bool SaveBlobData(string commandtext, byte[] blob)
        {
            byte[] buffer = blob;
            IDbCommand cmd = GetCommand(commandtext);
            database.AddInParameter((DbCommand)cmd, "@blob", System.Data.DbType.Binary);
            database.SetParameterValue((DbCommand)cmd, "@blob", buffer);
            //SetParameterValue(cmd, "@blob", buffer);
            DoCommand(cmd);
            return true;
        }


        public override int InsertRecord(string commandtext)
        {
            switch (DbType)
            {
                case DatabaseType.Oracle:
                    //string strsql = "SELECT Test_SQL.nextval FROM dual";SELECT  @@IDENTITY
                    if (isInTransaction)
                    {
                        command = database.GetSqlStringCommand(commandtext);
                        command.Connection = connection;
                        command.Transaction = transaction;
                        //command.CommandType = CommandType.Text;
                        //command.CommandText = command.CommandText + ";SELECT  @@IDENTITY";
                        return Convert.ToInt32(database.ExecuteScalar(command, transaction));
                    }
                    else
                    {
                        command = database.GetSqlStringCommand(commandtext);
                        //command.CommandText = command.CommandText + ";SELECT  @@IDENTITY";
                        return Convert.ToInt32(database.ExecuteScalar(command));
                    }
                case DatabaseType.SqlServer2005:
                case DatabaseType.SqlServer2000:
                    if (isInTransaction)
                    {
                        command = database.GetSqlStringCommand(commandtext);
                        command.Connection = connection;
                        command.Transaction = transaction;
                        //command.CommandType = CommandType.Text;
                        command.CommandText = command.CommandText + ";SELECT  @@IDENTITY";
                        return Convert.ToInt32(database.ExecuteScalar(command, transaction));
                    }
                    else
                    {
                        command = database.GetSqlStringCommand(commandtext);
                        command.CommandText = command.CommandText + ";SELECT  @@IDENTITY";
                        return Convert.ToInt32(database.ExecuteScalar(command));
                    }
                case DatabaseType.IbmDb2:
                    throw new Exception("未实现IbmDb2的数据库操作！");
                    break;
                case DatabaseType.MySQL:
                    throw new Exception("未实现MySQL的数据库操作！");
                    break;
                default:
                    throw new Exception("未实现的数据库操作！");
                    break;
            }
        }

        public override DataTable GetDataTable(string commandtext)
        {
            DataSet ds = null;

            if (isInTransaction)
            {
                command = new SqlCommand(commandtext);
                command.Connection = connection;
                command.Transaction = transaction;
                //command.CommandType = CommandType.Text;

                ds = database.ExecuteDataSet(command, transaction);
            }
            else
            {
                ds = database.ExecuteDataSet(CommandType.Text, commandtext);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            throw new Exception("没有数据");
        }

        public override DataTable GetDataTable(string storeProcedureName, params object[] parameters)
        {
            DataSet ds = null;
            if (isInTransaction)
            {
                command = database.GetStoredProcCommand(storeProcedureName, parameters);
                command.Connection = connection;
                command.Transaction = transaction;
                //command.CommandType = CommandType.Text;

                ds = database.ExecuteDataSet(command, transaction);
            }
            else
            {
                ds = database.ExecuteDataSet(storeProcedureName, parameters);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            throw new Exception("没有数据");
        }

        public override IDataReader GetDataReader(string commandtext)
        {
            if (isInTransaction)
            {
                command = database.GetSqlStringCommand(commandtext);
                command.Connection = connection;
                command.Transaction = transaction;
                //command.CommandType = CommandType.Text;
                return database.ExecuteReader(command, transaction);
            }
            else
            {
                return database.ExecuteReader(CommandType.Text, commandtext);
            }
        }

        /// <summary>
        /// 获取IDbCommand
        /// </summary>
        /// <param name="commandtext"></param>
        /// <returns></returns>
        public override IDbCommand GetCommand(string commandtext)
        {
            return database.GetSqlStringCommand(commandtext);
        }
        /// <summary>
        /// 设置DbCommand的值
        /// </summary>
        /// <param name="cmd">DbCommand对象</param>
        /// <param name="paraName">参数名称</param>
        /// <param name="value">值</param>
        //public override void SetParameterValue(IDbCommand cmd, string paraName, object value)
        //{
        //    //database.AddInParameter((DbCommand)cmd, paraName,System.Data.DbType.Binary);
        //    //database.SetParameterValue((DbCommand)cmd, paraName, value);
        //}


        public override int DoCommand(string commandtext)
        {
            if (isInTransaction)
            {
                command = database.GetSqlStringCommand(commandtext);
                command.Connection = connection;
                command.Transaction = transaction;
                //command.CommandType = CommandType.Text;
                return database.ExecuteNonQuery(command, transaction);
            }
            else
            {
                return database.ExecuteNonQuery(CommandType.Text, commandtext);
            }
        }
        public override int DoCommand(string storeProcedureName, params object[] parameters)
        {
            if (isInTransaction)
            {
                command = database.GetStoredProcCommand(storeProcedureName, parameters);
                command.Connection = connection;
                command.Transaction = transaction;
                //command.CommandType = CommandType.Text;
                return database.ExecuteNonQuery(command, transaction);
            }
            else
            {
                return database.ExecuteNonQuery(storeProcedureName, parameters);
            }
        }

        public override int DoCommand(IDbCommand cmd)
        {
            if (isInTransaction)
            {
                command = (DbCommand)cmd;
                command.Connection = connection;
                command.Transaction = transaction;
                //command.CommandType = CommandType.Text;
                return database.ExecuteNonQuery(command, transaction);
            }
            else
            {
                return database.ExecuteNonQuery((DbCommand)cmd);
            }
        }

        public override object GetDataResult(string commandtext)
        {
            if (isInTransaction)
            {
                command = database.GetSqlStringCommand(commandtext);
                command.Connection = connection;
                command.Transaction = transaction;
                //command.CommandType = CommandType.Text;
                return database.ExecuteScalar(command, transaction);
            }
            else
            {
                return database.ExecuteScalar(CommandType.Text, commandtext);
            }
        }
        public override object GetDataResult(string storeProcedureName, params object[] parameters)
        {
            if (isInTransaction)
            {
                command = database.GetStoredProcCommand(storeProcedureName, parameters);
                command.Connection = connection;
                command.Transaction = transaction;
                //command.CommandType = CommandType.Text;
                return database.ExecuteScalar(command, transaction);
            }
            else
            {
                return database.ExecuteScalar(storeProcedureName, parameters);
            }
        }

        public override DataSet GetDataSet(string storeProcedureName, params object[] parameters)
        {
            DataSet ds = null;

            if (isInTransaction)
            {
                command = database.GetStoredProcCommand(storeProcedureName, parameters);
                command.Connection = connection;
                command.Transaction = transaction;
                //command.CommandType = CommandType.Text;

                ds = database.ExecuteDataSet(command, transaction);
            }
            else
            {
                ds = database.ExecuteDataSet(storeProcedureName, parameters);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            throw new Exception("没有数据");
        }

        public override IDbCommand GetProcCommand(string storeProcedureName)
        {
            return database.GetStoredProcCommand(storeProcedureName);
        }

        public override void AddInParameter(DbCommand command,string name, DbType dbType, object value)
        {
            database.AddInParameter(command, name, dbType, value);
        }

        public override void AddOutParameter(DbCommand command,string name, DbType dbType, int size)
        {
            database.AddOutParameter(command, name, dbType, size);
        }

        public override object GetParameterValue(DbCommand command, string name)
        {
            return database.GetParameterValue(command, name);
        }

        public override DataTable GetDataTable(IDbCommand cmd)
        {
            command = (DbCommand)cmd;
            DataSet ds = null;
            if (isInTransaction)
            {
                command.Connection = connection;
                command.Transaction = transaction;
                ds = database.ExecuteDataSet(command, transaction);
            }
            else
            {
                ds = database.ExecuteDataSet(command);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            throw new Exception("没有数据");
        }

        public override IDataReader GetDataReader(IDbCommand cmd)
        {
            if (isInTransaction)
            {
                command = (DbCommand)cmd;
                command.Connection = connection;
                command.Transaction = transaction;
                //command.CommandType = CommandType.Text;
                return database.ExecuteReader(command, transaction);
            }
            else
            {
                return database.ExecuteReader((DbCommand)cmd);
            }
        }

        public override object GetDataResult(IDbCommand cmd)
        {
            if (isInTransaction)
            {
                command = (DbCommand)cmd;
                command.Connection = connection;
                command.Transaction = transaction;
                //command.CommandType = CommandType.Text;
                return database.ExecuteScalar(command, transaction);
            }
            else
            {
                return database.ExecuteScalar((DbCommand)cmd);
            }
        }

        public override IEnumerable<dynamic> Query(string sql, object param)
        {
            if (isInTransaction)
            {
                return connection.Query(PluginName, sql, param, transaction);
            }
            else {
                DbConnection _connection = database.CreateConnection();
                return _connection.Query(PluginName, sql, param);
            }
        }
        public override IEnumerable<T> Query<T>(string sql, object param)
        {
            if (isInTransaction)
            {
                return connection.Query<T>(PluginName, sql, param, transaction, true);
            }
            else {
                DbConnection _connection = database.CreateConnection();
                return _connection.Query<T>(PluginName, sql, param, null, true, null, null);
            }
        }


        public override IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param)
        {
            if (isInTransaction)
            {
                return connection.Query<TFirst, TSecond, TReturn>(PluginName, sql, map, param, transaction, true, "Id");
            }
            else {
                DbConnection _connection = database.CreateConnection();
                return _connection.Query<TFirst, TSecond, TReturn>(PluginName, sql, map, param, null, true, "Id", null, null);
            }
        }

        public override IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param)
        {
            if (isInTransaction)
            {
                return connection.Query<TFirst, TSecond, TThird, TReturn>(PluginName, sql, map, param, transaction, true, "Id");
            }
            else {
                DbConnection _connection = database.CreateConnection();
                return _connection.Query<TFirst, TSecond, TThird, TReturn>(PluginName, sql, map, param, null, true, "Id", null, null);
            }
        }

        public override IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param)
        {
            if (isInTransaction)
            {
                return connection.Query<TFirst, TSecond, TThird, TFourth, TReturn>(PluginName, sql, map, param, transaction, true, "Id");
            }
            else {
                DbConnection _connection = database.CreateConnection();
                return _connection.Query<TFirst, TSecond, TThird, TFourth, TReturn>(PluginName, sql, map, param, null, true, "Id", null, null);
            }
        }

        public override EFWCoreLib.CoreFrame.Orm.SqlMapper.GridReader QueryMultiple(string sql, object param)
        {
            if (isInTransaction)
            {
                return connection.QueryMultiple(sql, param, transaction);
            }
            else {
                DbConnection _connection = database.CreateConnection();
                return _connection.QueryMultiple(sql, param, null, null, null);
            }
        }
    }
}
