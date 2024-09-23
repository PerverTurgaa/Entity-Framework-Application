namespace EntityFrameworkCore.Data
{
    public class Kurs
    {
        public int KursId { get; set; }
        public string? Baslik {  get; set; }
        public int OgretmenId { get; set; }
        // burda örnek kurs açılır daha sonradan öğretmen o kursu sonradan verir
        // bu şekilde düşünürsek int? veririz ama kurs açıldığında kesin öğretmeni  de olsun diyorsak int bırakmalıyız
        //The ALTER TABLE statement conflicted with the FOREIGN KEY constraint "FK_Kurslar_Ogretmenler_OgretmenId". The conflict occurred in database "KursDB", table "dbo.Ogretmenler", column 'OgretmenId'.
        // bu hatayı verir daha sonradan migration ı kaldırıp nullable olanı yapcıaz
        public Ogretmen Ogretmen { get; set; } = null!;
        public ICollection<KursKayit> KursKayitlari { get; set; } = null!;

    }
}
