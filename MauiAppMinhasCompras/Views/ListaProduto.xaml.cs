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
            // ABRIR tela de Novo Produto
            await Navigation.PushAsync(new NovoProduto());
        }

        private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Produto produtoSelecionado)
            {
                var editarPage = new EditarProduto();
                editarPage.BindingContext = produtoSelecionado;

                await Navigation.PushAsync(editarPage);

                // Limpar seleção
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
            await Task.Delay(1000); // Simula carregamento
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
            var filtro = e.NewTextValue.ToLower();
            var produtosFiltrados = (lst_produtos.ItemsSource as List<Produto>)
                ?.Where(p => p.Descricao.ToLower().Contains(filtro))
                .ToList();

            lst_produtos.ItemsSource = produtosFiltrados;
        }

        private void OnFiltroCategoriaChanged(object sender, EventArgs e)
        {
            var categoriaSelecionada = pickerFiltroCategoria.SelectedItem as string;

            if (categoriaSelecionada == "Todas")
            {
                CarregarProdutos();
            }
            else
            {
                var produtosFiltrados = (lst_produtos.ItemsSource as List<Produto>)
                    ?.Where(p => p.Categoria == categoriaSelecionada)
                    .ToList();

                lst_produtos.ItemsSource = produtosFiltrados;
            }
        }

        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            var produtos = lst_produtos.ItemsSource as List<Produto>;

            if (produtos != null && produtos.Count > 0)
            {
                var somaTotal = produtos.Sum(p => p.Total);
                await DisplayAlert("Total de Compras", $"O valor total é {somaTotal:C}", "OK");
            }
            else
            {
                await DisplayAlert("Aviso", "Nenhum produto para somar.", "OK");
            }
        }
    }
}
