using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Image = Avalonia.Controls.Image;


namespace PruebaParalela;

public partial class MainWindow : Window
{    //DECLARACION
    int index = 0;
    List<Camara> lista = new List<Camara>();
    Boolean profesional = false;
    Image pic;
   
    public MainWindow()
    {       
        InitializeComponent();

    //cargo algunas por defecto
        Camara c1 = new Camara(0, 2, (float)121.30, false, 'd', "Canon", (new Avalonia.Media.Imaging.Bitmap("C:\\Users\\Cala\\RiderProjects\\PruebaParalela\\PruebaParalela\\fotos\\images.jpg")));
        Camara c2 = new Camara(1, 4, (float)12.33, false, '2', "Sony", (new Avalonia.Media.Imaging.Bitmap("C:\\Users\\Cala\\RiderProjects\\PruebaParalela\\PruebaParalela\\fotos\\images2.jpg")));
        Camara c3 = new Camara(2, 6, (float)12.320, false, 'a', "Pentax", (new Avalonia.Media.Imaging.Bitmap("C:\\Users\\Cala\\RiderProjects\\PruebaParalela\\PruebaParalela\\fotos\\images3.jpg")));
        Camara c4 = new Camara(3, 8, (float)123.30, false, 'b', "Nisu", (new Avalonia.Media.Imaging.Bitmap("C:\\Users\\Cala\\RiderProjects\\PruebaParalela\\PruebaParalela\\fotos\\images4.jpg")));
        lista.Add(c1);
        lista.Add(c2);
        lista.Add(c3);
        lista.Add(c4);
        mostrarDatos(lista[0]);
       
        btnOk.IsVisible = false;
        Closed += MainWindow_Closed;
      
    }
    
   

