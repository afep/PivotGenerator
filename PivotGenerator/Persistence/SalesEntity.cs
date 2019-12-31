using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivotGenerator
{
    class SalesEntity
    {
        private string idMerchant;
        private string idFabricante;
        private string idTiempo;

        public string IdTiempo { get => idTiempo; set => idTiempo = value; }
        public string IdFabricante { get => idFabricante; set => idFabricante = value; }
        public string IdMerchant { get => idMerchant; set => idMerchant = value; }
    }
}
