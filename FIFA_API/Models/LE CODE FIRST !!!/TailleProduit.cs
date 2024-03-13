using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_tailleproduit_tpr")]
    public class TailleProduit
    {
        [Key]
        [Column("tpr_id")]
        public int Id { get; set; }
        [Column("tpr_nom")]
        public string Nom { get; set; }

    }
}
