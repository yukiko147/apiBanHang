using BackendWebBanLinhKien.Data;
using BackendWebBanLinhKien.Service;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BackendWebBanLinhKien.Models
{
    public class Danhmuc:IThaoTac<Danhmuc>
    {
        public string id { get; set; }="DM_"+Guid.NewGuid().ToString().Substring(0,8);
        public string tendanhmuc {  get; set; }

        private database Db = null;
        private DataTable data = null;
        public Danhmuc()
        {
            Db = new database();
            data = new DataTable();
        }
       
        public async Task<List<Danhmuc>> GetAll()
        {
            string truyvan = "proc_GetAllDanhMuc";
            data = await Db.GetData(truyvan);
            List<Danhmuc> lst=new List<Danhmuc>();
            foreach(DataRow dr in data.Rows)
            {
                Danhmuc d = new Danhmuc()
                {
                    id = dr["Id"].ToString(),
                    tendanhmuc = dr["Tendanhmuc"].ToString()
                };
                lst.Add(d);
            }
            return lst;
        }

        public async Task<Danhmuc> GetByIdOrName(string Id, string ten)
        {
            foreach (var i in await this.GetAll())
            {
                if (string.Compare(Id, i.id, true) == 0)
                {
                    return i;
                }
            }
            return null;
        }

        public async Task<bool> Sua(Danhmuc Model, string Id)
        {
            string truyvan = "proc_SuaDanhMuc";
            bool ck = await Db.ThaoTac(truyvan, new SqlParameter("@id", Id), 
                                                new SqlParameter("@tendanhmuc", Model.tendanhmuc));
            return ck;
        }

        public async Task<bool> Them(Danhmuc Model)
        {
            string truyvan = "proc_ThemDanhMuc";
            SqlParameter[] parm =new[]
            {
                new SqlParameter("@id",Model.id),
                new SqlParameter("@tendanhmuc",Model.tendanhmuc)
            };
            bool ck = await Db.ThaoTac(truyvan, parm);
            return ck;
        }

        public async Task<bool> Xoa(string Id)
        {
            bool ck = true;
            Sanpham _sp=new Sanpham();
            foreach (var i in await _sp.GetAll())
            {
                if(string.Compare(Id, i.id_danhmuc, true)==0)
                {
                    ck = await _sp.Xoa(i.id);
                }
            }
            string truyvan = "proc_XoaDanhmuc";
            ck = await Db.ThaoTac(truyvan, new SqlParameter("@id", Id));
            return ck;
        }
    }
}
