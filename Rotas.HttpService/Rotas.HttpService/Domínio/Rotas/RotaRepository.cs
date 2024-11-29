using Newtonsoft.Json;

namespace Rotas.HttpService.Domínio.Rotas
{
    public sealed class RotaRepository
    {
        private  const string path = "rotas.txt";

        // TODO: Implementar
        public async Task<bool> ObterRota(string origem, string destino)
        {
            // Método que verifica se rota ja existe
            return false;
        }

        //TODO: Organizar código abaixo
        public async Task<IEnumerable<Rota>> ObterRotas()
        {
            try
            {
                // Poderia utilizar o REDIS (cache) aqui 

                var arquivo = File.ReadAllLines(path);
                var rotas = new List<Rota>();

                foreach (var rota in arquivo)
                {
                    var item = JsonConvert.DeserializeObject<Rota>(rota);

                    rotas.Add(item);
                }

                return rotas;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IncluirRota(Rota rota)
        {
            try
            {
                var json = JsonConvert.SerializeObject(rota);

                using var arquivo = File.AppendText(path);
                await arquivo.WriteLineAsync(json);

                arquivo.Close();
            }
            catch (Exception)
            {
                return false;
            }
           
            return true;
        }
    }
}
