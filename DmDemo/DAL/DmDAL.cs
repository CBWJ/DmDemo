﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dm;
using System.Configuration;

namespace DMAccess
{
    public class DmDAL : IDALOperation, IDisposable
    {
        public string ConnectionString { get; set; }
        private DmConnection conn;
        public DmDAL()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DmConnection"].ConnectionString;
            Open();
        }
        public DmDAL(string connString)
        {
            ConnectionString = connString;
            Open();
        }
        public void Close()
        {
            if (conn == null) return;
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
            }
        }

        public void Dispose()
        {
            Close();
            conn.Dispose();
        }

        public int ExecuteNonQuery(string sqlText)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = sqlText;
            return cmd.ExecuteNonQuery();
        }

        public int ExecuteProcedureNonQuery(string sqlText, params DbParameter[] parameters)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sqlText;
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteNonQuery();
        }

        public DataTable ExecuteQuery(string sqlText)
        {
            DataTable dataTable = new DataTable();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sqlText;
            var adapter = new DmDataAdapter(cmd);
            adapter.Fill(dataTable);
            return dataTable;
        }

        public object ExecuteScalar(string sqlText)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = sqlText;
            return cmd.ExecuteScalar();
        }

        public void Open()
        {
            conn = new DmConnection();
            conn.ConnectionString = ConnectionString;
            conn.Open();
        }
    }
}
