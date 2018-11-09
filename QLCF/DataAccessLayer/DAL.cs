﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DAL
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataAdapter adp;

        string strConnect =
            "Data Source=(local); Initial Catalog=QLCF; Integrated Security=True";

        public DAL()
        {
            cnn = new SqlConnection(strConnect);
            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            cnn.Open();
            cmd = cnn.CreateCommand();
        }
        public DataSet ExecuteQueryDataSet( string strSQL, CommandType ct, params SqlParameter[] param)
        {
            cmd.CommandText = strSQL;
            cmd.CommandType = ct;
            adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            if (param != null)
                foreach (SqlParameter p in param)
                    cmd.Parameters.Add(p);
            adp.Fill(ds);
            return ds;
        }
        public bool MyExecuteNonQuery(string strSQL, CommandType ct, ref string error, params SqlParameter[] param)
        {
            bool f = false;
            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            cnn.Open();
            cmd.Parameters.Clear();
            cmd.CommandText = strSQL;
            cmd.CommandType = ct;
            foreach (SqlParameter p in param)
                cmd.Parameters.Add(p);
            try
            {
                cmd.ExecuteNonQuery();
                f = true;
            }
            catch (SqlException ex)
            {
                error = ex.Message;
            }
            finally
            {
                cnn.Close();
            }
            return f;
        }
        public object MyExecuteScalar(string strSQL, CommandType ct, ref string error, params SqlParameter[] param)
        {
            object o = new object();
            if (cnn.State == ConnectionState.Open)
                cnn.Close();
            cnn.Open();
            if (param != null)
                foreach (SqlParameter p in param)
                    cmd.Parameters.Add(p);
            try
            {
                cmd = new SqlCommand(strSQL, cnn);
                o = cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                error = ex.Message;
            }
            cnn.Close();
            return o;
        }
    }
}
