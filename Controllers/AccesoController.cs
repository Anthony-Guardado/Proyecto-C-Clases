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
        public async Task<IActionResult> Index(InfoLogin infoLogin)
        {
            if (infoLogin != null)
            {
                SHA256 mySHA256 = SHA256.Create();
                byte[] datos = Encoding.UTF8.GetBytes(infoLogin.Password);
                byte[] hashValue = mySHA256.ComputeHash(datos);
                string hashValueHexadecimal = BitConverter.ToString(hashValue).Replace("-", "").ToLower();
                //string sql = String.Format("select Id, Login, Password from Usuarios a where Login='{0}' and convert(varchar(256), DecryptByPassPhrase('osni',a.Password)) = '{1}'", infoLogin.Login, infoLogin.Password);
                //Usuario? usuario = _context.Usuarios.FromSqlRaw(sql).FirstOrDefault<Usuario>();

                var usuario = _context.Usuarios
                .Where(a => a.Correo == infoLogin.Login && a.Clave == hashValueHexadecimal)
                .FirstOrDefault();
                //string? usuario = null;

                if (usuario != null)
                {
                    var claims = new List<Claim> {
        new Claim(ClaimTypes.Name, infoLogin.Login)
    };

                    // Traer roles asignados al usuario
                    var lista = (from rls in _context.Roles
                                 join rlsa in _context.RolesAsignados
                                 on rls.Id equals rlsa.RolId
                                 where rlsa.UsuarioId == usuario.Id
                                 select rls).ToList();

                    // Agregar cada rol como claim
                    foreach (var rol in lista)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, rol.Nombre));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity)
                    );

                    return RedirectToAction("Index", "Home");
                }
                else
                {
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

