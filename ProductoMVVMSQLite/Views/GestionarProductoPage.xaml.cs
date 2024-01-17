using ProductoMVVMSQLite.ViewModels;

namespace ProductoMVVMSQLite.Views;

public partial class GestionarProductoPage : ContentPage
{
    public GestionarProductoPage()
    {
        InitializeComponent();
        BindingContext = new GestionarProductoViewModel();
    }

    public GestionarProductoPage(int IdProducto)
    {
        InitializeComponent();
        BindingContext = new GestionarProductoViewModel(IdProducto);
    }
}