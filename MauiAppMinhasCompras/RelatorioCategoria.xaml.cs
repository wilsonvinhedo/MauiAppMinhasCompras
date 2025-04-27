using MauiAppMinhasCompras.Models;
using System.Collections.Generic;
using System.Linq;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioCategoria : ContentPage
{
    public RelatorioCategoria()
    {
        InitializeComponent();
        CarregarRelatorio();
    }

    private async void CarregarRelatorio()
    {
        var produtos = await App.Db.GetAll();

        // Agrupa por categoria e calcula o total
        var relatorio = produtos
            .GroupBy(p => p.Categoria)
            .Select(g => new KeyValuePair<string, double>(
                string.IsNullOrEmpty(g.Key) ? "Sem Categoria" : g.Key,
                g.Sum(p => p.Total)))
            .OrderByDescending(x => x.Value)
            .ToList();

        collectionCategorias.ItemsSource = relatorio;
        lblTotalGeral.Text = $"Total Geral: {produtos.Sum(p => p.Total):C}";
    }

    private async void VoltarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}