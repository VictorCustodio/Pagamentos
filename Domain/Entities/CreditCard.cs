using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PaymentService.Domain.ValueObjects
{
    public class CreditCard
    {
        private static readonly Regex NumeroRegex = new(@"\d+", RegexOptions.Compiled);
        private static readonly Regex CvvRegex = new(@"\d{3,4}", RegexOptions.Compiled);
        private static readonly Regex ValidadeRegex = new(@"^(0[1-9]|1[0-2])\/\d{4}$", RegexOptions.Compiled); // MM/YYYY

        private string _numero;
        private string _cvv;
        private string _validade;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Gerado pelo BD
        public int Id { get; set; }
        public string NomeTitular;
        public string Numero;
        public string Validade;
        public string Cvv;

    }
}
