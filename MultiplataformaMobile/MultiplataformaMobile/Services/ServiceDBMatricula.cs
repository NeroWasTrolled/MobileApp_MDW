using MultiplataformaMobile.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplataformaMobile.Services
{
    public class ServiceDBMatricula
    {
        SQLiteConnection conn;
        public string StatusMessage { get; set; }
        public ServiceDBMatricula(string dbPath)
        {
            if (dbPath == "") dbPath = App.DbPath;
            conn = new SQLiteConnection(dbPath);
            conn.CreateTable<ModelMatricula>();
        }

        public void Inserir(ModelMatricula control)
        {
            try
            {
                if (!int.TryParse(control.matricula, out int matricula))
                    throw new Exception("Matrícula inválida!");
                if (string.IsNullOrEmpty(control.epi))
                    throw new Exception("EPI não informado!");

                control.data_entrega = DateTime.Now;
                control.data_vencimento = DateTime.Now.AddDays(90);

                int result = conn.Insert(control);
                if (result != 0)
                {
                    this.StatusMessage = string.Format("{0} registro adicionado: {1}", result, control.matricula);
                }
                else
                {
                    this.StatusMessage = string.Format("0 registro adicionado! Por favor, informe a matrícula e a EPI!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ModelMatricula> Listar()
        {
            List<ModelMatricula> lista = new List<ModelMatricula>();
            try
            {
                lista = conn.Table<ModelMatricula>().ToList();
                this.StatusMessage = "Listagem das Matriculas";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lista;
        }
        public void Alterar(ModelMatricula control)
        {
            try
            {
                if (!int.TryParse(control.matricula, out int matricula))
                    throw new Exception("Matrícula inválida!");
                if (string.IsNullOrEmpty(control.epi))
                    throw new Exception("EPI não informado!");
                if (control.id <= 0)
                    throw new Exception("Id da matrícula não informado!");

                control.data_entrega = DateTime.Now;
                control.data_vencimento = DateTime.Now.AddDays(90);

                int result = conn.Update(control);
                StatusMessage = string.Format("{0} registro alterado!", result);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro: {0}", ex.Message));
            }
        }

        public void Excluir(int id)
        {
            try
            {
                int result = conn.Table<ModelMatricula>().Delete(r => r.id == id);
                StatusMessage = string.Format("{0} registro deletado", result);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro: {0}", ex.Message));
            }
        }

        public List<ModelMatricula> Localizar(string titulo)
        {
            List<ModelMatricula> lista = new List<ModelMatricula>();
            try
            {
                var resp = from p in conn.Table<ModelMatricula>()
                           where p.matricula.ToLower().Contains(titulo.ToLower())
                           select p;
                lista = resp.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao localizar registros: {0}", ex.Message));
            }
            return lista;
        }

    }
}
