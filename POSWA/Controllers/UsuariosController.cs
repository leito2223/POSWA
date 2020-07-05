using POSWA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace POSWA.Controllers
{
    public class UsuariosController: ApiController
    {
        POSContext db = new POSContext();
        object resp;
        public HttpResponseMessage GetUsuarios()
        {
          var user =  db.Usuarios.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        public IHttpActionResult GetOneUsuario(string email, string password)
        {
            var user = db.Usuarios.Where(a => a.Nombre == email).FirstOrDefault();
           
            if (user == null)
            {
                return Unauthorized();
            }
            if (!user.Activo.Value)
            {
                return Unauthorized();
            }
            if (!BCrypt.Net.BCrypt.Verify(password, user.Clave))
            {
                return Unauthorized();
            }

           

           // var token = TokenGenerator.GenerateTokenJwt(user.Nombre, "Admin");

            var obj = new
            {
               user
            };
            return Ok(obj);
        }
        public HttpResponseMessage Post([FromBody] Usuarios cliente)
        {
            try
            {
                Usuarios Consulta = db.Usuarios.Where( a => a.Nombre.ToLower().Contains(cliente.Nombre.ToLower())).FirstOrDefault();

                if (Consulta == null)
                {
                    Usuarios Usr = new Usuarios();


                    Usr.Nombre = cliente.Nombre;
               

                    Usr.Clave = BCrypt.Net.BCrypt.HashPassword(cliente.Clave);

                    Usr.Activo = true;

                    db.Usuarios.Add(Usr);

                    db.SaveChanges();

                    resp = new
                    {
                        Success = true
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
                else
                {
                    throw new Exception("Usuario ya existe");
                }
            }
            catch (Exception ex)
            {
                // EncuestasRegistroController.GuardarBitacoraTxt(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
       // [Route("api/Clientes/CambiarClave")]
        [HttpPut]
        public HttpResponseMessage Put([FromBody] CambioClaveModel cambio)
        {
            try
            {
                if (cambio.Nombre == null || cambio.Nombre == null)
                    throw new Exception("usuario no existe");

                Usuarios cliente = db.Usuarios.Where(a => a.Nombre.ToLower().Contains(cambio.Nombre.ToLower())).FirstOrDefault();
                if (cliente == null)
                    throw new Exception("Usuario no existe");
           

                    cliente.Clave = BCrypt.Net.BCrypt.HashPassword(cambio.Clave);
              

                db.SaveChanges();

                resp = new
                {
                    Success = true
                };

                return Request.CreateResponse(HttpStatusCode.OK, resp);
            }
            catch (Exception ex)
            {
                // EncuestasRegistroController.GuardarBitacoraTxt(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpDelete]// [Route("api/Productos/Eliminar")]

        public async Task<IHttpActionResult> DesactivarUsuario(string nombre)
        {
            Usuarios user = db.Usuarios.Where(a => a.Nombre == nombre).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            db.Entry(user).State = EntityState.Modified;

            user.Activo = false;
            await db.SaveChangesAsync();

            return Ok("OK");
        }

    }
}