using BackendWebBanLinhKien.Data;
using BackendWebBanLinhKien.Hepler;
using BackendWebBanLinhKien.Service;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BackendWebBanLinhKien.Models
{
    public class Sanpham:IThaoTac<Sanpham>
    {
        public string id {  get; set; }="IP_"+Guid.NewGuid().ToString().Substring(0,8);
        public string tensanpham {  get; set; }
        public string thumbnail {  get; set; }
        public int soton {  get; set; }
        public int gia {  get; set; }
        public string gioithieu {  get; set; }
        public string mota {  get; set; }
        public string base64 {  get; set; }
        public string id_danhmuc {  get; set; }

        private database Db = null;
        private DataTable data = null;
        public Sanpham()
        {
            Db = new database();
            data = new DataTable();
        }
        public async Task<List<Sanpham>> GetAll()
        {
            string truyvan = "proc_GetAllSanpham";
            data = await Db.GetData(truyvan);
            List<Sanpham> lst=new List<Sanpham>();
            UploadFile up=new UploadFile();
            foreach(DataRow dr in data.Rows)
            {
                Sanpham s = new Sanpham()
                {
                    id = dr["Id"].ToString(),
                    tensanpham = dr["Tensanpham"].ToString(),
                    thumbnail = dr["Thumbnail"].ToString(),
                    soton = int.Parse(dr["Soton"].ToString()),
                    gia = int.Parse(dr["Gia"].ToString()),
                    gioithieu = dr["Gioithieu"].ToString(),
                    mota = dr["Mota"].ToString(),
                    id_danhmuc = dr["id_danhmuc"].ToString(),
                    //base64 = up.getFileBase64(thumbnail)
                };
                s.base64=up.getFileBase64(s.thumbnail);
                lst.Add(s);
            }
            return lst;
        }

        public async Task<Sanpham> GetByIdOrName(string Id, string ten)
        {
            foreach (Sanpham s in await GetAll()) {
                if (string.Compare(Id, s.id, true) == 0)
                    return s;
            }
            return null;
        }

        public async Task<bool> Sua(Sanpham Model, string Id)
        {
            string truyvan = "proc_SuaSanpham";
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@id",Id),
                new SqlParameter("@tensanpham",Model.tensanpham),
                new SqlParameter("@thumbnail",Model.thumbnail),
                new SqlParameter("@soton",Model.soton),
                new SqlParameter("@gioithieu",Model.gioithieu),
                new SqlParameter("@gia",Model.gia),
                new SqlParameter("@mota",Model.mota),
                new SqlParameter("@id_danhmuc",Model.id_danhmuc)
            };
            bool ck = await Db.ThaoTac(truyvan, parm);
            return ck;
        }

        public async Task<bool> Them(Sanpham Model)
        {
            string truyvan = "proc_ThemSanpham";
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@id",Model.id),
                new SqlParameter("@tensanpham",Model.tensanpham),
                new SqlParameter("@thumbnail",Model.thumbnail),
                new SqlParameter("@soton",Model.soton),
                new SqlParameter("@gioithieu",Model.gioithieu),
                new SqlParameter("@mota",Model.mota),
                new SqlParameter("@gia",Model.gia),
                new SqlParameter("@id_danhmuc",Model.id_danhmuc)
            };
            bool ck = await Db.ThaoTac(truyvan, parm);
            return ck;
        }

        public async Task<bool> Xoa(string Id)
        {

            string truyvan = "proc_XoaSanpham";
            bool ck = await  Db.ThaoTac(truyvan, new SqlParameter("@id",Id));
            return ck;
        }
    }
}
