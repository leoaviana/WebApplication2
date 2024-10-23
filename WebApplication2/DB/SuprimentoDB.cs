using Npgsql;
using WebApplication2.Model;

namespace WebApplication2.DB
{
    public class SuprimentoDB
    {
        private readonly string _connectionString;

        public SuprimentoDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Suprimentos

        public List<Suprimento> GetAllSuprimentos()
        {
            var suprimentos = new List<Suprimento>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM lizcontrol.Suprimentos", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suprimentos.Add(new Suprimento
                            {
                                IdSuprimento = (int)reader["idSuprimento"],
                                Nome = (string)reader["nome"],
                                UnidadeMedida = (string)reader["unidadeMedida"],
                                Preco = (double)reader["preco"],
                                QuantidadePreco = (int)reader["quantidadePreco"]
                            });
                        }
                    }
                }
            }
            return suprimentos;
        }

        public Suprimento GetSuprimentoById(int id)
        {
            Suprimento suprimento = null;
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM lizcontrol.Suprimentos WHERE idSuprimento = @IdSuprimento", conn))
                {
                    cmd.Parameters.AddWithValue("IdSuprimento", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            suprimento = new Suprimento
                            {
                                IdSuprimento = (int)reader["idSuprimento"],
                                Nome = (string)reader["nome"],
                                UnidadeMedida = (string)reader["unidadeMedida"],
                                Preco = (double)reader["preco"],
                                QuantidadePreco = (int)reader["quantidadePreco"]
                            };
                        }
                    }
                }
            }
            return suprimento;
        }

        public void CreateSuprimento(Suprimento suprimento, int quantidade = 0)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    using (var cmd = new NpgsqlCommand(
                        "INSERT INTO lizcontrol.Suprimentos (nome, unidadeMedida, preco, quantidadePreco) VALUES (@Nome, @UnidadeMedida, @Preco, @QuantidadePreco) RETURNING idSuprimento", conn))
                    {
                        cmd.Parameters.AddWithValue("Nome", suprimento.Nome);
                        cmd.Parameters.AddWithValue("UnidadeMedida", suprimento.UnidadeMedida);
                        cmd.Parameters.AddWithValue("Preco", suprimento.Preco);
                        cmd.Parameters.AddWithValue("QuantidadePreco", suprimento.QuantidadePreco);
                        object result = cmd.ExecuteScalar();

                        suprimento.IdSuprimento = (int)result;
                    }

                    using (var cmd = new NpgsqlCommand(
                        "INSERT INTO lizcontrol.EstoqueSuprimentos (idSuprimento, quantidade) VALUES (@IdSuprimento, @Quantidade)", conn))
                    {
                        cmd.Parameters.AddWithValue("IdSuprimento", suprimento.IdSuprimento);
                        cmd.Parameters.AddWithValue("Quantidade", quantidade);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit(); 
                }
            }

        }


        public void UpdateSuprimento(int id, Suprimento suprimento)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE lizcontrol.Suprimentos SET nome = @Nome, unidadeMedida = @UnidadeMedida, preco = @Preco, quantidadePreco = @QuantidadePreco WHERE idSuprimento = @IdSuprimento", conn))
                {
                    cmd.Parameters.AddWithValue("Nome", suprimento.Nome);
                    cmd.Parameters.AddWithValue("UnidadeMedida", suprimento.UnidadeMedida);
                    cmd.Parameters.AddWithValue("Preco", suprimento.Preco);
                    cmd.Parameters.AddWithValue("QuantidadePreco", suprimento.QuantidadePreco);
                    cmd.Parameters.AddWithValue("IdSuprimento", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteSuprimento(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM lizcontrol.Suprimentos WHERE idSuprimento = @IdSuprimento", conn))
                {
                    cmd.Parameters.AddWithValue("IdSuprimento", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        // EstoqueSuprimentos

        public EstoqueSuprimento GetEstoqueBySuprimentoId(int id)
        {
            EstoqueSuprimento estoque = null;
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM lizcontrol.EstoqueSuprimentos WHERE idSuprimento = @IdSuprimento", conn))
                {
                    cmd.Parameters.AddWithValue("IdSuprimento", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            estoque = new EstoqueSuprimento
                            {
                                IdSuprimento = (int)reader["idSuprimento"],
                                Quantidade = (int)reader["quantidade"]
                            };
                        }
                    }
                }
            }
            return estoque;
        }

        public void UpdateEstoque(int id, EstoqueSuprimento estoque)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE lizcontrol.EstoqueSuprimentos SET quantidade = @Quantidade WHERE idSuprimento = @IdSuprimento", conn))
                {
                    cmd.Parameters.AddWithValue("Quantidade", estoque.Quantidade);
                    cmd.Parameters.AddWithValue("IdSuprimento", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
