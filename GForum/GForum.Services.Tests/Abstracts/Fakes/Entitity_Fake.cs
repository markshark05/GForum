using System;
using GForum.Data.Models.Contracts;

namespace GForum.Services.Tests.Abstracts.Fakes
{
    public class Entitity_Fake : IEntity, IEntityWithGuid
    {
        public Guid Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
