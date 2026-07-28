using InventaMeCF.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InventaMeCF.Controllers
{
    public class AccesoController : Controller
    {
        private readonly InventaMeCFContext _context;

        public AccesoController(InventaMeCFContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Index(InfoLogin infoLogin)
        {
            if (infoLogin != null)
            {
                // Generar hash SHA256 de la contraseña
                using SHA256 mySHA256 = SHA256.Create();
                byte[] datos = Encoding.UTF8.GetBytes(infoLogin.Password);
                byte[] hashValue = mySHA256.ComputeHash(datos);
                string hashValueHexadecimal = BitConverter.ToString(hashValue).Replace("-", "").ToLower();

                // Traer usuario con roles asignados
                var usuario = _context.Usuarios
                    .Include(u => u.RolesAsignados)
                    .ThenInclude(ra => ra.Rol)
                    .FirstOrDefault(u => u.Correo == infoLogin.Login && u.Clave == hashValueHexadecimal);

                if (usuario != null)
                {
                    var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, usuario.Nombre ?? infoLogin.Login)
            };

                    // Agregar roles como claims
                    foreach (var rolAsignado in usuario.RolesAsignados)
                    {
                        if (rolAsignado.Rol != null)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, rolAsignado.Rol.Nombre));
                        }
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Usuario no encontrado
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Acceso");
        }
    }
}

