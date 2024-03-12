using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_stockproduit_spr")]
    public class StockProduit
    {
        public VarianteCouleurProduit VCProduit { get; set; }
        public TailleProduit Taille { get; set; }

		[Column("spr_stocks")]
        public int Stocks { get; set; }

    }
}
