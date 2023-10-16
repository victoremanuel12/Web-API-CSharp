using AutoMapper;
using WebAPIMocoratti.Models;

namespace WebAPIMocoratti.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Categoria,CategoriaDTO>().ReverseMap();
        }
    }
}
