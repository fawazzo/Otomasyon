using Otomasyon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otomasyon.Core.Repositories;


namespace Services
{
    public class UserService : IUserService
    {
        // Inject necessary dependencies (e.g., repository)
        private readonly IBasvuruFormuRepository _BasvuruFormuRepository;

        public UserService(IBasvuruFormuRepository BasvuruFormuRepository)
        {
            _BasvuruFormuRepository = BasvuruFormuRepository;
        }

        public async Task<BasvuruFormu> GetUserDetailsByTcKimlikNumarasiAsync(string tcKimlikNumarasi)
        {
            // Fetch user details based on TcKimlikNumarasi
            return await _BasvuruFormuRepository.GetUserByTcKimlikNumarasiAsync(tcKimlikNumarasi);
        }
    }

}
