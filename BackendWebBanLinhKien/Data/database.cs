using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace BackendWebBanLinhKien.Data
{
    public class database
    {
        private static readonly string str = "Data Source=LAPTOP-TDQU694B\\SQLEXPRESS;Initial Catalog=QLBanLinhKien;Integrated Security=True;Trust Server Certificate=True";
        private SqlConnection ketnoi = null;
        private SqlCommand cmd = null;
        
        public database()
        {
            ketnoi = new SqlConnection(str);
            cmd = new SqlCommand();
            cmd.Connection= ketnoi;
        }

        public async Task<DataTable> GetData(string truyvan,params SqlParameter[] param)
        {
            try
            {
                await ketnoi.OpenAsync();
                DataTable data = new DataTable();
                cmd.CommandText = truyvan;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);

                return data;
            }
            catch (Exception ex) {
                throw ex;
            }
            finally
            {
                ketnoi.Close();
            }
        }

        public async Task<bool> ThaoTac(string truyvan,params SqlParameter[] param)
        {
            try
            {
                await ketnoi.OpenAsync();
                cmd.CommandText = truyvan;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                cmd.ExecuteNonQuery();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                ketnoi.Close();
            }
        }

    }
}
