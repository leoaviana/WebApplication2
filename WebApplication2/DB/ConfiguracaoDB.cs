using Npgsql;
using WebApplication2.Model;

namespace WebApplication2.DBV
{
    public class ConfiguracaoDB
    {
        private readonly string _connectionString;

        public ConfiguracaoDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Configuracao> GetAllConfiguracoes()
        {
            var configuracoes = new List<Configuracao>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM lizcontrol.Configuracoes", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            configuracoes.Add(new Configuracao
                            {
                                IdConfiguracao = (int)reader["idConfiguracao"],
                                Chave = (string)reader["Chave"],
                                Valor = (string)reader["Valor"]
                            });
                        }
                    }
                }
            }
            return configuracoes;
        }

        public Configuracao GetConfiguracaoById(int id)
        {
            Configuracao configuracao = null;
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM lizcontrol.Configuracoes WHERE idConfiguracao = @IdConfiguracao", conn))
                {
                    cmd.Parameters.AddWithValue("IdConfiguracao", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            configuracao = new Configuracao
                            {
                                IdConfiguracao = (int)reader["idConfiguracao"],
                                Chave = (string)reader["Chave"],
                                Valor = (string)reader["Valor"]
                            };
                        }
                    }
                }
            }
            return configuracao;
        }

        public bool CreateConfiguracao(Configuracao configuracao)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO lizcontrol.Configuracoes (Chave, Valor) VALUES (@Chave, @Valor) RETURNING idConfiguracao", conn))
                {
                    cmd.Parameters.AddWithValue("Chave", configuracao.Chave);
                    cmd.Parameters.AddWithValue("Valor", configuracao.Valor);
                    
                    var result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        return false;
                    }

                    configuracao.IdConfiguracao = (int)result;
                    return true;
                }
            }
        }

        public void UpdateConfiguracao(int id, Configuracao configuracao)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE lizcontrol.Configuracoes SET Chave = @Chave, Valor = @Valor WHERE idConfiguracao = @IdConfiguracao", conn))
                {
                    cmd.Parameters.AddWithValue("Chave", configuracao.Chave);
                    cmd.Parameters.AddWithValue("Valor", configuracao.Valor);
                    cmd.Parameters.AddWithValue("IdConfiguracao", id);
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteConfiguracao(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM lizcontrol.Configuracoes WHERE idConfiguracao = @IdConfiguracao", conn))
                {
                    cmd.Parameters.AddWithValue("IdConfiguracao", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