    private void MainWindow_Closed(object? sended, EventArgs e)
    {
        
       
        try
        {
            //con un binaryWriter sobreescribimos el archivo
            using (BinaryWriter bw = new BinaryWriter(File.Open("otravez.data", FileMode.Create)))
            {
                foreach (Camara a in lista)
                {
                    bw.Write(a.Calidad);
                    bw.Write(a.Marca);
                    bw.Write(a.Profesional);
                    //escribimos los datos y pasamos la foto al array de  bytes
                    byte[] fotoBytes = a.GetFotoBytes();
                    //guardamos el tamaño del array antes de la foto
                    bw.Write(fotoBytes.Length);
                    bw.Write(fotoBytes);
                    //se guarda el array
                    bw.Write(a.Index);
                    bw.Write(a.Numero);
                    bw.Write(a.Precio);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("no puc mes");
        }

        //Cerrar la aplicación
        Close();
    }
  
    
    private async void Cargar()
    {
        var dlg = new OpenFileDialog();
        dlg.Filters!.Add(new FileDialogFilter() { Name = "Archivos de texto", Extensions = { "txt" } });
        dlg.Filters!.Add(new FileDialogFilter() { Name = "Archivos de audio", Extensions = { "wav" } });
        dlg.Filters!.Add(new FileDialogFilter() { Name = "Todos los archivos", Extensions = { "*" } });
        dlg.AllowMultiple = false;

        var result = await dlg.ShowAsync(this);
        if (result != null)
        {
            lista = CargarLista(result[0]);

            if (lista.Count > 0)
            {
                mostrarDatos(lista[index]);
            }
            else
            {
                Console.WriteLine("Error al cargar la lista de cámaras desde el archivo.");
            }
        }
    }

   
    private void BtnRetroceder_OnClick(object? sender, RoutedEventArgs e)
    {
        //LLAMADA A METODO
        retroceder();
    }

    private void BtnAvanzar_OnClick(object? sender, RoutedEventArgs e)
    {
        siguiente();
    }

    private void BtnEliminar_OnClick(object? sender, RoutedEventArgs e)
    {
        if (lista.Count > 0)
        {
            //eliminamos de la lista y controlando los errores 
            lista.RemoveAt(index);

            if (index != 0)
            {
                index--;
                Camara aux = lista[index];

                mostrarDatos(aux);
            }
            else
            {
                if (index == 0&& lista.Count>0)
                {
                    Camara aux = lista[index];
                    mostrarDatos(aux);
                }
                else
                    vaciarCampos();

                
            }
        }
        else
        {
            var customDialog = new CustomDialog("Error");
            customDialog.ShowDialog(this);
            vaciarCampos();
        }
    }

    private void BtnNuevo_OnClick(object? sender, RoutedEventArgs e)
    {
        vaciarCampos();
        btnOk.IsEnabled = true;
        btnOk.IsVisible = true;
        btnEliminar.IsEnabled = false;
        btnAvanzar.IsEnabled = false;
        btnNuevo.IsEnabled = false;
        btnRetroceder.IsEnabled = false;
        //vaciamos todo y dejamos solo la opcion para añadir
    }

    private void BtnOk_OnClick(object? sender, RoutedEventArgs e)
    {
       // Image imagen = elegirArchivo();
        bool profesional = (txtCalidad.Text.Equals("Si", StringComparison.OrdinalIgnoreCase));

        // Creamos el objeto que recoge los datos proporcionados
        Camara aux = new Camara(index++, int.Parse(txtNumero.Text), float.Parse(txtPrecio.Text), profesional,
            char.Parse(txtCalidad.Text), txtMarca.Text);//, imagen);

        // Añadimos a la lista
        lista.Add(aux);

        mostrarDatos(aux);

        // Volvemos al estado anterior
        btnOk.IsEnabled = false;
        btnOk.IsVisible = false;
        btnEliminar.IsEnabled = true;
        btnAvanzar.IsEnabled = true;
        btnNuevo.IsEnabled = true;
        btnRetroceder.IsEnabled = true;
    }
    
 
    private void retroceder()
    {
        if (index == 0)
        {
            btnRetroceder.IsEnabled = false;
        }

        if (index > 0)
        {
            index--;
            Camara aux = lista[index];
            mostrarDatos(aux);
        }

        btnAvanzar.IsEnabled = true;


    }
    
    private void mostrarDatos(Camara aux)
    {
        txtCalidad.Text = aux.Calidad.ToString();
        txtMarca.Text = aux.Marca.ToString();
        txtNumero.Text = aux.Numero.ToString();
        txtPrecio.Text = aux.Precio.ToString();
        //mostramos los datos y compr
        if (aux.Profesional)
        {
            txtProfesional.Text = "Profesional";
        }
        else
        {
            txtProfesional.Text = "Aficionado";
        }

        img.Source = aux.Foto;
        // Image imagen = aux.GetFoto();
        // afoto.Image = imagen;
    }
    
    private void siguiente()
    {
        //CONTROLAMOS SEGÚN EL ÍNDICE ESTÁTICO LA POSICIÓN DE LA LISTA QUE MOSTRAMOS
        if (index+1 == lista.Count)
        {
            btnAvanzar.IsEnabled = false;
        }
    
        btnRetroceder.IsEnabled = true;


        if (lista.Count - 1 > index)
        {
            index++;
            Camara aux = lista[index];
            mostrarDatos(aux);
        }

    }

    
    private void vaciarCampos()
    {
        txtCalidad.Text = "";
        txtMarca.Text = "";
        txtNumero.Text = "";
        txtPrecio.Text = "";
        txtProfesional.Text = "";
       // afoto.Image = null;
    }

    private void BtnCargar_OnClick(object? sender, RoutedEventArgs e)
    {
       Cargar();
    }
    
    
    private List<Camara> CargarLista(string rutaArchivo)
    {
        List<Camara> listaCamaras = new List<Camara>();

        try
        {
            using (BinaryReader br = new BinaryReader(File.Open(rutaArchivo, FileMode.Open)))
            {
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    char calidad = br.ReadChar();
                    string marca = br.ReadString();
                    bool profesional = br.ReadBoolean();
                    int fotoLength = br.ReadInt32();
                
                    byte[] fotoBytes = br.ReadBytes(fotoLength);
                    int index = br.ReadInt32();
                    int numero = br.ReadInt32();
                    float precio = br.ReadSingle();

                    Camara camara = new Camara(index, numero, precio, profesional, calidad, marca, null);
                    camara.SetFotoBytes(byteArrayToImage(fotoBytes));
                    listaCamaras.Add(camara);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar el archivo: {ex.Message}");
        }

        return listaCamaras;
    }


    private byte[] ImageToByteArray(Avalonia.Media.Imaging.Bitmap image)
    {
        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Save the image as PNG format
                image.Save(ms);

                return ms.ToArray();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en ImageToByteArray: {ex.Message}");
            return null;
        }
    }

    
    public Avalonia.Media.Imaging.Bitmap byteArrayToImage(byte[] byteArrayIn)
    {
        try
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                // Creamos una imagen a partir de la secuencia de memoria
                var bitmap = new Avalonia.Media.Imaging.Bitmap(ms);
                return bitmap;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al convertir bytes a imagen: {ex.Message}");
            return null;
        }
    }

}