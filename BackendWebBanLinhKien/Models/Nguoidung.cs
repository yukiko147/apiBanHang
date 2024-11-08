using BackendWebBanLinhKien.Data;
using BackendWebBanLinhKien.Hepler;
using BackendWebBanLinhKien.Service;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BackendWebBanLinhKien.Models
{
    public class Nguoidung:IThaoTac<Nguoidung>
    {
        public string id {  get; set; }="ND_"+Guid.NewGuid().ToString().Substring(0,8);
        public string email { get; set; }
        public string matkhau {  get; set; }
        public string hoten {  get; set; }
        public int quyen {  get; set; }
        public int trangthai { get; set; }
        public int them {  get; set; }
        public int sua {  get; set; }
        public int xoa {  get; set; }
        public DateTime ngaylap { get; set; } = DateTime.UtcNow;
        public DateTime ngaysua { get; set; } = DateTime.Now;

        private database Db = null;
        private DataTable data = null;
        public Nguoidung()
        {
            Db=new database();
            data= new DataTable();
        }

        public async Task<List<Nguoidung>> GetAll()
        {
            string truyvan = "GetAllNguoiDung";
            data = await Db.GetData(truyvan);
            List<Nguoidung> lst = new List<Nguoidung>();
            foreach (DataRow dr in data.Rows)
            {
                Nguoidung n = new Nguoidung()
                {
                    id = dr["Id"].ToString(),
                    email = dr["Email"].ToString(),
                    matkhau = dr["Matkhau"].ToString(),
                    hoten = dr["Hoten"].ToString(),
                    quyen = int.Parse(dr["Quyen"].ToString()),
                    trangthai = int.Parse(dr["Trangthai"].ToString()),
                    them = int.Parse(dr["Them"].ToString()),
                    sua = int.Parse(dr["Sua"].ToString()),
                    xoa = int.Parse(dr["Xoa"].ToString()),
                    ngaylap = DateTime.Parse(dr["NgayLap"].ToString()),
                    ngaysua = DateTime.Parse(dr["NgaySua"].ToString())
                };
                lst.Add(n);
            }
            return lst;
        }

        public async Task<Nguoidung> GetByIdOrName(string Id, string ten)
        {
            foreach(var i in await this.GetAll())
            {
                if(string.Compare(Id,i.id,true)==0 || string.Compare(ten, i.email, true) == 0)
                {
                    return i;
                }
            }
            return null;
        }

        public async Task<bool> Them(Nguoidung Model)
        {
            string truyvan = "ThemNguoiDung";
            bool ck = await Db.ThaoTac(truyvan,new SqlParameter("@id", Model.id),
                                                new SqlParameter("@email", Model.email),
                                                new SqlParameter("@matkhau", Model.matkhau),
                                                new SqlParameter("@hoten", Model.hoten),
                                                new SqlParameter("@quyen", Model.quyen),
                                                new SqlParameter("@trangthai", Model.trangthai),
                                                new SqlParameter("@them", Model.them),
                                                new SqlParameter("@sua", Model.sua),
                                                new SqlParameter("@xoa", Model.xoa));
            return ck;
        }

        public async Task<bool> Sua(Nguoidung Model, string Id)
        {
            string truyvan = "proc_SuaNguoidung";
            bool ck = await Db.ThaoTac(truyvan, new SqlParameter("@id", Id),
                                    new SqlParameter("@matkhau", Model.matkhau),
                                    new SqlParameter("@hoten", Model.hoten),
                                    new SqlParameter("@quyen", Model.quyen),
                                    new SqlParameter("@trangthai", Model.trangthai),
                                    new SqlParameter("@them", Model.them),
                                    new SqlParameter("@sua", Model.sua),
                                    new SqlParameter("@xoa", Model.xoa));
            return ck; 
        }

        public async Task<bool> Xoa(string Id)
        {
            bool ck = true;
            Hoadon _hd = new Hoadon();
            foreach (var i in await _hd.GetAll())
            {
                if (string.Compare(Id, i.id_taikhoan, true) == 0)
                {
                    ck = await _hd.Xoa(i.id);
                }
            }
            string truyvan = "proc_XoaNguoidung";
            ck = await Db.ThaoTac(truyvan, new SqlParameter("@id", Id));
            return ck;
        }

        public async Task<Nguoidung> DangNhap(dangnhap login)
        {
            List<Nguoidung> lst = await this.GetAll();
            foreach(var item in lst)
            {
                if(string.Compare(item.email,login.email,true)==0 && string.Compare(item.matkhau,login.password,true)==0)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
