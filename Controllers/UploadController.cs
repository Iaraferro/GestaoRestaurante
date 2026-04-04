using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("imagem-prato")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UploadImagemPrato(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");

            // Valida se é imagem
            var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();

            if (!extensoesPermitidas.Contains(extensao))
                return BadRequest("Formato inválido. Use JPG, PNG ou WEBP.");

            // Limita tamanho a 5MB
            if (arquivo.Length > 5 * 1024 * 1024)
                return BadRequest("Arquivo muito grande. Máximo 5MB.");

            // Gera nome único para evitar conflitos
            var nomeArquivo = $"prato-{Guid.NewGuid()}{extensao}";
            var pasta = Path.Combine(_env.WebRootPath, "img", "pratos");

            // Cria a pasta se não existir
            Directory.CreateDirectory(pasta);

            var caminhoCompleto = Path.Combine(pasta, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            // Retorna a URL pública que o frontend vai salvar no banco
            var urlPublica = $"/img/pratos/{nomeArquivo}";

            return Ok(new { url = urlPublica });
        }
    }
}