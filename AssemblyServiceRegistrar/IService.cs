namespace AssemblyServiceRegistrar
{
    public interface IService { }
    public interface IScopedService : IService { }
    public interface ISingletonService : IService { }
    public interface ITransientService : IService { }
}
