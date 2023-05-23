using AutoMapper;

namespace AudioPlayerWebAPI.UseCase.Mapping
{
    public interface IMap<T>
    {
        void Mapping(Profile profile) =>
            profile.CreateMap(typeof(T), GetType());
    }
}
