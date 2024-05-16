using Doorang.Core.Models;
using Doorang.Core.RepositoryAbstacts;
using Doorang.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doorang.Data.RepositoryConcretes;

public class ExplorerRepository : GenericRepository<Explorer>, IExplorerRepository
{
    public ExplorerRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
}
