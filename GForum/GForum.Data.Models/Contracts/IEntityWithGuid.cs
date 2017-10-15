using System;

namespace GForum.Data.Models.Contracts
{
    public interface IEntityWithGuid
    {
        Guid Id { get; set; }
    }
}
