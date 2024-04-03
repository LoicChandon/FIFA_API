﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class ThemeVotesController : ControllerBase
    {
        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour les actions de modifications.
        /// </summary>
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly FifaDbContext _context;

        public ThemeVotesController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/ThemeVotes
        /// <summary>
        /// Retourne la liste des thèmes de vote.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <returns>La liste des thèmes de vote.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ThemeVote>>> GetThemeVotes()
        {
            IQueryable<ThemeVote> query = _context.ThemeVotes;
            if (!await this.MatchPolicyAsync(ProduitsController.SEE_POLICY)) query = query.FilterVisibles();
            return await query.ToListAsync();
        }

        // GET: api/ThemeVotes/5
        /// <summary>
        /// Retourne un thème de vote.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <param name="id">L'id du thème de vote recherché.</param>
        /// <returns>Le thème de vote recherché.</returns>
        /// <response code="404">Le thème de vote recherché n'existe pas ou a été filtré.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ThemeVote>> GetThemeVote(int id)
        {
            var themeVote = await _context.ThemeVotes.FindAsync(id);

            if (themeVote == null) return NotFound();
            if (!themeVote.Visible
                && !await this.MatchPolicyAsync(ProduitsController.SEE_POLICY))
                return NotFound();

            return themeVote;
        }

        // PUT: api/ThemeVotes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie un thème de vote.
        /// </summary>
        /// <param name="id">L'id du thème de vote à modifier.</param>
        /// <param name="themeVote">Les nouvelles informations du thème de vote.</param>
        /// <remarks>NOTE: Requiert les droits d'édition de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le thème de vote recherché n'existe pas.</response>
        /// <response code="400">Les nouvelles informations du thème de vote sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutThemeVote(int id, ThemeVote themeVote)
        {
            if (id != themeVote.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(themeVote);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThemeVoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ThemeVotes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute un thème de vote.
        /// </summary>
        /// <param name="themeVote">Le thème de vote à ajouter.</param>
        /// <remarks>NOTE: Requiert les droits d'ajout de produit.</remarks>
        /// <returns>Le nouveau thème de vote.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">Le nouveau thème de vote est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<ThemeVote>> PostThemeVote(ThemeVote themeVote)
        {
            await _context.ThemeVotes.AddAsync(themeVote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetThemeVote", new { id = themeVote.Id }, themeVote);
        }

        // DELETE: api/ThemeVotes/5
        /// <summary>
        /// Supprime un thème de vote.
        /// </summary>
        /// <param name="id">L'id du thème de vote recherché.</param>
        /// <remarks>NOTE: Requiert les droits de suppression de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le thème de vote recherché n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteThemeVote(int id)
        {
            var themeVote = await _context.ThemeVotes.FindAsync(id);
            if (themeVote == null)
            {
                return NotFound();
            }

            _context.ThemeVotes.Remove(themeVote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ThemeVoteExists(int id)
        {
            return (_context.ThemeVotes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
