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
        public void PostCompri([FromForm] IFormFile file)
        {
            var nombreArchivo = file.FileName;
            var nombre = nombreArchivo.Split('.');
            nombreArchivo = nombre[0];
            var disk = "D:\\";
            var PesoOriginal = Convert.ToDouble(file.Length);
            if (file != null && file.Length > 0)
            {
                var model = "";
                model = Path.GetRelativePath("D:/", file.FileName);
                var Model = "";
                Model = disk + model;
                var ubica = Path.GetFullPath("Archivos Comprimidos\\");
                var UbicacionHuffman = "Archivos comprimidos\\" + file.FileName;
                var h = "";
                for (int i = 0; i < UbicacionHuffman.Length; i++)
                {
                    h += UbicacionHuffman[i];
                }            
                if (Huffman.Huffman.Instancia.CompresiónHuffman(Model, nombre, ubica) == 1)
                {
                    var pathArchivoCompri = Path.GetFullPath("Archivos Comprimidos\\");
                    var RutaArchivoCompreso = (pathArchivoCompri + nombreArchivo + ".huff");


                    var ArchivoCompreso = new FileInfo(RutaArchivoCompreso);
                    var PesoCompreso = Convert.ToDouble(ArchivoCompreso.Length);

                    var Archivo = new Archivo();
                    Archivo.NombreArchivo = nombreArchivo;
                    Archivo.Factor = Math.Round(PesoOriginal / PesoCompreso, 3);
                    Archivo.Razon = Math.Round(PesoCompreso / PesoOriginal, 3);
                    Archivo.Porcentaje = Math.Round(100 * (1 - Convert.ToDouble(Archivo.Razon)), 3);

                    Huffman.Huffman.Instancia.DatosDeArchivos.Add(Archivo.NombreArchivo, Archivo);

                    listaComprimidos.Add(nombreArchivo + ".huff");
                }
            }
        }

        [Route("Descompress")]
        [HttpPost]
        public void PostDescompri([FromForm] IFormFile Nombre)
        {
            var NombreArchivo = Nombre.FileName;
            var nombre = NombreArchivo.Split('.');
            var nombreArchivo = nombre[0];
            var filePath = Path.GetFullPath("Archivos Comprimidos\\");
            filePath = filePath + NombreArchivo;
            var pathDescompress = Path.GetFullPath("Archivos Descomprimidos\\");
            var Archivo = new FileStream(filePath, FileMode.Open);
            Archivo.Close();
            var model = filePath;

            var UbicacionDescomprimidos = pathDescompress;

            if (Huffman.Huffman.Instancia.Descompresion(model, nombre, UbicacionDescomprimidos) == 1)
            {
                var g = "";
            }
        }

        [Route("Compresionlzw")]
        [HttpPost]
        public void CompresiónLZW([FromForm] IFormFile file)
        {
            var nombreArchivo = file.FileName;
            var nombre = nombreArchivo.Split('.');
            nombreArchivo = nombre[0];
            var disk = "D:\\";
            var PesoOriginal = Convert.ToDouble(file.Length);
            if (file != null && file.Length > 0)
            {
                var model = "";
                model = Path.GetRelativePath("D:/", file.FileName);
                var Model = "";
                Model = disk + model;
                var ubica = Path.GetFullPath("Archivos Comprimidos\\");
                var UbicacionAAlmacenarLZW = "Archivos comprimidos\\" + file.FileName;
                if (LZW.LZW.Comprimir(Model, nombre, ubica) == 1)
                {
                    var pathArchivoCompri = Path.GetFullPath("Archivos Comprimidos\\");
                    var RutaArchivoCompreso = (pathArchivoCompri + nombreArchivo + ".lzw");


                    var ArchivoCompreso = new FileInfo(RutaArchivoCompreso);
                    var PesoCompreso = Convert.ToDouble(ArchivoCompreso.Length);

                    var Archivo = new Archivo();
                    Archivo.NombreArchivo = nombreArchivo;
                    Archivo.Factor = Math.Round(PesoOriginal / PesoCompreso, 3);
                    Archivo.Razon = Math.Round(PesoCompreso / PesoOriginal, 3);
                    Archivo.Porcentaje = Math.Round(100 * (1 - Convert.ToDouble(Archivo.Razon)), 3);

                    Huffman.Huffman.Instancia.DatosDeArchivos.Add(Archivo.NombreArchivo, Archivo);

                    listaComprimidos.Add(nombreArchivo + ".lzw");
                }
            }
        }

        [Route("Descompresionlzw")]
        [HttpPost]
        public void DescompresiónLZW([FromForm] IFormFile Nombre)
        {
            var NombreArchivo = Nombre.FileName;
            var nombre = NombreArchivo.Split('.');
            var nombreArchivo = nombre[0];
            var filePath = Path.GetFullPath("Archivos Comprimidos\\");
            filePath = filePath + NombreArchivo;
            var pathDescompress = Path.GetFullPath("Archivos Descomprimidos\\");
            var Archivo = new FileStream(filePath, FileMode.Open);
            Archivo.Close();
            var model = filePath;

            var UbicacionDescomprimidos = pathDescompress;

            if (LZW.LZW.Descomprimir(model, nombre, UbicacionDescomprimidos) == 1)
            {
                var g = "";
            }
        }
    }
}

