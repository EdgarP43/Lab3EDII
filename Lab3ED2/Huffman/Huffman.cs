using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3ED2.Huffman
{
    public class Huffman
    {
        public Dictionary<string, Dictionary<byte, string>> CodigosPrefijo = new Dictionary<string, Dictionary<byte, string>>();
        public ArbolS Arbol = new ArbolS();
        public Dictionary<byte, string> CodigosPrefijoArchivoActual = new Dictionary<byte, string>();
        public Dictionary<string, Archivos> DatosDeArchivos = new Dictionary<string, Archivos>();
        public Dictionary<string, byte> CodigoPD = new Dictionary<string, byte>();
    }
}
