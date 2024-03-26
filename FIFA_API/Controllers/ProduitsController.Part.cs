﻿using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Controllers
{
    public partial class ProduitsController
    {
        public const int PRODUCTS_PER_PAGE = 20;

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Produit>>> SearchProduits(
            [FromQuery] string? q,
            [FromQuery] int[] categories,
            [FromQuery] int[] competitions,
            [FromQuery] int[] genres,
            [FromQuery] int[] nations,
            [FromQuery] int[] couleurs,
            [FromQuery] int[] tailles,
            [FromQuery] bool? desc,
            [FromQuery] int? page)
        {
            IQueryable<Produit> query = _context.Produits
                .Include(p => p.Variantes)
                .Include(p => p.Tailles);

            if (categories.Length > 0)
                query = query.Where(p => categories.Contains(p.IdCategorieProduit));

            if (competitions.Length > 0)
                query = query.Where(p => competitions.Contains(p.IdCompetition));

            if (genres.Length > 0)
                query = query.Where(p => genres.Contains(p.IdGenre));

            if (nations.Length > 0)
                query = query.Where(p => nations.Contains(p.IdNation));

            if (couleurs.Length > 0)
                query = query.Where(p => p.Variantes.Any(v => couleurs.Contains(v.IdCouleur)));

            if (tailles.Length > 0)
                query = query.Where(p => p.Tailles.Any(t => tailles.Contains(t.Id)));

            if(q is not null)
                query = query.Where(p => p.Titre.ToLower().Contains(q.ToLower()));

            if (desc == false)
                query = query.OrderBy(p => p.Variantes.Select(v => v.Prix).Min());

            else if (desc == true)
                query = query.OrderByDescending(p => p.Variantes.Select(v => v.Prix).Min());

            if (page is not null)
            {
                int _page = (int)page;
                if (_page < 1) return BadRequest(new { Message = "Invalid page" });

                query = query.Skip(PRODUCTS_PER_PAGE * (_page - 1)).Take(PRODUCTS_PER_PAGE); // TODO : Pagination PAS OPTI
            }

            return Ok(await query.ToListAsync());
        }
    }
}