using Core.DataBase;

namespace Core.Factories
{
    public interface IContextAdapterFactory
    {
        IContextAdapter Create();
    }

    public class ContextAdapterFactory : IContextAdapterFactory
    {
        public IContextAdapter Create()
        {
            var context = new Context();
            return new ContextAdapter(context);
        }
    }
}