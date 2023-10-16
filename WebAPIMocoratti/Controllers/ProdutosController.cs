using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPIMocoratti.DTOs;
using WebAPIMocoratti.Filter;
using WebAPIMocoratti.Models;
using WebAPIMocoratti.Pagination;
using WebAPIMocoratti.Repository.Interfaces;

namespace WebAPIMocoratti.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("PermitirAPIRequest")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;


        public ProdutosController(IUnitOfWork uof, IMapper mapper)
        {
            _uow = uof;
            _mapper = mapper;
        }
        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = await _uow.ProdutoRepository.GetAllProducts(produtosParameters);
            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevius
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            if (produtos is null)
            {
                return NotFound("Não foi encontrado nenhum produto");
            }

            List<ProdutoDTO> produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDTO;
        }
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        [EnableCors("PermitirAPIRequest")]

        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {

            Produto produto = await _uow.ProdutoRepository.GetById(p => p.Id == id);
            if (produto is null)
            {
                return NotFound("Não foi encontrado nenhum produto");
            }

            ProdutoDTO produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            return produtoDTO;
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProdutoDTO produtoDTO)
        {
            Produto produto = _mapper.Map<Produto>(produtoDTO);
            _uow.ProdutoRepository.Add(produto);
            await _uow.Comit();
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id }, produto);
        }
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.Id) return BadRequest();
            Produto produto = _mapper.Map<Produto>(produtoDTO);
            _uow.ProdutoRepository.Update(produto);
            _uow.Comit();
            return Ok(produto);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            Produto produto = await _uow.ProdutoRepository.GetById(p => p.Id == id);
            if (produto is null) return NotFound("Produto não localizado");
            _uow.ProdutoRepository.Delete(produto);
            await _uow.Comit();
            ProdutoDTO produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }
        [HttpGet("MenorPrecoProduto")]
        public async  Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutoByPreco() 
        {
            IEnumerable<Produto> produtos =  await _uow.ProdutoRepository.GetProdutoByPreco();
            List <ProdutoDTO > produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDTO;
        }

    }
}
