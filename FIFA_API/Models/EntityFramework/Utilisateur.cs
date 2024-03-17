using FIFA_API.Models.Annotations;
using FIFA_API.Models.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    ////Ajouter les roles
    //public enum RoleUtilisateur
    //{
    //    Utilisateur = 0,
    //    DirecteurDuServiceVentes = 1,
    //    MembreDuServiceExpedition = 2,
    //    MembreDuServiceCommande = 3
    //}

	[Table("t_e_utilisateur_utl")]
    [Index(nameof(Mail), IsUnique = true)]
    public partial class Utilisateur
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("utl_id")]
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Le prénom ne doit pas dépasser les 100 caractères")]
        [Column("utl_prenom")]
        public string Prenom { get; set; }

        [StringLength(100, ErrorMessage = "Le surnom ne doit pas dépasser les 100 caractères")]
        [Column("utl_surnom")]
        public string Surnom { get; set; }

        [RegularExpression(ModelUtils.REGEX_TELEPHONE, ErrorMessage = "Le numéro de téléphone doit contenir 10 chiffres, dont le 1er doit être un 0")]
        [Column("utl_telephone")]
        public string Telephone { get; set; }
      
        [StringLength(100, ErrorMessage = "Le prénom ne doit pas dépasser les 100 caractères")]
        [Column("utl_prenom")]
        public string Prenom { get; set; }

        [EmailAddress(ErrorMessage = "L'addresse email doit convenir aux normes des adresses email")]
        [Column("utl_mail")]
        public string Mail { get; set; }

        [StringLength(100, ErrorMessage = "L'id de stripe ne doit pas dépasser les 100 caractères")]
        [Column("utl_stripeid")]
        public string StripeId { get; set; }

		[Column("utl_datenaissance", TypeName = "date")]
        public DateTime DateNaissance { get; set; }

        [Column("utl_motdepasse")]
        [StringLength(60, MinimumLength = 60)]
        public string MotDePasse { get; set; }

		[Column("utl_derniereconnexion")]
        public DateTime DerniereConnexion { get; set; }

		[Column("utl_dateverificationemail")]
        public DateTime? DateVerificationEmail { get; set; }

		[Column("utl_doubleauthentification")]
        public bool DoubleAuthentification { get; set; }

        // Role
        [Column("rut_id")]
        public int? IdRole { get; set; }

        [ForeignKey(nameof(IdRole))]
        public RoleUtilisateur? Role { get; set; }

        // Langue
        [Column("lan_id")]
        public int IdLangue { get; set; }

        [ForeignKey(nameof(IdLangue))]
        public Langue Langue { get; set; }

        // Pays
        [Column("pys_idpays")]
        public int IdPays { get; set; }

        [ForeignKey(nameof(IdPays))]
        public Pays Pays { get; set; }

        // Pays favori
        [Column("utl_idpaysfavori")]
        public int? IdPaysFavori { get; set; }

        [ForeignKey(nameof(IdPaysFavori))]
        [OnDelete(DeleteBehavior.SetNull)]
        public Pays? PaysFavori { get; set; }
    }
}
