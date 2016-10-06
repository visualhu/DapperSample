using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public abstract class DapperContext : IContext
    {
        private bool _isTransactionStarted;
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private int? _commandTimeout = null;

        public bool IsTransactionStarted => _isTransactionStarted;

        protected abstract IDbConnection CreateConnection();

        protected DapperContext()
        {
            _connection = CreateConnection();
            _connection.Open();
            DebugPrint("Connection started.");
        }

        private void DebugPrint(string message)
        {
#if DEBUG
            Debug.Print(">>>UnitOfWorkDapper - Thread {0} : {1}", Thread.CurrentThread.ManagedThreadId, message);
#endif
        }

        public void BeginTransaction()
        {
            if (_isTransactionStarted)
                throw new InvalidOperationException("Transaction is already started.");
            _transaction = _connection.BeginTransaction();
            _isTransactionStarted = true;
            DebugPrint("Transaction started.");
        }

        public void Commit()
        {
            if (!_isTransactionStarted)
                throw new InvalidOperationException("No transaction started.");

            _transaction.Commit();
            _transaction = null;

            _isTransactionStarted = false;

            DebugPrint("Transaction commintted.");
        }

        public void Rollback()
        {
            if (!_isTransactionStarted)
                throw new InvalidOperationException("No transaction started.");

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
            _isTransactionStarted = false;

            DebugPrint("Transaction rollbacked and disposed.");
        }

        public int Execute(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return SqlMapper.Execute(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public Task<int> ExecuteAsync(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return SqlMapper.ExecuteAsync(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public IDataReader ExecuteReader(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return SqlMapper.ExecuteReader(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public T ExecuteScalar<T>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return SqlMapper.ExecuteScalar<T>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return SqlMapper.Query<T>(_connection, sql, param, _transaction, true, _commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id", CommandType commandType = CommandType.Text)
        {
            return SqlMapper.Query<TFirst, TSecond, TReturn>(_connection, sql, map, param, _transaction, true, splitOn, _commandTimeout, commandType);
        }

        public async Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return await SqlMapper.QueryAsync(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return await SqlMapper.QueryAsync<T>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return await SqlMapper.QueryFirstOrDefaultAsync<T>(_connection, sql, param, _transaction, _commandTimeout, commandType);
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id", CommandType commandType = CommandType.Text)
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TReturn>(_connection, sql, map, param, _transaction, true, splitOn, _commandTimeout, commandType);
        }

        public void Dispose()
        {
            if (_isTransactionStarted)
                Rollback();

            _connection.Close();
            _connection.Dispose();
            _connection = null;

            DebugPrint("Connection closed and disposed.");
        }
    }
}