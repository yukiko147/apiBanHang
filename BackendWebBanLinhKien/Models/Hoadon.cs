using BackendWebBanLinhKien.Data;
using BackendWebBanLinhKien.Service;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BackendWebBanLinhKien.Models
{
    public class Hoadon:IThaoTac<Hoadon>
    {
        public string id {  get; set; }="HD_"+Guid.NewGuid().ToString().Substring(0,8);
        public string id_taikhoan {  get; set; }
        public string ptthanhtoan {  get; set; }
        public int tonghang {  get; set; }
        public int tongtien {  get; set; }
        public string diachinhan {  get; set; }
        public string sdt {  get; set; }
        public int trangthai { get; set; }
        public DateTime ngaylaphd { get; set; } = DateTime.UtcNow;

        private database Db = null;
        private DataTable data = null;
        public Hoadon()
        {
            Db=new database();
            data = new DataTable();
        }

        public async Task<List<Hoadon>> GetAll()
        {
            List<Hoadon> lst = new List<Hoadon>();
            string truyvan = "proc_layHoadon";
            data = await Db.GetData(truyvan);
            foreach (DataRow dr in data.Rows)
            {
                Hoadon h = new Hoadon()
                {
                    id = dr["id"].ToString(),
                    id_taikhoan = dr["id_taikhoan"].ToString(),
                    ptthanhtoan = dr["PTThanhToan"].ToString(),
                    tonghang = int.Parse(dr["Tonghang"].ToString()),
                    tongtien = int.Parse(dr["Tongtien"].ToString()),
                    diachinhan = dr["Diachinhan"].ToString(),
                    trangthai = int.Parse(dr["Trangthai"].ToString()),
                    sdt = dr["Sdt"].ToString(),
                    ngaylaphd = DateTime.Parse(dr["NgaylapHd"].ToString())
                };
                lst.Add(h);
            }
            return lst;
        }

        public async Task<Hoadon> GetByIdOrName(string Id, string ten)
        {
            List<Hoadon> lst = await this.GetAll();
            foreach (Hoadon h in lst)
            {
                if (string.Compare(id, h.id, true) == 0)
                {
                    return h;
                }
            }
            return null;
        }

        public async Task<bool> Sua(Hoadon Model, string Id)
        {
            string truyvan = "proc_suahoadon";
            SqlParameter[] parm = new SqlParameter[] {
                    new SqlParameter("@id", id),
                    new SqlParameter("@tranthai",Model.trangthai)
                };
            if (await Db.ThaoTac(truyvan, parm))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Them(Hoadon Model)
        {
            string truyvan = "proc_themhoadon";
            SqlParameter[] parm = new SqlParameter[] {
                    new SqlParameter("@id", Model.id),
                    new SqlParameter("@id_taikhoan", Model.id_taikhoan),
                    new SqlParameter("@ptthanhtoan", Model.ptthanhtoan),
                    new SqlParameter("@tonghang", Model.tonghang),
                    new SqlParameter("@tongtien", Model.tongtien),
                    new SqlParameter("@trangthai", Model.trangthai),
                    new SqlParameter("@diachinhan", Model.diachinhan),
                    new SqlParameter("@sdt", Model.sdt)
                };
            if (await Db.ThaoTac(truyvan, parm))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Xoa(string Id)
        {
            bool ck = true;
            CTHD _cthd = new CTHD();
            foreach (var i in await _cthd.GetAll())
            {
                if (string.Compare(Id, i.id_hoadon, true) == 0)
                {
                    ck = await _cthd.Xoa(i.id);
                }
            }
            string truyvan = "proc_xoahoadon";
            ck = await Db.ThaoTac(truyvan, new SqlParameter("@id", Id));
            return ck;
        }
    }
}
