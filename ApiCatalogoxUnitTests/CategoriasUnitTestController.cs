using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIMocoratti.Context;
using WebAPIMocoratti.Controllers;
using WebAPIMocoratti.DTOs;
using WebAPIMocoratti.DTOs.Mappings;
using WebAPIMocoratti.Models;
using WebAPIMocoratti.Pagination;
using WebAPIMocoratti.Repository;
using WebAPIMocoratti.Repository.Interfaces;

namespace ApiCatalogoxUnitTests
{
    public class CategoriasUnitTestController
    {
        private readonly IUnitOfWork repository;
        private readonly IMapper mapper;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "Data Source=RN0986;Initial Catalog=WebAPIMocaratti;Integrated Security=True;TrustServerCertificate=True";
        //um construtor estatico é chamado automaticamente  antes que a primeira instancia seja criada
        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options;
        }
        public CategoriasUnitTestController()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mappingConfig.CreateMapper();
            var context = new AppDbContext(dbContextOptions);
            // caso eu quisesse criar um novo banco para teste e quisesse povoalo chamaria uma instancia da classe e o seu método
            //DBUnitTestsMockInitializer db = new DBUnitTestMockInitializer();
            //db.Seed(context)
            repository = new UnitOfWork(context);
        }
        [Fact]
        public async Task GetAllCategory_Return_OkResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);
            var parameters = new CategoriasParameters();

            //Act
            var data = await controller.Get(parameters);

            //Assert
            Assert.IsType<List<CategoriaDTO>>(data.Value);
        }
        [Fact]
        public async Task GetAllCategory_Return_BadRequest()
        {
            var controller = new CategoriasController(repository, mapper);
            var parameters = new CategoriasParameters();

            var data = await controller.Get(parameters);

            Assert.IsType<BadRequestObjectResult>(data.Result);
        }
        [Fact]
        public async Task GetProdutosByCategory_Return_OkResult()
        {
            var controller = new CategoriasController(repository, mapper);

            var data = await controller.GetProdutosByCategory();

            Assert.IsType<List<CategoriaDTO>>(data.Value);
        }
        [Fact]
        public async Task GetProdutosByCategory_Return_BadResquest()
        {
            var controller = new CategoriasController(repository, mapper);

            var data = await controller.GetProdutosByCategory();

            Assert.IsType<BadRequestResult>(data.Result);
        }
        [Fact]
        public async Task GetProdutosById_Return_OkResult()
        {
            var controller = new CategoriasController(repository, mapper);

            var data = await controller.Get(1);

            Assert.IsType<CategoriaDTO>(data.Value);
        }
        [Fact]
        public async Task GetProdutosById_Return_NotFound()
        {
            var controller = new CategoriasController(repository, mapper);

            var data = await controller.Get(1111111);

            Assert.IsType<NotFoundObjectResult>(data.Result);
        }
        [Fact]
        public async Task CreateNewCategory_Return_OkResult()
        {
            var controller = new CategoriasController(repository, mapper);
            var objectCategoria = new Categoria
            {
                Nome = "Teste post com XUnit",
                Descricao = "Teste de post com Xunit",

            };
            var data = await controller.Post(objectCategoria);

            Assert.IsType<CreatedAtRouteResult>(data);
        }
        [Fact]
        public async Task CreateNewCategory_Return_BadRequest()
        {
            var controller = new CategoriasController(repository, mapper);
            Categoria objectCategoria = null;

            var data = await controller.Post(objectCategoria);

            Assert.IsType<BadRequestObjectResult>(data);
        }
        [Fact]
        public async Task Put_EditCategory_Return_OkResult()
        {
            var controller = new CategoriasController(repository, mapper);
            int categoriaId = 1014;

            var existingCategory = await controller.Get(categoriaId);
            var result = existingCategory.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;
            CategoriaDTO catDTO = new CategoriaDTO();
            catDTO.Nome = "Mudei o nome da categoria com o teste Put";
            catDTO.Id = categoriaId;
            catDTO.Descricao = result.Descricao;
            var UpdateData = controller.Put(categoriaId, catDTO);

            Assert.IsType<OkObjectResult>(UpdateData);
        }
        [Fact]
        public void Put_EditCategory_WithWrongId_Return_BadRequest()
        {
            var controller = new CategoriasController(repository, mapper);
            CategoriaDTO catDTO = new CategoriaDTO();
            catDTO.Nome = "Mudei o nome da categoria com o teste Put";
            catDTO.Id = 0;
            var data = controller.Put(1014, catDTO);

            Assert.IsType<BadRequestObjectResult>(data);
        }
        [Fact]
        public void DeleteCategory_Return_OkResult()
        {
            var controller = new CategoriasController(repository, mapper);
            var categoriaId = 1016;

            var data = controller.Delete(categoriaId);

            Assert.IsType<OkObjectResult>(data.Result);
        }

        [Fact]
        public void DeleteCategory_Return_NotFound()
        {
            var controller = new CategoriasController(repository, mapper);
            var categoriaId = 100000;

            var data = controller.Delete(categoriaId);

            Assert.IsType<NotFoundObjectResult>(data.Result);
        }
    }
}
