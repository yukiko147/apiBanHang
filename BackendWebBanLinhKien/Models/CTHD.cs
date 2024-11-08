using BackendWebBanLinhKien.Data;
using BackendWebBanLinhKien.Service;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BackendWebBanLinhKien.Models
{
    public class CTHD:IThaoTac<CTHD>
    {
        public string id {  get; set; }="id_"+Guid.NewGuid().ToString().Substring(0,8);
        public string id_hoadon {  get; set; }
        public string id_sanpham {  get; set; }
        public int dongia { get; set; }
        public int thanhtien {  get; set; }
        public int slhang {  get; set; }

        private database db = null;
        private DataTable data = null;

        public CTHD() { 
            db = new database();
            data = new DataTable();
        }

        public async Task<List<CTHD>> GetAll()
        {
            List<CTHD> list = new List<CTHD>();
            data = await db.GetData("proc_cthd");
            foreach(DataRow dr in data.Rows)
            {
                CTHD cthd=new CTHD() {
                    id = dr["id"].ToString(),
                    id_hoadon = dr["id_hoadon"].ToString(),
                    id_sanpham = dr["id_sanpham"].ToString(),
                    dongia = int.Parse(dr["Dongia"].ToString()),
                    thanhtien = int.Parse(dr["Thanhtien"].ToString()),
                    slhang = int.Parse(dr["SLhang"].ToString())
                };
                list.Add(cthd);
            }
            return list;
        }

        public async Task<CTHD> GetByIdOrName(string Id, string ten)
        {
           foreach(var i in await this.GetAll())
            {
                if(string.Compare(i.id,id,true)==0)
                {
                    return i;
                }
            }
            return null;
        }

        public async Task<bool> Them(CTHD Model)
        {
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@id",Model.id),
                new SqlParameter("@id_hoadon",Model.id_hoadon),
                new SqlParameter("@id_sanpham",Model.id_sanpham),
                new SqlParameter("@dongia",Model.dongia),
                new SqlParameter("@slhang",Model.slhang)
            };
            string truyvan = "proc_themCTHD";
            return await db.ThaoTac(truyvan,parm);
        }

        public async Task<bool> Sua(CTHD Model, string Id)
        {
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@id",Id),
                new SqlParameter("@id_hoadon",Model.id_hoadon),
                new SqlParameter("@id_sanpham",Model.id_sanpham),
                new SqlParameter("@dongia",Model.dongia),
                new SqlParameter("@slhang",Model.slhang)
            };
            string truyvan = "proc_suaCTHD";
            return await db.ThaoTac(truyvan, parm);
        }

        public async Task<bool> Xoa(string Id)
        {
            return await db.ThaoTac("proc_XoaCTHD", new SqlParameter("@id",Id));
        }
    }
}
