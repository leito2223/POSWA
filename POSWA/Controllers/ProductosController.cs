using POSWA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Drawing;
namespace POSWA.Controllers
{
    public class ProductosController:ApiController
    {
        POSContext db = new POSContext();
        object resp;
        public string GuardaImagenBase64(string ImagenBase64, string CarpetaImagen, string NomImagen, System.Drawing.Imaging.ImageFormat FormatoImagen)
        {
            Parametros Params = db.Parametros.FirstOrDefault();

            string NombreImagen = "";
            string rutaImagen = "";

            if (NomImagen == "")
            {
                NombreImagen = "NoImage.png";
            }

            //NombreImagen = $"{PrefijoImagen}_{NomImagen}";
            Random i = new Random();
            int o = i.Next(0, 10000);
            NombreImagen = o + "_" + NomImagen;

            var _bytes = Convert.FromBase64String(ImagenBase64);
            string pathImage = $"~/Content/Images/{CarpetaImagen}/{NombreImagen}";
            var fullpath = System.Web.HttpContext.Current.Server.MapPath(pathImage);
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(_bytes)))
            {
                //image.Save(fullpath, System.Drawing.Imaging.ImageFormat.Png);  // aqui seria en base al tipo de imagen
                image.Save(fullpath, FormatoImagen);  // aqui seria en base al tipo de imagen
            }
            rutaImagen = Params.UrlImagenesApp + pathImage;
            rutaImagen = rutaImagen.Replace("~/Content/Images/", "");

            return NombreImagen;
        }

    
        public HttpResponseMessage GetProducto()
        {


            var producto = db.Productos.Where(a => a.Activo == true).Select(p => new {
                p.Codpro,
                p.Nompro,
                p.Descripcion,
                p.CodCla,
                ProductosClasificaciones = db.ProductosClasificacion.Where(a => a.CodClasificacion == p.CodCla).Select(i => i.Nombre).FirstOrDefault(),
                p.CodBarras,
                p.Precio,
                p.Cantidad,
                Imagen = db.Parametros.Select(a => a.UrlImagenesApp).FirstOrDefault()+ "Productos/" + p.Imagen

            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, producto);
        }

        public IHttpActionResult GetOneProducto(string id)
        {
            var producto = db.Productos.Where(a => a.Codpro == id && a.Activo == true).Select(p => new {
                p.Codpro,
                p.Nompro,
                p.Descripcion,
                p.CodCla,
                ProductosClasificaciones = db.ProductosClasificacion.Where(a => a.CodClasificacion == p.CodCla).Select(i => i.Nombre).FirstOrDefault(),
                p.CodBarras,
                p.Precio,
                p.Cantidad,
                Imagen = db.Parametros.Select(a => a.UrlImagenesApp).FirstOrDefault()+"Productos/" + p.Imagen

            }).FirstOrDefault();

            if (producto == null)
            {
                return NotFound();
            }

 

            var obj = new
            {
                producto
            };
            return Ok(obj);
        }
        public HttpResponseMessage Post([FromBody] ProductosViewModel producto)
        {
            try 
            {
                Productos Consulta = db.Productos.Where(a => a.Codpro == producto.Codpro).FirstOrDefault();

                if (Consulta == null)
                {
                    Consulta = new Productos();
                    if (!String.IsNullOrEmpty(producto.ImagenBase64))
                    {
                        string Url = GuardaImagenBase64(producto.ImagenBase64, "Productos", producto.Codpro + "_" + producto.CodBarras + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        Consulta.Imagen = Url;
                    }
                    else
                    {
                        Consulta.Imagen = Consulta.Imagen;
                    }
                    Consulta.Codpro = producto.Codpro;
                    Consulta.CodCla = producto.CodCla;
                    Consulta.Descripcion = producto.Descripcion;
                    Consulta.CodBarras = producto.CodBarras;
                    Consulta.Cantidad = producto.Cantidad;
                    Consulta.Nompro = producto.Nompro;
                    Consulta.Precio = producto.Precio;
                    Consulta.Activo = true;
                    Consulta.FechaCreacion = DateTime.Now;
                    db.Productos.Add(Consulta);

                    db.SaveChanges();

                    resp = new
                    {
                        Success = true
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
                else
                {
                    throw new Exception("Producto ya existe");
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
        public HttpResponseMessage Put([FromBody] ProductosViewModel cambio)
        {
            try
            {
                if (cambio.Codpro == null || cambio.Codpro == "")
                    throw new Exception("Producto no existe");

                Productos pc = db.Productos.Where(a => a.Codpro == cambio.Codpro).FirstOrDefault();
                if (pc == null)
                    throw new Exception("Producto no existe");


                db.Entry(pc).State = EntityState.Modified;
                pc.Codpro = cambio.Codpro;
                pc.Nompro = cambio.Nompro;
                pc.Descripcion = cambio.Descripcion;
                pc.CodCla = cambio.CodCla;
                pc.CodBarras = cambio.CodBarras;
                pc.Precio = cambio.Precio;
                pc.Cantidad = cambio.Cantidad;
                if (!String.IsNullOrEmpty(cambio.ImagenBase64))
                {
                    string Url = GuardaImagenBase64(cambio.ImagenBase64, "Productos", cambio.Codpro + "_" + cambio.CodBarras + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    pc.Imagen = Url;
                }
                else
                {
                    pc.Imagen = pc.Imagen;
                }
                pc.FechaCreacion = pc.FechaCreacion;
                pc.Activo = cambio.Activo;

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
            Productos cliente = db.Productos.Where(a => a.Codpro == id).FirstOrDefault();
            if (cliente == null)
            {
                return NotFound();
            }

            db.Productos.Remove(cliente);


            await db.SaveChangesAsync();

            return Ok("OK");
        }
    }
}