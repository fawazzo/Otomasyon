namespace Otomasyon.Core.Entities
{
    public class Kisiler : BaseEntity
    {
        public int Id { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string TcKimlikNumarasi { get; set; }
        public string Unvan { get; set; }
        public string Password { get; set; }
    }
}
