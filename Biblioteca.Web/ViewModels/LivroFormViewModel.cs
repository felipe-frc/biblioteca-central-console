using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.ViewModels;

public class LivroFormViewModel : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o título.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o autor.")]
    public string Autor { get; set; } = string.Empty;

    [Display(Name = "Ano de publicação")]
    [Required(ErrorMessage = "Informe o ano de publicação.")]
    public int? AnoPublicacao { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        int anoAtual = DateTime.Now.Year;

        if (AnoPublicacao.HasValue &&
            (AnoPublicacao < 1450 || AnoPublicacao > anoAtual))
        {
            yield return new ValidationResult(
                $"O ano de publicação deve estar entre 1450 e {anoAtual}.",
                new[] { nameof(AnoPublicacao) });
        }
    }
}