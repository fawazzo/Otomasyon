using Otomasyon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        Task<BasvuruFormu> GetUserDetailsByTcKimlikNumarasiAsync(string tcKimlikNumarasi);
    }

}
