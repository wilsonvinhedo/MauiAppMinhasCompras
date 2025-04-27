using MauiAppMinhasCompras.Views;

namespace MauiAppMinhasCompras;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // REGISTRANDO as rotas para as páginas
        Routing.RegisterRoute(nameof(NovoProduto), typeof(NovoProduto));
        Routing.RegisterRoute(nameof(EditarProduto), typeof(EditarProduto));
    }
}
