using System.Reflection;
using AutoMapper;

namespace AudioPlayerWebAPI.UseCase.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile(Assembly assembly)
        {
            ApplyMappings(assembly);
        }

        private void ApplyMappings(Assembly assembly)
        {
            var types = assembly.GetExportedTypes().Where(t =>
                t.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                              i.GetGenericTypeDefinition() == typeof(IMap<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
