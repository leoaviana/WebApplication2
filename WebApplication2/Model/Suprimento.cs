namespace WebApplication2.Model
{
    public class Suprimento
    {
        public int IdSuprimento { get; set; }
        public string Nome { get; set; }
        public string UnidadeMedida { get; set; }
        public double Preco { get; set; }
        public int QuantidadePreco { get; set; }
        public int Quantidade { get; set; } // EstoqueSuprimentos.Quantidade
    }
}
