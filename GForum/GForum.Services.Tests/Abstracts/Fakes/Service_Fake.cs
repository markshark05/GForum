using GForum.Data.Contracts;
using GForum.Services.Abstract;

namespace GForum.Services.Tests.Abstracts.Fakes
{
    public class Service_Fake : Service<Entitity_Fake>
    {
        public Service_Fake(IUnitOfWork unitOfWork, IRepository<Entitity_Fake> repository)
            : base(unitOfWork, repository)
        {
        }
    }
}
