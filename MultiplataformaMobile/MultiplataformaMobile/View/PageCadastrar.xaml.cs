using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MultiplataformaMobile.Models;
using MultiplataformaMobile.Services;
using MultiplataformaMobile.Views;

namespace MultiplataformaMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCadastrar : ContentPage
    {
        public PageCadastrar()
        {
            InitializeComponent();
        }

        public PageCadastrar(ModelMatricula matricula)
        {
            InitializeComponent();
            btSalvar.Text = "Alterar";
            txtCodigo.IsVisible = true;
            btExcluir.IsVisible = true;
            txtCodigo.Text = matricula.id.ToString();
            txtMatricula.Text = matricula.matricula;
            datePickerEntrega.Date = matricula.data_entrega;
            datePickerVencimento.Date = matricula.data_vencimento;
        }

        private void btSalvar_Clicked(object sender, EventArgs e)
        {
            try
            {
                ModelMatricula matriculas = new ModelMatricula();
                matriculas.matricula = txtMatricula.Text;
                matriculas.epi = txtEpi.Text;

                matriculas.data_entrega = DateTime.Now;
                matriculas.data_vencimento = DateTime.Now.AddDays(90);

                ServiceDBMatricula dBMatriculas = new ServiceDBMatricula(App.DbPath);
                if (btSalvar.Text == "Inserir")
                {
                    dBMatriculas.Inserir(matriculas);
                    DisplayAlert("Resultado", dBMatriculas.StatusMessage, "OK");
                }
                else
                {
                    matriculas.id = Convert.ToInt32(txtCodigo.Text);
                    dBMatriculas.Alterar(matriculas);
                    DisplayAlert("Matricula alterada com sucesso!", dBMatriculas.StatusMessage, "OK");
                }
                MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
                p.Detail = new NavigationPage(new PageHome());
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", ex.Message, "OK");
            }
        }


        private async void btExcluir_Clicked(object sender, EventArgs e)
        {
            var resp = await DisplayAlert("Excluir Matrícula", "Deseja EXCLUIR esta matricula selecionada?", "Sim", "Não");
            if (resp == true)
            {
                ServiceDBMatricula dBMatriculas = new ServiceDBMatricula(App.DbPath);
                int id = Convert.ToInt32(txtCodigo.Text);
                dBMatriculas.Excluir(id);
                DisplayAlert("Matricula excluída com sucesso", dBMatriculas.StatusMessage, "OK");
                MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
                p.Detail = new NavigationPage(new PageHome());
            }
        }

        private void btCancelar_Clicked(object sender, EventArgs e)
        {
            MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
            p.Detail = new NavigationPage(new PageHome());
        }

        private void DatePickerEntrega_DateSelected(object sender, DateChangedEventArgs e)
        {
            AtualizarContagemRegressiva();
        }

        private void DatePickerVencimento_DateSelected(object sender, DateChangedEventArgs e)
        {
            AtualizarContagemRegressiva();
        }

        private void AtualizarContagemRegressiva()
        {
            DateTime dataEntrega = datePickerEntrega.Date;
            DateTime dataVencimento = datePickerVencimento.Date;

            TimeSpan diff = dataVencimento - dataEntrega;
            int diasRestantes = (int)Math.Ceiling(diff.TotalDays);

            if (diasRestantes >= 0)
            {
                labelCountdown.Text = $"{diasRestantes} dias restantes para o vencimento.";
            }
            else
            {
                labelCountdown.Text = "Data de vencimento anterior à data de entrega.";
            }
        }
    }
}