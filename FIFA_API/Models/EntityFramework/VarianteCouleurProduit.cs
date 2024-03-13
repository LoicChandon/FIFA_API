using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_j_variantecouleurproduit_vcp")]
    public class VarianteCouleurProduit
    {
        public VarianteCouleurProduit()
        {
            ImageUrls = new List<string>();
        }

        [Key, Column("prd_id", Order = 0)]
        public int IdProduit { get; set; }

        [Key, Column("col_id", Order = 1)]
        public int IdCouleur { get; set; }

		[Column("vcp_prix")]
        public decimal Prix { get; set; }

        [Column("vcp_images")]
        public IList<string> ImageUrls { get; set; }

        [ForeignKey(nameof(IdProduit))]
        public Produit Produit { get; set; }

        [ForeignKey(nameof(IdCouleur))]
        public Couleur Couleur { get; set; }
    }
}