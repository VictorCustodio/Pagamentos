using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Domain.ValueObjects;

namespace PaymentService.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("cobranca")]
        public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
        {
            var result = await _paymentService.CreatePaymentAsync(payment);
            if (result.Status == PaymentStatus.Failed)
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
        public IActionResult AddToQueue([FromBody] Payment payment)
        {
            var result = _paymentService.AddToQueue(payment);
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
        public async Task<IActionResult> ValidateCreditCard([FromBody] Payment payment)
        {
            
            var isValid = await _paymentService.ValidateCardAsync(payment);

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