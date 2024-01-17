using ProductoMVVMSQLite.Models;
using ProductoMVVMSQLite.Utils;
using ProductoMVVMSQLite.Views;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace ProductoMVVMSQLite.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ProductoViewModel
    {
        public ObservableCollection<Producto> ListaProductos { get; set; }
        public Producto ProductoSeleccionado { get; set; }

        public ProductoViewModel()
        {
            Util.ListaProductos = new ObservableCollection<Producto>(App.productoRepository.GetAll());
            ListaProductos = Util.ListaProductos;
        }

        public ICommand CrearProducto =>
            new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new GestionarProductoPage());
            });

        public ICommand EditarProducto =>
            new Command<Producto>(async (producto) =>
            {
                if (producto != null)
                {
                    await App.Current.MainPage.Navigation.PushAsync(new GestionarProductoPage(producto.IdProducto));
                }
            });

        public ICommand EliminarProducto =>
             new Command<Producto>(async (producto) =>
             {
                 if (producto != null)
                 {
                     bool confirmacion = await App.Current.MainPage.DisplayAlert("Aviso", "¿Quieres eliminar este producto?", "Sí", "No");
                     if (confirmacion)
                     {
                         App.productoRepository.Delete(producto);
                         Util.ListaProductos.Remove(producto);
                     }
                 }
             });
    }
}
