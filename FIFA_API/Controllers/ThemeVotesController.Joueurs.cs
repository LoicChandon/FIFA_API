﻿using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Controllers
{
    public partial class ThemeVotesController
    {
        /// <summary>
        /// Associe un joueur à un thème de vote.
        /// </summary>
        /// <param name="id">L'id du thème de vote.</param>
        /// <param name="idjoueur">L'id du joueur.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le thème de vote ou le joueur recherché n'existe pas.</response>
        [HttpPost("{id}/joueurs/{idjoueur}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> AddJoueurToTheme(int id, int idjoueur)
        {
            bool okTheme = await _uow.Themes.Exists(id);
            if (!okTheme) return NotFound();

            bool okJoueur = await _uow.Joueurs.Exists(idjoueur);
            if(!okJoueur) return NotFound();

            var themevotejoueur = new ThemeVoteJoueur()
            {
                IdJoueur = idjoueur,
                IdTheme = id
            };

            await _uow.ThemeJoueurs.Add(themevotejoueur);
            await _uow.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Supprime l'association entre un thème de vote et un joueur.
        /// </summary>
        /// <param name="id">L'id du thème de vote.</param>
        /// <param name="idjoueur">L'id du joueur.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">L'association recherchée n'existe pas.</response>
        [HttpDelete("{id}/joueurs/{idjoueur}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteJoueurFromTheme(int id, int idjoueur)
        {
            var themevotejoueur = await _uow.ThemeJoueurs.GetById(id, idjoueur);
            if (themevotejoueur is null) return NotFound();

            await _uow.ThemeJoueurs.Delete(themevotejoueur);
            await _uow.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Retourne la liste des joueurs d'un thème de vote.
        /// </summary>
        /// <param name="id">L'id du thème de vote.</param>
        /// <returns>La liste des joueurs du thème de vote.</returns>
        [HttpGet("{id}/joueurs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Joueur>>> GetThemeJoueurs(int id)
        {
            var theme = await _uow.Themes.GetByIdWithJoueurs(id);
            if(theme is null) return NotFound();

            return Ok(theme.Joueurs);
        }
    }
}
