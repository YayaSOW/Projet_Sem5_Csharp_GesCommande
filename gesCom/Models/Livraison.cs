namespace gesCom.Models;

public class Livraison
{
    public int Id { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string? Adresse { get; set; }
        public int CommandeId { get; set; }
        public Commande? Commande { get; set; }
        public int LivreurId { get; set; }
        public Livreur? Livreur { get; set; }
}