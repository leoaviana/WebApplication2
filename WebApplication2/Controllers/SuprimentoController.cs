using Microsoft.AspNetCore.Mvc;
using WebApplication2.DB;
using WebApplication2.Model;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuprimentosController : ControllerBase
    {
        private readonly SuprimentoDB _dataAccess;

        public SuprimentosController(IConfiguration configuration)
        {
            _dataAccess = new SuprimentoDB(configuration.GetConnectionString("DefaultConnection"));
        }

        // Suprimentos

        [HttpGet]
        public ActionResult<IEnumerable<Suprimento>> GetSuprimentos()
        {
            var suprimentos = _dataAccess.GetAllSuprimentos();
            return Ok(suprimentos);
        }

        [HttpGet("{id}")]
        public ActionResult<Suprimento> GetSuprimento(int id)
        {
            var suprimento = _dataAccess.GetSuprimentoById(id);
            if (suprimento == null)
            {
                return NotFound();
            }
            return Ok(suprimento);
        }

        [HttpPost]
        public IActionResult CreateSuprimento([FromBody] Suprimento suprimento)
        {
            try
            {
                _dataAccess.CreateSuprimento(suprimento, suprimento.Quantidade);
                return CreatedAtAction(nameof(GetSuprimento), new { id = suprimento.IdSuprimento }, suprimento);
            }
            catch
            { 
                return StatusCode(500, "Ocorreu um erro ao criar o suprimento.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSuprimento(int id, [FromBody] Suprimento suprimento)
        {
            var existingSuprimento = _dataAccess.GetSuprimentoById(id);
            if (existingSuprimento == null)
            {
                return NotFound();
            }

            _dataAccess.UpdateSuprimento(id, suprimento); 
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSuprimento(int id)
        {
            var existingSuprimento = _dataAccess.GetSuprimentoById(id);
            if (existingSuprimento == null)
            {
                return NotFound();
            }

            _dataAccess.DeleteSuprimento(id); 
            return NoContent();
        }

        // EstoqueSuprimentos

        [HttpGet("{id}/estoque")]
        public ActionResult<EstoqueSuprimento> GetEstoqueSuprimento(int id)
        {
            var estoqueSuprimento = _dataAccess.GetEstoqueBySuprimentoId(id);
            if (estoqueSuprimento == null)
            {
                return NotFound();
            }
            return Ok(estoqueSuprimento);
        }

        [HttpPut("{id}/estoque")]
        public IActionResult UpdateEstoqueSuprimento(int id, [FromBody] EstoqueSuprimento estoqueSuprimento)
        {
            _dataAccess.UpdateEstoque(id, estoqueSuprimento);
            return NoContent();
        }
    }
} 
