using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SystemPodatkowy.Data;
using SystemPodatkowy.Models;

namespace SystemPodatkowy.Repositories
{
    internal class TaxpayerRepository : GenericRepository<Taxpayer>, ITaxpayerRepository
    {
        public TaxpayerRepository(TaxSystemContext context) : base(context)
        {
        }

        public IEnumerable<Taxpayer> GetAllActive()
        {
            return _dbSet
                .Include(p => p.Payments)
                .Include(p => p.PropertyAreas)
                .Include(p => p.TaxDeclarations)
                    .ThenInclude(d => d.DeclarationItems)
                .Where(p => p.IsDeleted == false)
                .AsSplitQuery()
                .ToList();
        }

        public IEnumerable <Taxpayer> GetAllDeleted()
        {
            return _dbSet
                .Include(p => p.Payments)
                .Include(p => p.PropertyAreas)
                .Include(p => p.TaxDeclarations)
                    .ThenInclude(d => d.DeclarationItems)
                .Where(p => p.IsDeleted == true)
                .AsSplitQuery()
                .ToList();
        }

        public override void Delete(Taxpayer taxpayer)
        {
            taxpayer.IsDeleted = true;
            _context.SaveChanges();
        }

        public void Restore(Taxpayer taxpayer)
        {
            taxpayer.IsDeleted = false;
            _context.SaveChanges();
        }
    }
}
