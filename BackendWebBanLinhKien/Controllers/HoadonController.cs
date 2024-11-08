using BackendWebBanLinhKien.Data;
using BackendWebBanLinhKien.Models;
using BackendWebBanLinhKien.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BackendWebBanLinhKien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoadonController : ControllerBase,IThaotacController<Hoadon,IActionResult>
    {
        private readonly Hoadon _hoadon;
        private readonly Token _token;
        public HoadonController(IOptionsMonitor<Token> opt)
        {
            _hoadon = new Hoadon();
            _token = opt.CurrentValue;
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Hoadon model)
        {
            try
            {
                Hoadon h = new Hoadon();
                h.id_taikhoan = model.id_taikhoan;
                h.ptthanhtoan = model.ptthanhtoan;
                h.tonghang = model.tonghang;
                h.tongtien = model.tongtien;
                h.diachinhan = model.diachinhan;
                h.sdt = model.sdt;
                h.trangthai = model.trangthai;

                if (await _hoadon.Them(h))
                    return Ok(new
                    {
                        Succes = true,
                        Message = "Thêm hóa đơn thành công !!!",
                        Data = h
                    });
                else
                    return BadRequest(new { Succes = false, Message = "Thêm hóa đơn thất bại !!!" });
            } catch (Exception ex) {
                return BadRequest(new { 
                    Succes=false,
                    Message = ex.Message.ToString()
                });
            }
        }

        [HttpDelete("{Id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                if(await _hoadon.Xoa(Id))
                {
                    return Ok(new
                    {
                        Succes = true,
                        Message = "Xóa hóa đơn thành công !!!"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Succes = false,
                        Message = "Xóa người dùng thất bại !!!"
                    });
                }
            }catch(Exception ex)
            {
                return BadRequest(new
                {
                    Succes=false,
                    Message= ex.Message.ToString()
                });
            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Edit(Hoadon model, string Id)
        {
            try
            {
                Hoadon h = new Hoadon();
                h.id_taikhoan = model.id_taikhoan;
                h.ptthanhtoan = model.ptthanhtoan;
                h.tonghang = model.tonghang;
                h.tongtien = model.tongtien;
                h.diachinhan = model.diachinhan;
                h.sdt = model.sdt;
                h.trangthai = model.trangthai;

                if (await _hoadon.Sua(h,Id))
                    return Ok(new
                    {
                        Succes = true,
                        Message = "Sửa hóa đơn thành công !!!",
                        Data = h
                    });
                else
                    return BadRequest(new { Succes = false, Message = "Sửa hóa đơn thất bại !!!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Succes = false,
                    Message = ex.Message.ToString()
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Hoadon> lst = new List<Hoadon>();
                lst=await _hoadon.GetAll();
                return Ok(new {
                    Succes=true,
                    Data=lst
                });
            }
            catch (Exception ex) {
                return BadRequest(new
                {
                    Succes=false,
                    Message = ex.Message.ToString()
                });
            }
        }

        public Task<IActionResult> GetById(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
