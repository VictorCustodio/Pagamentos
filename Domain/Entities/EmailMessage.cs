using System.ComponentModel.DataAnnotations;

namespace EmailService.Models
{
    public class EmailMessage
    {
        [Required(ErrorMessage = "O campo 'email' é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo 'email' deve conter um endereço válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo 'assunto' é obrigatório.")]
        public string Assunto { get; set; }

        [Required(ErrorMessage = "O campo 'mensagem' é obrigatório.")]
        public string Mensagem { get; set; }
    }
}
