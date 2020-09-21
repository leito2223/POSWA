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
using System.Web.Http.Cors;
using System.Web.UI.WebControls;
namespace POSWA.Controllers
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*")]
    public class ClientesController: ApiController
    {
        POSContext db = new POSContext();
        object resp;
        public HttpResponseMessage GetCliente()
        {
            var cliente = db.Clientes.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, cliente);
        }

        public IHttpActionResult GetOneCliente(int id)
        {
            var user = db.Clientes.Where(a => a.CodCliente == id ).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }





            // var token = TokenGenerator.GenerateTokenJwt(user.Nombre, "Admin");

            var obj = new
            {
                user
            };
            return Ok(user);
        }
        public HttpResponseMessage Post([FromBody] Clientes cliente)
        {
            try
            {
                Clientes Consulta = db.Clientes.Where(a => a.CodCliente == cliente.CodCliente).FirstOrDefault();

                if (Consulta == null)
                {





                    db.Clientes.Add(cliente);

                    db.SaveChanges();

                    resp = new
                    {
                        Success = true
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
                else
                {
                    throw new Exception("Cliente ya existe");
                }
            }
            catch (Exception ex)
            {
                // EncuestasRegistroController.GuardarBitacoraTxt(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

         [Route("api/Clientes/Actualizar")]
        [AllowAnonymous]
        [HttpPut]
        public HttpResponseMessage Put([FromBody] Clientes cambio)
        {
            try
            {
                if (cambio.CodCliente == null || cambio.CodCliente == 0)
                    throw new Exception("Cliente no existe");

                Clientes pc = db.Clientes.Where(a => a.CodCliente == cambio.CodCliente).FirstOrDefault();
                if (pc == null)
                    throw new Exception("Cliente no existe");


                db.Entry(pc).State = EntityState.Modified;

                pc.Nombre = cambio.Nombre;
                pc.Email = cambio.Email;
                pc.Telefono = cambio.Telefono;
                pc.Direccion = cambio.Direccion;


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


        [Route("api/Clientes/Eliminar")]
        [AllowAnonymous]
        [HttpDelete]// [Route("api/Productos/Eliminar")]

        public async Task<IHttpActionResult> Eliminar(int id)
        {
            Clientes cliente = db.Clientes.Where(a => a.CodCliente == id).FirstOrDefault();
            if (cliente == null)
            {
                return NotFound();
            }

            db.Clientes.Remove(cliente);

            try
            {
                 await db.SaveChangesAsync();

            }catch(Exception ex)
            {
                throw ex;
            }

            return Ok("OK");
        }
    }
}