﻿using FIFA_API.Contracts;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models.Utils;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    public class RegisterInfo
    {
        [Required]
        [EmailAddress(ErrorMessage = "Le mail doit être une adresse valide")]
        public string Mail { get; set; }

        [Required]
        [RegularExpression(ModelUtils.REGEX_PASSWORD, ErrorMessage = "Le mot de passe doit suivre toutes les règles")]
        public string Password { get; set; }

        [Required]
        public int IdLangue { get; set; }

        [Required]
        public int IdPays { get; set; }

        [RegularExpression(ModelUtils.REGEX_TELEPHONE, ErrorMessage = "Le numéro de téléphone n'est pas valide")]
        public string? Telephone { get; set; }

        public string? Prenom { get; set; }
        public string? Surnom { get; set; }
        public DateTime? DateNaissance { get; set; }

        public Utilisateur BuildUser(IPasswordHasher passwordHasher)
        {
            return new Utilisateur()
            {
                Mail = Mail,
                IdLangue = IdLangue,
                IdPays = IdPays,
                HashMotDePasse = passwordHasher.Hash(Password),
                DateNaissance = DateNaissance,
                Prenom = Prenom,
                Surnom = Surnom,
                Telephone = Telephone
            };
        }

        // TODO: Champs facultatifs
    }
}
