using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Lab3ED2.LZW
{
    public class LZW
    {
        const int bufferLength = 1000000;

        static public void EscribirLZW(Dictionary<string, int> DiccionarioOriginal, int indice, string RutaOriginal, string[] NombreArchivo, string UbicacionAAlmacenarLZW)
        {
            using (var stream = new FileStream(RutaOriginal, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    using (var streamWriter = new FileStream($"{UbicacionAAlmacenarLZW}/{NombreArchivo[0]}.lzw", FileMode.OpenOrCreate))
                    {
                        using (var writer = new BinaryWriter(streamWriter))
                        {
                            var byteBuffer = new byte[bufferLength];

                            writer.Write(NombreArchivo[1]);

                            foreach (var item in DiccionarioOriginal)
                            {
                                writer.Write($"{item.Key}|{item.Value}");
                            }

                            writer.Write("--");


                            var anterior = string.Empty;
                            var actual = string.Empty;
                            var anteriorYActual = string.Empty;
                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                byteBuffer = reader.ReadBytes(bufferLength);

                                for (int i = 0; i < byteBuffer.Length; i++)
                                {
                                    actual = Convert.ToString(Convert.ToChar(byteBuffer[i]));

                                    anteriorYActual = $"{anterior}{actual}";

                                    if (DiccionarioOriginal.ContainsKey(anteriorYActual))
                                    {
                                        anterior = anteriorYActual;
                                    }
                                    else
                                    {
                                        var binario = Convert.ToString(DiccionarioOriginal[anterior], 2);
                                        if (binario.Length <= 8)
                                        {
                                            writer.Write(Convert.ToByte(1));
                                            writer.Write(Convert.ToByte(DiccionarioOriginal[anterior]));
                                        }
                                        else
                                        {
                                            var cantDeBytes = binario.Length / 8 + 1;
                                            binario = Convert.ToString(binario).PadLeft(cantDeBytes * 8, '0');
                                            var vectorBytes = new byte[cantDeBytes];
                                            var index = 0;
                                            for (int j = 0; j < binario.Length; j += 8)
                                            {
                                                vectorBytes[index] = Convert.ToByte(Convert.ToInt32(binario.Substring(j, 8), 2));
                                                index++;
                                            }

                                            writer.Write(Convert.ToByte(cantDeBytes));
                                            foreach (var bytes in vectorBytes)
                                                writer.Write(bytes);

                                        }
                                        DiccionarioOriginal.Add(anteriorYActual, indice);
                                        indice++;
                                        anterior = actual;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        static public int Comprimir(string RutaOriginal, string[] NombreArchivo, string UbicacionAAlmacenarLZW)
        {
            var diccionarioOriginal = new Dictionary<string, int>();
            var indice = 1;
            var valorDiccionario = 0;

            diccionarioOriginal = ObtenerDiccionarioCaracteresEspeciales(RutaOriginal, ref indice);
            valorDiccionario = indice;

            EscribirLZW(diccionarioOriginal, valorDiccionario, RutaOriginal, NombreArchivo, UbicacionAAlmacenarLZW);

            return 1;
        }
        static public Dictionary<string, int> ObtenerDiccionarioCaracteresEspeciales(string RutaOriginal, ref int indice)
        {
            var DiccionarioAEscribir = new Dictionary<string, int>();
            using (var stream = new FileStream(RutaOriginal, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var byteBuffer = new byte[bufferLength];

                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLength);
                        for (int i = 0; i < byteBuffer.Length; i++)
                        {
                            var llave = Convert.ToString(Convert.ToChar(byteBuffer[i]));
                            if (!DiccionarioAEscribir.ContainsKey(llave))
                            {
                                DiccionarioAEscribir.Add(llave, indice);
                                indice++;
                            }
                        }
                    }
                }
            }
            return DiccionarioAEscribir;
        }
    }
}
