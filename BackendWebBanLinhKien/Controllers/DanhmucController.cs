using BackendWebBanLinhKien.Data;
using BackendWebBanLinhKien.Models;
using BackendWebBanLinhKien.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BackendWebBanLinhKien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhmucController : ControllerBase, IThaotacController<Danhmuc, IActionResult>
    {
        private Danhmuc _danhmuc;
        private Token _token;
        public DanhmucController(IOptionsMonitor<Token> opt)
        {
            _token = opt.CurrentValue;
            _danhmuc = new Danhmuc();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Danhmuc model)
        {
            try
            {
                string auth = Request.Headers.Authorization;
                string token = auth["Bearer ".Length..].Trim();

                ClaimsPrincipal claim = _token.GiaiToken(token);
                int ckquyen = int.Parse(claim.FindFirst(c => c.Type == "Quyen").Value);
                if (ckquyen > 0)
                {
                    Danhmuc d = new Danhmuc() {
                        tendanhmuc = model.tendanhmuc,
                    };

                    if (await _danhmuc.Them(d))
                    {
                        return Ok(new
                        {
                            Succes = true,
                            Message = "Thêm danh mục thành công !!!"
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Succes = false,
                            Message = "Thêm danh mục thất bại !!!"
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
                    if (await _danhmuc.Xoa(Id))
                    {
                        return Ok(new
                        {
                            Succes = true,
                            Message = "Xóa danh mục thành công !!!"
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Succes = false,
                            Message = "Xóa danh mục thất bại !!!"
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
        public async Task<IActionResult> Edit(Danhmuc model, string Id)
        {
            try
            {
                string auth = Request.Headers.Authorization;
                string token = auth["Bearer ".Length..].Trim();

                ClaimsPrincipal claim = _token.GiaiToken(token);
                int ckquyen = int.Parse(claim.FindFirst(c => c.Type == "Quyen").Value);
                if (ckquyen > 0)
                {
                    Danhmuc d = new Danhmuc()
                    {
                        tendanhmuc = model.tendanhmuc,
                    };

                    if (await _danhmuc.Sua(d,Id))
                    {
                        return Ok(new
                        {
                            Succes = true,
                            Message = "Thêm danh mục thành công !!!"
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Succes = false,
                            Message = "Thêm danh mục thất bại !!!"
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
                List<Danhmuc> lst = await _danhmuc.GetAll();
                return Ok(new
                {
                    Succes = true,
                    Data = lst
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

        public Task<IActionResult> GetById(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
