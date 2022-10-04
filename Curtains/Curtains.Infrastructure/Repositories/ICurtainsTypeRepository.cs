﻿using Curtains.Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curtains.Infrastructure.Repositories
{
    public interface ICurtainsTypeRepository
    {
        IEnumerable<CurtainsTypeModel> GetAll();
        Task<CurtainsTypeModel> GetByIdAsync(int Id);
        Task<EntityEntry<CurtainsTypeModel>> InsertAsync(CurtainsTypeModel entity, CancellationToken cancellationToken);
        void UpdateAsync(CurtainsTypeModel entity);
        void RemoveAsync(CurtainsTypeModel entity);
        void SaveChangesAsync();
    }
}
