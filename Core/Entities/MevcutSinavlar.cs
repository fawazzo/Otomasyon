using System;

namespace Otomasyon.Core.Entities
{
    public class MevcutSinavlar : BaseEntity
    {
        public int Id { get; set; }
        public string SinavAdi { get; set; }
        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public DateTime SinavTarihi { get; set; }


        public ICollection<BasvuruFormu>? BasvuruFormular { get; set; }
    }
}
