using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_tailleproduit_tpr")]
    [Index(nameof(Nom), IsUnique = true)]
    public class TailleProduit
    {
        public const int MAX_NOM_LENGTH = 50;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("tpr_id")]
        public int Id { get; set; }

        [Column("tpr_nom")]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le nom ne doit pas dépasser 50 caractères.")]
        public string Nom { get; set; }

    }
}