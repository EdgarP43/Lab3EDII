using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab3ED2.Data;
using Lab3ED2.Huffman;
using Lab3ED2;
using System.IO;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting.Server;

namespace Lab3ED2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public List<string> listaComprimidos = new List<string>();
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            foreach (var item in listaComprimidos)
            {
                return item;
            }
            return "agregar más...";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [Route("Compress")]
        [HttpPost]
        public async Task<IActionResult> PostCompri([FromForm] IFormFile file)
        {
            var nombreArchivo = file.FileName;
            var nombre = nombreArchivo.Split('.');
            nombreArchivo = nombre[0];
            var disk = "D:\\";
            string[] array = new string[2];
            array[0] = disk;
           // var h = Path.IsPathFullyQualified(file.FileName);
            var PesoOriginal = Convert.ToDouble(file.Length);
            if (file != null && file.Length > 0)
            {
                var model = "";
                model = Path.GetRelativePath("D:/", file.FileName);
                array[1] = model;
                var Model = "";
                Model = array[0] + model;
                //model = Path.GetFullPath(model);
                var UbicacionHuffman = ("D:/Escritorio/Lab03EDII/Lab3ED2/Archivos Comprimidos/"+file.FileName);
                var h = "";
                for (int i = 4; i < UbicacionHuffman.Length; i++)
                {
                    h += UbicacionHuffman[i];
                }
                var ubi = "";
                ubi = array[0] + h;
                //UbicacionHuffman = Path.GetFullPath(UbicacionHuffman);
                //var arch = new FileStream(file.FileName, FileMode.Create);
                //file.CopyTo(arch);                
                if (Huffman.Huffman.Instancia.CompresiónHuffman(Model, nombre, ubi) == 1)
                {
                    var RutaArchivoCompreso = ("D:/Escritorio/Lab03EDII/Lab3ED2/Archivos Comprimidos/"+nombreArchivo+".huff");
                    // RutaArchivoCompreso = Path.GetFullPath(RutaArchivoCompreso);
                    var h1 = "";
                    for (int i = 4; i < RutaArchivoCompreso.Length; i++)
                    {
                        h1 += RutaArchivoCompreso[i];
                    }
                    var ruta = "";
                    ruta = array[0] + h1;

                    var ArchivoCompreso = new FileInfo(ruta);
                    var PesoCompreso = Convert.ToDouble(ArchivoCompreso.Length);

                    var Archivo = new Archivo();
                    Archivo.NombreArchivo = nombreArchivo;
                    Archivo.Factor = Math.Round(PesoOriginal / PesoCompreso, 3);
                    Archivo.Razon = Math.Round(PesoCompreso / PesoOriginal, 3);
                    Archivo.Porcentaje = Math.Round(100 * (1 - Convert.ToDouble(Archivo.Razon)), 3);

                    Huffman.Huffman.Instancia.DatosDeArchivos.Add(Archivo.NombreArchivo, Archivo);

                    listaComprimidos.Add(file.FileName+".huff");
                }
            }
            return Ok();
        }

        [Route("Descompress")]
        [HttpPost]
        public ActionResult<string> PostDescompri([FromBody] string Nombre)
        {
            var NombreArchivo = Nombre;
            var nombre = NombreArchivo.Split('.');
            var nombreArchivo = nombre[0];
            var filePath = Path.GetFullPath(NombreArchivo);
            var Archivo = new FileStream(filePath, FileMode.Open);
            Archivo.Close();
            try
            {
                if (Archivo != null && Archivo.Length > 0)
                {
                    var model = ($"/Archivos Comprimidos/{nombreArchivo}");

                    var UbicacionDescomprimidos = ("//Archivos Descomprimidos");

                    if (Huffman.Huffman.Instancia.Descompresion(model, nombre, UbicacionDescomprimidos) == 1)
                    {
                        return "Carga del archivo correcta";
                    }
                    else
                    {
                        return "Carga del archivo incorrecta";
                    }
                }
                else
                {
                    return "ERROR AL CARGAR EL ARCHIVO, INTENTE DE NUEVO";
                }
            }
            catch
            {
                return "ERROR AL CARGAR EL ARCHIVO, INTENTE DE NUEVO";
            }
        }
    }
}
