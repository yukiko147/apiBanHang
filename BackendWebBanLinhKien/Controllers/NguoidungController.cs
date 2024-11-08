using BackendWebBanLinhKien.Data;
using BackendWebBanLinhKien.Hepler;
using BackendWebBanLinhKien.Models;
using BackendWebBanLinhKien.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;

namespace BackendWebBanLinhKien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoidungController : ControllerBase, IThaotacController<Nguoidung, IActionResult>
    {
        private Nguoidung _nguoidung;
        private readonly Token _token;
        public NguoidungController(IOptionsMonitor<Token> opt)
        {
            _token = opt.CurrentValue;
            _nguoidung = new Nguoidung();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(dangnhap login)
        {
            try
            {
                if (login.email == string.Empty || login.password == string.Empty)
                    return BadRequest(new
                    {
                        Succes = false,
                        Message = " Chưa nhập email hoặc mật khẩu !!! "
                    });
                else
                {
                    Nguoidung ck = await _nguoidung.DangNhap(login);
                    if (ck != null)
                    {
                        if (ck.trangthai == 0)
                        {
                            return BadRequest(new
                            {
                                Succes = false,
                                Message = " Tài khoản người dùng đã bị khóa !!! "
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                Succes = true,
                                Message = " Đăng nhập thành công !!! ",
                                JwtToken = _token.TaoToken(ck),
                                Tentaikhoan = ck.hoten,
                                Quyen = ck.quyen,
                                Id = ck.id
                            });
                        }
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Succes = false,
                            Message = " Đăng nhập thất bại !!! "
                        });
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Succes = false,
                    Message = e.Message,
                });
            }
        }

        [HttpPost("dangky")]
        public async Task<IActionResult> Dangky(dangky dangky)
        {
            try
            {
                Nguoidung ck = await _nguoidung.GetByIdOrName("", dangky.email);
                if (ck != null)
                    return BadRequest(new
                    {
                        Succes = false,
                        Message = " Email đã tồn tại !!! "
                    });
                else
                {
                    ck = new Nguoidung()
                    {
                        email = dangky.email,
                        matkhau = dangky.matkhau,
                        hoten = dangky.tendaydu,
                        quyen = 0,
                        trangthai = 1,
                        them = 0,
                        sua = 0,
                        xoa = 0
                    };

                    if (await _nguoidung.Them(ck))
                        return Ok(new
                        {
                            Succes = true,
                            Message = " Đang ký thành công !!! "
                        });
                    else
                        return BadRequest(new
                        {
                            Succes = false,
                            Message = " Đăng ký thất bại "
                        });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Succes = false,
                    Message = e.Message,
                });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                string auth = Request.Headers.Authorization;
                string token = auth["Bearer ".Length..].Trim();

                ClaimsPrincipal claim = _token.GiaiToken(token);
                int ckquyen = int.Parse(claim.FindFirst(c => c.Type == "Quyen").Value);
                if (ckquyen > 0)
                {
                    List<Nguoidung> lst = await _nguoidung.GetAll();
                    return Ok(new
                    {
                        Succes = true,
                        Data = lst
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Succes = false,
                        Message = "Người dùng không có quyền !!!"
                    });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Succes = false,
                    Message = e.Message,
                });
            }
        }

        [HttpGet("{Id}")]
        [Authorize]
        public async Task<IActionResult> GetById(string Id)
        {
            try
            {
                Nguoidung nguoidung = await _nguoidung.GetByIdOrName(Id, "");
                return Ok(new
                {
                    Succes = true,
                    Data = nguoidung
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Succes = false,
                    Message = e.Message.ToString()
                });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Nguoidung model)
        {
            try
            {
                string auth = Request.Headers.Authorization;
                string token = auth["Bearer ".Length..].Trim();

                ClaimsPrincipal claim = _token.GiaiToken(token);
                int ckThem = int.Parse(claim.FindFirst(c => c.Type == "Them")?.Value);
                int ckquyen = int.Parse(claim.FindFirst(c => c.Type == "Quyen").Value);
                if (ckThem > 0 && ckquyen > 0)
                {
                    Nguoidung ck = await _nguoidung.GetByIdOrName("", model.email);
                    if (ck != null)
                        return BadRequest(new
                        {
                            Succes = false,
                            Message = " Email đã tồn tại !!! "
                        });
                    else
                    {
                        ck = new Nguoidung()
                        {
                            email = model.email,
                            matkhau = model.matkhau,
                            hoten = model.hoten,
                            quyen = model.quyen,
                            trangthai = model.trangthai,
                            them = model.them,
                            sua = model.sua,
                            xoa = model.xoa,
                        };

                        if (await _nguoidung.Them(ck) == true)
                            return Ok(new
                            {
                                Succes = true,
                                Message = " Thêm người dùng thành công !!! ",
                                DataAdd = ck
                            });
                        else
                            return BadRequest(new
                            {
                                Succes = false,
                                Message = " Thêm người dùng thất bại !!! "
                            });
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        Succes = false,
                        Message = " Người dùng chưa được cấp quyền !!! "
                    });
                }

            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Succes = false,
                    Message = e.Message.ToString(),
                });
            }
        }

        [HttpPut("{Id}")]
        [Authorize]
        public async Task<IActionResult> Edit(Nguoidung model, string Id)
        {
            try
            {
                string auth = Request.Headers.Authorization;
                string token = auth["Bearer ".Length..].Trim();

                ClaimsPrincipal claim = _token.GiaiToken(token);
                int ckSua = int.Parse(claim.FindFirst(c => c.Type == "Sua")?.Value);
                int ckquyen = int.Parse(claim.FindFirst(c => c.Type == "Quyen").Value);
                if (ckSua > 0 && ckquyen > 0)
                {
                    Nguoidung ck = new Nguoidung()
                    {
                        matkhau = model.matkhau,
                        quyen = model.quyen,
                        hoten = model.hoten,
                        trangthai = model.trangthai,
                        them = model.them,
                        sua = model.sua,
                        xoa = model.xoa,
                    };
                    if (await _nguoidung.Sua(ck, Id) == true)
                        return Ok(new
                        {
                            Succes = true,
                            Message = " Sửa người dùng thành công !!! "
                        });
                    else
                        return BadRequest(new
                        {
                            Succes = false,
                            Message = " Sửa thất bại !!! "
                        });
                }
                else
                {
                    return BadRequest(new
                    {
                        Succes = false,
                        Message = " Người dùng chưa được cấp quyền !!! "
                    });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Succes = false,
                    Message = e.Message.ToString(),
                });
            }
        }

        [HttpDelete("{Id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                string auth = Request.Headers.Authorization;
                string token = auth["Bearer ".Length..].Trim();

                ClaimsPrincipal claim = _token.GiaiToken(token);
                int ckXoa = int.Parse(claim.FindFirst(c => c.Type == "Xoa")?.Value);
                int ckquyen = int.Parse(claim.FindFirst(c => c.Type == "Quyen").Value);
                string IdToken = claim.FindFirst(c => c.Type == "Id").Value;
                if (ckXoa > 0 && ckquyen > 0 && string.Compare(Id, IdToken, true) > 0)
                {
                    if (await _nguoidung.Xoa(Id) == true)
                    {
                        return Ok(new
                        {
                            Succes = true,
                            Message = " Xóa người dùng thành công !!! "
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Succes = false,
                            Message = " Xóa người dùng thất bại !!!"
                        });
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        Succes = false,
                        Message = " Người dùng chưa được cấp quyền !!! "
                    });
                }

            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Succes = false,
                    Message = e.Message.ToString(),
                });
            }
        }

        [HttpPut("/thongtin/{Id}")]
        [Authorize]
        public async Task<IActionResult> Thongtinsua(capnhattaikhoan model, string Id)
        {
            try
            {
                Nguoidung n = new Nguoidung();
                n.matkhau = model.matkhau;
                n.hoten = model.hoten;

                if (await _nguoidung.Sua(n, Id))
                {
                    return Ok(new
                    {
                        Succes = true,
                        Message = "Sửa thành công !!!"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Succes = false,
                        Message = "Sửa thất bại !!!"
                    });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Succes = false,
                    Message = e.Message.ToString()
                });
            }
        }
    }
}