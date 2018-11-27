using System;
using System.ComponentModel.DataAnnotations;

namespace SmartContractEthereum.Domain.Entities
{
    public class Entity
    {
        public int? ID { get; set; }

        [Display(Name = "Criado")]
        public DateTime? Created { get; set; }

        [Display(Name = "Atualizado")]
        public DateTime? Updated { get; set; }

        [Display(Name = "Ativo")]
        public bool? Active { get; set; }
    }
}
