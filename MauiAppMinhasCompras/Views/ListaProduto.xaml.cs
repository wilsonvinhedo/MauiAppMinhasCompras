using MauiAppMinhasCompras.Models;
using MauiAppMinhasCompras.Views;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    public ListaProduto()
    {
        InitializeComponent();
        CarregarProdutos();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NovoProduto());
    }

    private async void ToolbarItem_Relatorio_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Relatório", "Função de Relatório chamada com sucesso!", "OK");
    }

    private async void CarregarProdutos()
    {
        try
        {
            var lista = await App.Db.GetAll();
            collection_produtos.ItemsSource = lista;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async void RefreshView_Refreshing(object sender, EventArgs e)
    {
        await Task.Delay(1000);
        CarregarProdutos();

        // Corrigido aqui:
        refreshView.IsRefreshing = false;
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var produtoSelecionado = e.CurrentSelection.FirstOrDefault() as Produto;

        if (produtoSelecionado != null)
        {
            var editarProdutoPage = new EditarProduto();
            editarProdutoPage.BindingContext = produtoSelecionado;
            await Navigation.PushAsync(editarProdutoPage);

            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        var termo = txt_search.Text?.ToLower() ?? "";
        var lista = collection_produtos.ItemsSource as List<Produto>;

        if (string.IsNullOrWhiteSpace(termo))
        {
            CarregarProdutos();
        }
        else
        {
            var filtrado = lista?.Where(x => x.Descricao.ToLower().Contains(termo)).ToList();
            collection_produtos.ItemsSource = filtrado;
        }
    }

    private void OnFiltroCategoriaChanged(object sender, EventArgs e)
    {
        var categoria = pickerFiltroCategoria.SelectedItem?.ToString();
        var lista = collection_produtos.ItemsSource as List<Produto>;

        if (categoria == "Todas" || string.IsNullOrEmpty(categoria))
        {
            CarregarProdutos();
        }
        else
        {
            var filtrado = lista?.Where(x => x.Categoria == categoria).ToList();
            collection_produtos.ItemsSource = filtrado;
        }
    }
}
