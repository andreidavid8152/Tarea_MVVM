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
                await App.Current.MainPage.Navigation.PushAsync(new NuevoProductoPage());
            });

        public ICommand EditarProducto =>
            new Command(async () =>
            {
                if (ProductoSeleccionado != null)
                {
                    int IdProducto = ProductoSeleccionado.IdProducto;
                    await App.Current.MainPage.Navigation.PushAsync(new NuevoProductoPage(IdProducto));
                    ProductoSeleccionado = null;
                }
            });

        public ICommand EliminarProducto =>
            new Command(async () =>
            {
                if (ProductoSeleccionado != null)
                {
                    bool confirmacion = await App.Current.MainPage.DisplayAlert("Confirmación", "¿Estás seguro de que quieres eliminar este producto?", "Sí", "No");

                    if (confirmacion)
                    {
                        App.productoRepository.Delete(ProductoSeleccionado);

                        Util.ListaProductos.Clear();
                        App.productoRepository.GetAll().ForEach(Util.ListaProductos.Add);

                        ProductoSeleccionado = null;
                    }
                }
            });
    }
}
