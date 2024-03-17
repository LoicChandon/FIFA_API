using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_categorieproduit_cpr")]
    [Index(nameof(Nom), IsUnique = true)]
    public partial class CategorieProduit
    {
        public CategorieProduit()
        {
            SousCategories = new HashSet<CategorieProduit>();
            Produits = new HashSet<Produit>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cpr_id")]
        public int Id { get; set; }

        [Column("cpr_nom"), Required]
        [StringLength(100, ErrorMessage = "Le nom de la catégorie ne doit pas dépasser les 100 caractères")]
        public string Nom { get; set; }

        [Column("cpr_idparent")]
        public int? IdCategorieProduitParent { get; set; }

        [ForeignKey(nameof(IdCategorieProduitParent))]
        public CategorieProduit? Parent { get; set; }

        [InverseProperty(nameof(Parent))]
        public virtual ICollection<CategorieProduit> SousCategories { get; set; }

        [InverseProperty(nameof(Produit.Categorie))]
        public virtual ICollection<Produit> Produits { get; set; }
    }
}
