using AutoMapper;

namespace Store.Order.Models.Dto.Mapper
{
    public interface IMapTo<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T));
    }
}
