using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3ED2.Data
{
    public class Archivo
    {
        public string NombreArchivo { get; set; }
        public double Razon { get; set; }
        public double Factor { get; set; }
        public double Porcentaje { get; set; }

        public Archivo()
        {
            NombreArchivo = "";
            Razon = 0;
            Factor = 0;
            Porcentaje = 0;
        }
    }
}
