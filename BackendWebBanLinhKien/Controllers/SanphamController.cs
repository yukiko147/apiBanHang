using BackendWebBanLinhKien.Data;
using BackendWebBanLinhKien.Hepler;
using BackendWebBanLinhKien.Models;
using BackendWebBanLinhKien.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Security.Claims;

namespace BackendWebBanLinhKien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanphamController : ControllerBase, IThaotacController<Sanpham, IActionResult>
    {
        private Sanpham _sanpham;
        private Token _token;
        public SanphamController(IOptionsMonitor<Token> opt)
        {
            _sanpham = new Sanpham();
            _token = opt.CurrentValue;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Sanpham model)
        {
            try
            {
                string auth = Request.Headers.Authorization;
                string token = auth["Bearer ".Length..].Trim();

                ClaimsPrincipal claim = _token.GiaiToken(token);
                int ckquyen = int.Parse(claim.FindFirst(c => c.Type == "Quyen").Value);
                if (ckquyen > 0)
                {
                    Sanpham sp = new Sanpham()
                    {
                        tensanpham = model.tensanpham,
                        thumbnail = model.thumbnail,
                        gia = model.gia,
                        soton = model.soton,
                        gioithieu = model.gioithieu,
                        mota = model.mota,
                        id_danhmuc = model.id_danhmuc,

                    };

                    if (await _sanpham.Them(sp))
                    {
                        return Ok(new
                        {
                            Succes = true,
                            Message = "Thêm sản phẩm thành công !!!"
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Succes = false,
                            Message = "Thêm sản phẩm thất bại !!!"
                        });
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        Succes = false,
                        Message = "Người dùng không có quyền"
                    });
                }
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

        [HttpDelete("{Id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                string auth = Request.Headers.Authorization;
                string token = auth["Bearer ".Length..].Trim();

                ClaimsPrincipal claim = _token.GiaiToken(token);
                int ckquyen = int.Parse(claim.FindFirst(c => c.Type == "Quyen").Value);
                if (ckquyen > 0)
                {
                    if (await _sanpham.Xoa(Id))
                    {
                        return Ok(new
                        {
                            Succes = true,
                            Message = "Xóa sản phẩm thành công !!!"
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Succes = false,
                            Message = "Xóa sản phẩm thất bại !!!"
                        });
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        Succes = false,
                        Message = "Người dùng không có quyền"
                    });
                }
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

        [HttpPut("{Id}")]
        [Authorize]
        public async Task<IActionResult> Edit(Sanpham model, string Id)
        {
            try
            {
                string auth = Request.Headers.Authorization;
                string token = auth["Bearer ".Length..].Trim();

                ClaimsPrincipal claim = _token.GiaiToken(token);
                int ckquyen = int.Parse(claim.FindFirst(c => c.Type == "Quyen").Value);
                if (ckquyen > 0)
                {
                    Sanpham sp = new Sanpham()
                    {
                        tensanpham = model.tensanpham,
                        thumbnail = model.thumbnail,
                        gia = model.gia,
                        soton = model.soton,
                        gioithieu = model.gioithieu,
                        mota = model.mota,
                        id_danhmuc = model.id_danhmuc,
                    };

                    if (await _sanpham.Sua(sp, Id))
                    {
                        return Ok(new
                        {
                            Succes = true,
                            Message = "Sửa sản phẩm thành công !!!"
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Succes = false,
                            Message = "Sửa sản phẩm thất bại !!!"
                        });
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        Succes = false,
                        Message = "Người dùng không có quyền"
                    });
                }
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
                List<Sanpham> lst = await _sanpham.GetAll();
                return Ok(new
                {
                    Succes = true,
                    Data = lst,

                });
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

        [HttpPost("uploadImg")]
        public async Task<IActionResult> upHinh(IFormFile file)
        {
            try
            {
                UploadFile up = new UploadFile();
                up.Upload(file);
                return Ok(new
                {
                    Succes = true,
                    Message = "Them hinh thanh cong"
                });
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

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            try
            {
                Sanpham sp = await _sanpham.GetByIdOrName(Id, "");
                return Ok(new
                {
                    Succes = true,
                    Data = sp
                });
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

        //[HttpGet("file/{fileName}")]
        //public IActionResult GetImage(string fileName)
        //{
        //    try
        //    {
        //        var path = Path.Combine(Directory.GetCurrentDirectory(), "Img", fileName);
        //        if (!System.IO.File.Exists(path))
        //            return NotFound();

        //        var imageBytes = System.IO.File.ReadAllBytes(path);
        //        var base64String = Convert.ToBase64String(imageBytes);
        //        return Ok(new { Image = base64String });

        //    }
        //    catch (Exception ex) {
        //        return BadRequest(new
        //        {
        //            Succes = false,
        //            Message = ex.Message.ToString(),
        //        });
        //    }
        //}
    }
}
