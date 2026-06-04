using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemPodatkowy.Models;

namespace SystemPodatkowy.Repositories
{
    public interface ITaxpayerRepository : IGenericRepository<Taxpayer>
    {
        IEnumerable<Taxpayer> GetAllActive();
        IEnumerable<Taxpayer> GetAllDeleted();
        void Restore(Taxpayer taxpayer);
    }
}
