using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductoMVVMSQLite.Models;
using ProductoMVVMSQLite.Utils;
using ProductoMVVMSQLite.Views;
using PropertyChanged;
using System.Windows.Input;

namespace ProductoMVVMSQLite.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class GestionarProductoViewModel
    {

        public string Nombre { get; set; }

        public string Cantidad { get; set; }

        public string Descripcion { get; set; }

        private int id = 0;

        private string imagen { set; get; }

        private Producto pEncontrado { get; set; }

        public ImageSource ImagenProducto { set; get; }

        public GestionarProductoViewModel()
        {
            ImagenProducto = "black.jpeg";
        }

        public GestionarProductoViewModel(int IdProducto)
        {
            id = IdProducto;
            pEncontrado = App.productoRepository.Get(id);
            Nombre = pEncontrado.Nombre;
            Descripcion = pEncontrado.Descripcion;
            Cantidad = pEncontrado.Cantidad.ToString();
            ImagenProducto = ImageSource.FromFile(pEncontrado.Imagen);
        }

        public ICommand GuardarProducto =>
            new Command(async () =>
            {
                if (id == 0)
                {
                    Producto producto = new Producto
                    {
                        Nombre = Nombre,
                        Descripcion = Descripcion,
                        Cantidad = Int32.Parse(Cantidad),
                        Imagen = imagen ?? "black.jpeg" // Si no hay nueva imagen, usa una por defecto
                    };
                    App.productoRepository.Add(producto);

                }
                else
                {
                    pEncontrado.Nombre = Nombre;
                    pEncontrado.Cantidad = Int32.Parse(Cantidad);
                    pEncontrado.Descripcion = Descripcion;
                    if (imagen != null) // Solo actualiza la imagen si hay una nueva
                    {
                        pEncontrado.Imagen = imagen;
                    }
                    App.productoRepository.Update(pEncontrado);

                }
                Util.ListaProductos.Clear();
                App.productoRepository.GetAll().ForEach(Util.ListaProductos.Add);
                await App.Current.MainPage.Navigation.PopAsync();
            });

        public ICommand TomarFoto =>
            new Command(async () =>
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo != null)
                    {
                        string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                        using Stream source = await photo.OpenReadAsync();
                        using FileStream localFile = File.OpenWrite(localFilePath);
                        Console.WriteLine(localFilePath);
                        imagen = localFilePath;
                        ImagenProducto = ImageSource.FromFile(imagen);
                        await source.CopyToAsync(localFile);
                    }
                }
            });

    }
}
