using Microsoft.AspNetCore.Mvc;
using WebApplication2.DB;
using WebApplication2.DBV;
using WebApplication2.Model;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracaoController : ControllerBase
    {
        private readonly ConfiguracaoDB _dataAccess;

        public ConfiguracaoController(IConfiguration configuration)
        {
            _dataAccess = new ConfiguracaoDB(configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpGet]
        public ActionResult<IEnumerable<Configuracao>> GetConfiguracoes()
        {
            var configuracoes = _dataAccess.GetAllConfiguracoes();
            return Ok(configuracoes);
        }

        [HttpGet("{id}")]
        public ActionResult<Configuracao> GetConfiguracao(int id)
        {
            var configuracao = _dataAccess.GetConfiguracaoById(id);
            if (configuracao == null)
            {
                return NotFound();
            }
            return Ok(configuracao);
        }

        [HttpPost]
        public IActionResult CreateConfiguracao([FromBody] Configuracao configuracao)
        {
            _dataAccess.CreateConfiguracao(configuracao);
            return CreatedAtAction(nameof(GetConfiguracao), new { id = configuracao.IdConfiguracao }, configuracao);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateConfiguracao(int id, [FromBody] Configuracao configuracao)
        {
            var existingConfiguracao = _dataAccess.GetConfiguracaoById(id);
            if (existingConfiguracao == null)
            {
                return NotFound();
            }
            _dataAccess.UpdateConfiguracao(id, configuracao);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteConfiguracao(int id)
        {
            var existingConfiguracao = _dataAccess.GetConfiguracaoById(id);
            if (existingConfiguracao == null)
            {
                return NotFound();
            }
            _dataAccess.DeleteConfiguracao(id);
            return NoContent();
        }
    }
}
