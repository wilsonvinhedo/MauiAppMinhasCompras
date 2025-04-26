using System;
using System.IO;
using System.Globalization;
using System.Threading;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        public static Database Db { get; private set; }

        public App()
        {
            InitializeComponent();

            // Configurar regionalização para pt-BR
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

            // Inicializar o banco de dados
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "compras.db3");
            Db = new Database(dbPath);

            MainPage = new NavigationPage(new Views.ListaProduto());
        }
    }
}