using MauiAppMinhasCompras.Models;
using MauiAppMinhasCompras.Views;

namespace MauiAppMinhasCompras.Views
{
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
            await Navigation.PushAsync(new RelatorioCategoria());
        }

        private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Produto produtoSelecionado)
            {
                var editarPage = new EditarProduto();
                editarPage.BindingContext = produtoSelecionado;
                await Navigation.PushAsync(editarPage);
                lst_produtos.SelectedItem = null;
            }
        }

        private async void CarregarProdutos()
        {
            try
            {
                var lista = await App.Db.GetAll();
                lst_produtos.ItemsSource = lista;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void lst_produtos_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            CarregarProdutos();
            lst_produtos.IsRefreshing = false;
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var produto = menuItem?.BindingContext as Produto;

            if (produto != null)
            {
                var confirmacao = await DisplayAlert("Confirmar", $"Deseja excluir o produto {produto.Descricao}?", "Sim", "Não");
                if (confirmacao)
                {
                    await App.Db.Delete(produto);
                    CarregarProdutos();
                }
            }
        }

        private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtro = e.NewTextValue?.ToLower() ?? string.Empty;
            var produtos = lst_produtos.ItemsSource as List<Produto>;

            if (produtos == null) return;

            lst_produtos.ItemsSource = string.IsNullOrEmpty(filtro)
                ? produtos
                : produtos.Where(p => p.Descricao?.ToLower().Contains(filtro) ?? false).ToList();
        }

        private void OnFiltroCategoriaChanged(object sender, EventArgs e)
        {
            var categoriaSelecionada = pickerFiltroCategoria.SelectedItem as string;
            var produtos = lst_produtos.ItemsSource as List<Produto>;

            if (produtos == null || string.IsNullOrEmpty(categoriaSelecionada)) return;

            lst_produtos.ItemsSource = categoriaSelecionada switch
            {
                "Todas" => produtos,
                "Sem Categoria" => produtos.Where(p => string.IsNullOrEmpty(p.Categoria)).ToList(),
                _ => produtos.Where(p => p.Categoria == categoriaSelecionada).ToList()
            };
        }

        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            var produtos = lst_produtos.ItemsSource as List<Produto>;

            if (produtos == null || produtos.Count == 0)
            {
                await DisplayAlert("Aviso", "Nenhum produto para somar.", "OK");
                return;
            }

            var somaTotal = produtos.Sum(p => p.Total);
            await DisplayAlert("Total de Compras", $"O valor total é {somaTotal:C}", "OK");
        }
    }
}