using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Domain.ValueObjects;

using EmailService.Models;
using PaymentService.Infrastructure.Configuration;

namespace PaymentService.Presentation.Controllers
{
    [ApiController]
    public class ExternoController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;

        public ExternoController(IPaymentService paymentService, IEmailService emailService)
        {
            _paymentService = paymentService;
            _emailService = emailService;
        }

        [HttpPost("enviarEmail")]
        public async Task<IActionResult> NotificarViaEmail([FromBody] EmailMessage Email)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            try
            {
                var emailMessage = new EmailMessage
                {
                    Email = Email.Email,
                    Assunto = Email.Assunto,
                    Mensagem = Email.Mensagem,
                };

                await _emailService.SendEmailAsync(emailMessage);

                return Ok(new
                {
                    email = Email.Email,
                    assunto = Email.Assunto,
                    mensagem = Email.Mensagem
                });
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    Error = "Falha ao enviar o e-mail.",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("cobranca")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentStart paymentStart)
        {
            var result = await _paymentService.CreatePaymentAsync(paymentStart);
            if (result.Status == PaymentStatus.FALHA)
            {
                return UnprocessableEntity(new { codigo = "422", mensagem = "Cobrança não realizada" });
            }

            return Ok(result);
        }

        [HttpPost("processaCobrancasEmFila")]
        public async Task<IActionResult> ProcessPendingPayments()
        {
            var processedPayments = await _paymentService.ProcessPendingPaymentsAsync();
            return Ok(processedPayments);
        }

        [HttpPost("filaCobranca")]
        public IActionResult AddToQueue([FromBody] PaymentStart paymentStart)
        {
            var result = _paymentService.AddToQueue(paymentStart);
            return Ok(result);
        }

        [HttpGet("cobranca/{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = _paymentService.GetPaymentById(id);
            if (payment == null)
            {
                return NotFound(new { codigo = "404", mensagem = "Cobrança não encontrada" });
            }
            return Ok(payment);
        }

        [HttpPost("validaCartaoDeCredito")]
        public async Task<IActionResult> ValidateCreditCard([FromBody] CreditCard card)
        {
            
            var isValid = await _paymentService.ValidateCardAsync(card);

            if (isValid)
            {
                return Ok(new { mensagem = "Cartão válido" });
            }
            else
            {
                return UnprocessableEntity(new[]
                {
                    new { codigo = "422", mensagem = "Cartão inválido ou erro na validação" }
                });
            }
        }
    }
}