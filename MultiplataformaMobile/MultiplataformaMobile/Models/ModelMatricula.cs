using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplataformaMobile.Models
{
    using System;

    [Table("Matricula")]
    public class ModelMatricula
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [NotNull]
        public string matricula { get; set; }

        [NotNull]
        public string epi { get; set; }

        [NotNull]
        public DateTime data_entrega { get; set; }

        [NotNull]
        public DateTime data_vencimento { get; set; }

        public ModelMatricula()
        {
            this.id = 0;
            this.matricula = "";
            this.epi = "";
            this.data_entrega = DateTime.Now;
            this.data_vencimento = DateTime.Now.AddDays(60);
        }
    }
}
