﻿using FIFA_API.Models;
using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Controllers
{
    public partial class ProduitsController
    {
        /// <summary>
        /// Associe un produit et une taille.
        /// </summary>
        /// <param name="id">L'id du produit.</param>
        /// <param name="idtaille">L'id de la taille.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le produit ou la taille recherchée n'existe pas.</response>
        /// <response code="204">Le produit et la taille ont été associés ou étaient déjà associés.</response>
        [HttpPost("{id}/tailles/{idtaille}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = EDIT_POLICY)]
        public async Task<IActionResult> PostProduitTaille(int id, int idtaille)
        {
            var produit = await _context.Produits.GetByIdAsync(id);
            if (produit is null) return NotFound();

            var taille = await _context.TailleProduits.FindAsync(idtaille);
            if (taille is null) return NotFound();

            produit.Tailles.Add(taille);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Supprime l'association entre un produit et une taille.
        /// </summary>
        /// <param name="id">L'id du produit.</param>
        /// <param name="idtaille">L'id de la taille.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le produit recherché n'existe pas ou la taille recherchée n'est pas associée au produit.</response>
        /// <response code="403">L'association est utilisée par d'autres entités (<see cref="StockProduit"/>).</response>
        [HttpDelete("{id}/tailles/{idtaille}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = EDIT_POLICY)]
        public async Task<IActionResult> DeleteProduitTaille(int id, int idtaille)
        {
            var produit = await _context.Produits.GetByIdAsync(id);
            if (produit is null) return NotFound();

            var taille = produit.Tailles.FirstOrDefault(t => t.Id == idtaille);
            if (taille is null) return NotFound();

            bool stocksExists = await _context.StockProduits.Include(s => s.VCProduit)
                .AnyAsync(s => s.IdTaille == idtaille && s.VCProduit.IdProduit == id);

            if (stocksExists) return Forbid();

            produit.Tailles.Remove(taille);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Retourne les différentes permissions de l'utilisateur concernant les produits.
        /// </summary>
        /// <returns>Les différentes permissions de l'utilisateur.</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpGet("checkperms")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<ProduitsPermissionCheck>> CheckPerms([FromServices] IAuthorizationService authService)
        {
            return Ok(new ProduitsPermissionCheck()
            {
                Add = (await authService.AuthorizeAsync(User, ADD_POLICY)).Succeeded,
                Edit = (await authService.AuthorizeAsync(User, EDIT_POLICY)).Succeeded,
                Delete = (await authService.AuthorizeAsync(User, DELETE_POLICY)).Succeeded,
            });
        }
    }
}
