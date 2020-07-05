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
    public class ProductosClasificacionController:ApiController
    {
        POSContext db = new POSContext();
        object resp;
        public HttpResponseMessage GetClasificacion()
        {
            var pc = db.ProductosClasificacion.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, pc);
        }

        public IHttpActionResult GetOnePC(string id)
        {
            var pc = db.ProductosClasificacion.Where(a => a.CodClasificacion == id).FirstOrDefault();

            if (pc == null)
            {
                return NotFound();
            }

        



            // var token = TokenGenerator.GenerateTokenJwt(user.Nombre, "Admin");

            var obj = new
            {
                pc
            };
            return Ok(obj);
        }
        public HttpResponseMessage Post([FromBody] ProductosClasificacion pc)
        {
            try
            {
                ProductosClasificacion Consulta = db.ProductosClasificacion.Where(a => a.CodClasificacion == pc.CodClasificacion).FirstOrDefault();

                if (Consulta == null)
                {
                    


                

                    db.ProductosClasificacion.Add(pc);

                    db.SaveChanges();

                    resp = new
                    {
                        Success = true
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
                else
                {
                    throw new Exception("Clasificacion ya existe");
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
        public HttpResponseMessage Put([FromBody] ProductosClasificacion cambio)
        {
            try
            {
                if (cambio.CodClasificacion == null || cambio.CodClasificacion == null)
                    throw new Exception("Clasificacion no existe");

                ProductosClasificacion pc = db.ProductosClasificacion.Where(a => a.CodClasificacion == cambio.CodClasificacion).FirstOrDefault();
                if (pc == null)
                    throw new Exception("Clasificacion no existe");


                db.Entry(pc).State = EntityState.Modified;
                pc.Nombre = cambio.Nombre;

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

        public async Task<IHttpActionResult> Eliminar(string id)
        {
            ProductosClasificacion pc = db.ProductosClasificacion.Where(a => a.CodClasificacion == id).FirstOrDefault();
            if (pc == null)
            {
                return NotFound();
            }

            db.ProductosClasificacion.Remove(pc);

          
            await db.SaveChangesAsync();

            return Ok("OK");
        }
    }
}