using Readgress.Data.Contracts;
using Readgress.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readgress.Data
{
    public class ReaderRepository : EFRepository<Reader>, IReaderRepository
    {
        public ReaderRepository(DbContext context) : base(context) { }

    }
}
