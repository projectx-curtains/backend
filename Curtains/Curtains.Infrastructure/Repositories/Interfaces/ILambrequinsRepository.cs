﻿using Curtains.Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Curtains.Infrastructure.Interfaces
{
    public interface ILambrequinsRepository
    {
        IEnumerable<LambrequinsModel> GetAll();
        Task<LambrequinsModel> GetByIdAsync(int Id);
        Task<EntityEntry<LambrequinsModel>> InsertAsync(LambrequinsModel entity, CancellationToken cancellationToken);
        Task UpdateAsync(LambrequinsModel entity);
        Task RemoveAsync(LambrequinsModel entity);
        void SaveChangesAsync();
    }
}
