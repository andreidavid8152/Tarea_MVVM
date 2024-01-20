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

        private int id = -1;

        private Producto pEncontrado { get; set; }

        public GestionarProductoViewModel()
        {
        }

        public GestionarProductoViewModel(int IdProducto)
        {
            id = IdProducto;
            pEncontrado = App.productoRepository.Get(id);
            Nombre = pEncontrado.Nombre;
            Descripcion = pEncontrado.Descripcion;
            Cantidad = pEncontrado.Cantidad.ToString();
        }

        public ICommand GuardarProducto =>
            new Command(async () =>
            {
                if (id == -1)
                {
                    Producto producto = new Producto
                    {
                        Nombre = Nombre,
                        Descripcion = Descripcion,
                        Cantidad = Int32.Parse(Cantidad),
                    };
                    App.productoRepository.Add(producto);

                }
                else
                {
                    pEncontrado.Nombre = Nombre;
                    pEncontrado.Cantidad = Int32.Parse(Cantidad);
                    pEncontrado.Descripcion = Descripcion;
                    App.productoRepository.Update(pEncontrado);

                }
                Util.ListaProductos.Clear();
                App.productoRepository.GetAll().ForEach(Util.ListaProductos.Add);
                await App.Current.MainPage.Navigation.PopAsync();
            });
    }
}
