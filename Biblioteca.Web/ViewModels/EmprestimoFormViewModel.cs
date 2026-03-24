using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Web.ViewModels;

public class EmprestimoFormViewModel : IValidatableObject
{
    [Display(Name = "Usuário")]
    [Required(ErrorMessage = "Selecione um usuário.")]
    public int? UsuarioId { get; set; }

    [Display(Name = "Livro")]
    [Required(ErrorMessage = "Selecione um livro.")]
    public int? LivroId { get; set; }

    [Display(Name = "Data prevista para devolução")]
    [Required(ErrorMessage = "Informe a data prevista para devolução.")]
    [DataType(DataType.Date)]
    public DateTime? DataPrevistaDevolucao { get; set; }

    public List<SelectListItem> Usuarios { get; set; } = new();
    public List<SelectListItem> Livros { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!DataPrevistaDevolucao.HasValue)
            yield break;

        var data = DataPrevistaDevolucao.Value.Date;
        var hoje = DateTime.Today;
        var dataLimite = hoje.AddDays(365);

        if (data < hoje)
        {
            yield return new ValidationResult(
                "A data prevista para devolução não pode ser anterior à data de hoje.",
                new[] { nameof(DataPrevistaDevolucao) });
        }

        if (data > dataLimite)
        {
            yield return new ValidationResult(
                "A data prevista para devolução não pode ultrapassar 365 dias a partir de hoje.",
                new[] { nameof(DataPrevistaDevolucao) });
        }
    }
}