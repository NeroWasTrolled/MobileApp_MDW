using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MultiplataformaMobile.Models;
using MultiplataformaMobile.Services;

namespace MultiplataformaMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageListar : ContentPage
    {
        public PageListar()
        {
            InitializeComponent();
            AtualizaLista();
        }

        public void AtualizaLista()
        {
            ServiceDBMatricula dBMatricula = new ServiceDBMatricula(App.DbPath);
            String matricula = "";
            if (txtMatricula.Text != null) matricula = txtMatricula.Text;
            ListaMatriculas.ItemsSource = dBMatricula.Localizar(matricula);
        }

        private void ListaMatriculas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ModelMatricula matricula = (ModelMatricula)e.SelectedItem;
            MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
            p.Detail = new NavigationPage(new PageCadastrar(matricula));
        }

        private void btLocalizar_Clicked_1(object sender, EventArgs e)
        {
            AtualizaLista();
        }
    }
}