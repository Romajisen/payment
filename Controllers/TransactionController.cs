using Bosnet.Models;
using Bosnet.Services;
using Microsoft.AspNetCore.Mvc;
namespace Bosnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Menyetor saldo ke akun tertentu.
        /// </summary>
        /// <param name="request">Data penyetoran.</param>
        /// <returns>Pesan keberhasilan.</returns>
        [HttpPut("setor")]
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public IActionResult Setor([FromBody] SetorRequest request)
        {
            try
            {
                _transactionService.Setor(request);
                return Ok(new { message = "Setor berhasil" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Menarik saldo dari akun tertentu.
        /// </summary>
        /// <param name="request">Data penarikan.</param>
        /// <returns>Pesan keberhasilan.</returns>
        [HttpPut("tarik")]
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public IActionResult Tarik([FromBody] TarikRequest request)
        {
            try
            {
                _transactionService.Tarik(request);
                return Ok(new { message = "Tarik berhasil" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Mentransfer saldo dari satu akun ke beberapa akun tujuan.
        /// </summary>
        /// <param name="request">Data transfer.</param>
        /// <returns>Pesan keberhasilan.</returns>
        [HttpPut("transfer")]
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public IActionResult Transfer([FromBody] TransferRequest request)
        {
            try
            {
                //_transactionService.TransferBagi(request);//maka transfer amount akan di kali jumlah accountid /multiple
                _transactionService.TransferBagi(request);//maka transfer amount  akan di bagi rata ke masing-masing accountid /multiple
                return Ok(new { message = "Transfer berhasil" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Mendapatkan riwayat transaksi berdasarkan akun dan rentang tanggal.
        /// </summary>
        /// <param name="accountId">ID akun.</param>
        /// <param name="startDate">Tanggal mulai.</param>
        /// <param name="endDate">Tanggal akhir.</param>
        /// <returns>Daftar riwayat transaksi.</returns>
        [HttpGet("history")]
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public IActionResult GetHistory([FromQuery] string accountId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var history = _transactionService.GetHistory(accountId, startDate, endDate);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}