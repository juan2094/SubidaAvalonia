using System;
using System.IO;
using Avalonia.Media;
using Avalonia.Media.Imaging;

[Serializable]
internal class Camara
{
    public int Index { get; set; }
    public int Numero { get; set; }
    public float Precio { get; set; }
    public bool Profesional { get; set; }
    public char Calidad { get; set; }
    public string Marca { get; set; }
    private byte[] FotoBytes { get; set; }
    
    public Avalonia.Media.Imaging.Bitmap Foto { get; set; }

    public Camara(int index, int numero, float precio, bool profesional, char calidad, string marca, Avalonia.Media.Imaging.Bitmap foto)
    {
        this.Index = index;
        this.Numero = numero;
        this.Precio = precio;
        this.Calidad = calidad;
        this.Marca = marca;
        this.Profesional = profesional;
        this.Foto = foto;
    }

    public Camara(int index, int numero, float precio, bool profesional, char calidad, string marca)
    {
        this.Index = index;
        this.Numero = numero;
        this.Precio = precio;
        this.Calidad = calidad;
        this.Marca = marca;
        this.Profesional = profesional;
    }

    public Avalonia.Media.Imaging.Bitmap GetFoto()
    {
        if (FotoBytes != null && FotoBytes.Length > 0)
        {
            return ByteArrayToBitmap(FotoBytes);
        }
        return null;
    }

    public byte[] GetFotoBytes()
    {
        return BitmapToByteArray(Foto);
    }

    public void SetFotoBytes(Avalonia.Media.Imaging.Bitmap foto)
    {
        if (foto != null)
        {
            Foto = foto;
        }
    }

    private byte[] BitmapToByteArray(Avalonia.Media.Imaging.Bitmap bitmap)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            bitmap.Save(ms);
            return ms.ToArray();
        }
    }

    private Avalonia.Media.Imaging.Bitmap ByteArrayToBitmap(byte[] byteArray)
    {
        using (MemoryStream ms = new MemoryStream(byteArray))
        {
            return new Bitmap(ms);
        }
    }
}