namespace GForum.Data.Contracts
{
    public interface IUnitOfWork
    {
        int Complete();
    }
}