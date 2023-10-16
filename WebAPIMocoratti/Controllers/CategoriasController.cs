using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebAPIMocoratti.DTOs;
using WebAPIMocoratti.Models;
using WebAPIMocoratti.Pagination;
using WebAPIMocoratti.Repository.Interfaces;

namespace WebAPIMocoratti.Controllers
{
    //[Authorize(AuthenticationSchemes ="Bearer")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        //private readonly IConfiguration _configuration;
        //private readonly ILogger _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;


        public CategoriasController(IUnitOfWork uof, IMapper mapper)
        {
            //_configuration = config;
            _uow = uof;
            _mapper = mapper;


        }
        //[HttpGet("Autor")]
        //public string Autor()
        //{
        //    return _configuration["autor"];
        //}
        //[HttpGet("ConnectionString")]
        //public string ConnectionStrings()
        //{
        //    return _configuration["ConnectionStrings:DefaultConnection"];
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
                var categorias = await _uow.CategoriaRepository.GetAllCategorias(categoriasParameters);

                if (categorias is null)
                {
                    return NotFound("Não foi encontrado nenhum produto");
                }
                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevius
                };

                if (Response != null) // Check if Response is not null
                {
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                }

                var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriaDto;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao buscar todas as categorias ou paginá-las");
            }

        }
        /// <summary>
        /// Obetem a categoria pelo ID
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(CategoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoria = await _uow.CategoriaRepository.GetById(p => p.Id == id);
            if (categoria is null)
            {
                return NotFound("Não foi encontrado nenhum produto");
            }
            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return categoriaDto;
        }
        // Ocorreu um erro de referencia ciclica
        [HttpGet("produtosByCategory")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetProdutosByCategory()
        {
            try
            {
                List<Categoria> produtosPorCategoria = await _uow.CategoriaRepository.Get().Include(c => c.Produtos).ToListAsync();
                List<CategoriaDTO> categoriaDTO = _mapper.Map<List<CategoriaDTO>>(produtosPorCategoria);
                //throw new Exception();
                return categoriaDTO;
            }
            catch (Exception)
            {
                //return BadRequest();
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao realizar sua solicitação");
            }
        }

        [HttpPost]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult> Post([FromBody] Categoria categoria)
        {
            if(categoria is null)
            {
                return BadRequest("Não é possível adicionar uma categoria nula");
            }
            _uow.CategoriaRepository.Add(categoria);
            await _uow.Comit();
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id:int}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public  ActionResult Put(int id, [FromBody] CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.Id) return BadRequest("Id da categoria alterada não é o mesmo da categoria encontrada.");
            Categoria categoria = _mapper.Map<Categoria>(categoriaDTO);
            _uow.CategoriaRepository.Update(categoria);
            _uow.Comit();
            return Ok(categoria);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var categoria = await _uow.CategoriaRepository.GetById(c => c.Id == id);
            if (categoria is null) return NotFound("Produto não localizado");
            _uow.CategoriaRepository.Delete(categoria);
            CategoriaDTO categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
            await _uow.Comit();
            return Ok(categoriaDTO);
        }
    }
}
