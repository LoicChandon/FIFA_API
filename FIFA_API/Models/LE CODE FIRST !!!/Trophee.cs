using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_trophee_tph")]
    public class Trophee
    {
        [Key]
        [Column("tph_id")]
        public int Id { get; set; }
        [Column("tph_nom")]
        public string Nom { get; set; }

        public ICollection<Joueur> Joueurs { get; set; }
    }
}
