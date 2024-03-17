
using FIFA_API.Models.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{

	[Table("t_e_voteutilisateur_vtl")]
    [ComposedKey(nameof(IdUtilisateur), nameof(IdCouleur), nameof(IdTaille))]
    public partial class VoteUtilisateur
    {
        [Column("utl_id", Order = 0), Required]
        public int IdUtilisateur { get; set; }

        [Column("col_id", Order = 1), Required]
        public int IdCouleur { get; set; }

        [Column("tpr_id", Order = 2), Required]
        public int IdTaille { get; set; }

        [ForeignKey(nameof(IdUtilisateur))]
        public Utilisateur Utilisateur { get; set; }

        [ForeignKey(nameof(IdCouleur))]
        public Couleur Couleur { get; set; }

        [ForeignKey(nameof(IdTaille))]
        public TailleProduit Taille { get; set; }

        [Column("vtl_rankvote"), Required]
        [Range(0, 5, ErrorMessage = "Le rank de vote doit �tre entre 0 et 5.")]
        public int RankVote { get; set; }
    }
}
