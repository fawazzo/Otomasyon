namespace Otomasyon.Core.Entities
{
    public class BasvuruFormu : BaseEntity
    {
        public int Id { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string TcKimlikNumarasi { get; set; }
        public string Unvan { get; set; }
        public string? Password { get; set; }
        public string? Degerlendirme { get; set; }
        public string? Gorev { get; set; }

        // Foreign key for MevcutSinavlar
        public int? MevcutSinavId { get; set; }
        public MevcutSinavlar? MevcutSinav { get; set; }
    }
}
