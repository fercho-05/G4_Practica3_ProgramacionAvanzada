using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMN.Entities;
using WebMN.Models;

namespace WebMN.Controllers
{
    public class ProductoController : Controller
    {
        ProductoModel productoModel = new ProductoModel();

        [HttpGet]
        public ActionResult ConsultarProductos()
        {
            var datos = productoModel.ConsultarProductos();
            return View(datos);
        }

        [HttpGet]
        public ActionResult RegistrarProducto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarProducto(HttpPostedFileBase ImgProducto, ProductoEnt entidad)
        {
            entidad.Imagen = string.Empty;
            entidad.Estado = true;

            long conProducto = productoModel.RegistrarProducto(entidad);

            if (conProducto > 0)
            {
                string extension = Path.GetExtension(Path.GetFileName(ImgProducto.FileName));
                string ruta = AppDomain.CurrentDomain.BaseDirectory + "Images\\" + conProducto + extension;
                ImgProducto.SaveAs(ruta);

                entidad.Imagen = "/Images/" + conProducto + extension;
                entidad.ConProducto = conProducto;
                
                productoModel.ActualizarRutaImagen(entidad);

                return RedirectToAction("ConsultarProductos", "Producto");
            }
            else
            {
                ViewBag.MensajeUsuario = "No se ha registrado el producto";
                return View();
            }
        }

        [HttpGet]
        public ActionResult ActualizarEstadoProducto(long q)
        {
            var entidad = new ProductoEnt();
            entidad.ConProducto = q;

            var resp = productoModel.ActualizarEstadoProducto(entidad);

            if (resp == "OK")
            {
                return RedirectToAction("ConsultarProductos", "Producto");
            }
            else
            {
                ViewBag.MensajeUsuario = "No se pudo actualizar el estado del producto";
                return View();
            }
        }

        [HttpGet]
        public ActionResult ActualizarProducto(long q)
        {
            var datos = productoModel.ConsultaProducto(q);
            return View(datos);
        }


        [HttpPost]
        public ActionResult ActualizarProducto(HttpPostedFileBase ImgProducto, ProductoEnt entidad)
        {
            string extension = Path.GetExtension(Path.GetFileName(ImgProducto.FileName));
            
            entidad.Imagen = "/Images/" + entidad.ConProducto + extension; //asignar nombre a la imagen cargada

            string resp = productoModel.ActualizarProducto(entidad);

            if (resp == "OK")
            {
                string nombreBaseSinExtension = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", entidad.ConProducto.ToString());

                // Buscar archivos en el folder images con el consecutivo como nombre no importa la ext
                string[] archivosConMismoNombre = Directory.GetFiles(Path.GetDirectoryName(nombreBaseSinExtension), Path.GetFileNameWithoutExtension(nombreBaseSinExtension) + ".*");

                foreach (var archivo in archivosConMismoNombre)
                {
                    System.IO.File.Delete(archivo);
                }

                string rutaNueva = AppDomain.CurrentDomain.BaseDirectory + "Images\\" + entidad.ConProducto + extension;
                ImgProducto.SaveAs(rutaNueva);

                productoModel.ActualizarRutaImagen(entidad);

                ViewBag.MensajeUsuario = "Producto actualizado exitosamente";
                return View();
            }
            else
            {
                ViewBag.MensajeUsuario = "No se pudo actualizar el producto";
                return View();
            }
        }


    }
}