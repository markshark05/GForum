using System;

namespace GForum.Data.Models.Contracts
{
    public interface IEntity
    {
        DateTime? CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }

        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}
