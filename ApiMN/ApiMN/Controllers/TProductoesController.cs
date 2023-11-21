using ApiMN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace ApiMN.Controllers
{
    public class TProductoesController : ApiController
    {

        [HttpGet]
        [Route("ConsultarProductos")]
        public List<TProducto> ConsultarProductos()
        {
            using (var context = new BDMNEntities())
            {
                context.Configuration.LazyLoadingEnabled = false;
                return context.TProducto.ToList();
            }
        }

        [HttpPost]
        [Route("RegistrarProducto")]
        public long RegistrarProducto(TProducto tProducto)
        {
            using (var context = new BDMNEntities())
            {
                context.TProducto.Add(tProducto);
                context.SaveChanges();
                return tProducto.ConProducto;
            }
        }

        [HttpPut]
        [Route("ActualizarRutaImagen")]
        public string ActualizarRutaImagen(TProducto tProducto)
        {
            using (var context = new BDMNEntities())
            {
                var datos = context.TProducto.FirstOrDefault(x => x.ConProducto == tProducto.ConProducto);

                if (datos != null)
                {
                    datos.Imagen = tProducto.Imagen;
                    context.SaveChanges();
                }

                return "OK";
            }
        }


        /*
        // GET: api/TProductoes/5
        [ResponseType(typeof(TProducto))]
        public IHttpActionResult GetTProducto(long id)
        {
            TProducto tProducto = db.TProducto.Find(id);
            if (tProducto == null)
            {
                return NotFound();
            }

            return Ok(tProducto);
        }
        

        // DELETE: api/TProductoes/5
        [ResponseType(typeof(TProducto))]
        public IHttpActionResult DeleteTProducto(long id)
        {
            TProducto tProducto = db.TProducto.Find(id);
            if (tProducto == null)
            {
                return NotFound();
            }

            db.TProducto.Remove(tProducto);
            db.SaveChanges();

            return Ok(tProducto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TProductoExists(long id)
        {
            return db.TProducto.Count(e => e.ConProducto == id) > 0;
        }
        */

    }
}