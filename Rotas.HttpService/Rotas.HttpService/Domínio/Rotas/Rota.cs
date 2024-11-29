using CSharpFunctionalExtensions;


namespace Rotas.HttpService.Domínio.Rotas
{
    public sealed class Rota
    {
        public string Origem;
        public string Destino;
        public decimal Valor;

        protected Rota() { }

        private Rota(
            string origem,
            string destino,
            decimal valor)
        {
            Origem = origem;
            Destino = destino;
            Valor = valor;
        }

        public static Result<Rota> Criar(
            string origem,
            string destino,
            decimal valor)
        {
            // poderia ter um motor de regras aqui para validar regras de negocio sobre uma nova rota

            if (origem == destino)
                return Result.Failure<Rota>("Origem da rota não pode ser igual ao destino");

            if (valor == 0)
                return Result.Failure<Rota>("Valor da rota não pode ser 0"); // Assumi que não existe rota a custo zero.

            var rota = new Rota(origem, destino, valor);

            return Result.Success(rota);
        }

        // TODO: Organizar código abaixo
        public static Result<string> BuscarRotaComMenorCusto(IEnumerable<Rota> rotas, string origem, string destino)
        {

            var listaNodos = new List<Nodo>();

            foreach (var item in rotas.DistinctBy(x => x.Origem))
            {
                var nodo = new Nodo(item.Origem);
                listaNodos.Add(nodo);
            }

            foreach (var item in rotas.DistinctBy(x => x.Destino))
            {
                if (listaNodos.FirstOrDefault(x => x.Nome.Equals(item.Destino)) is null)
                {
                    var nodo = new Nodo(item.Destino);
                    listaNodos.Add(nodo);
                }
            }

            foreach (var nodo in listaNodos)
            {

                var listaOrigens = rotas.Where(x => x.Origem.Equals(nodo.Nome)); // pego as rotas do nodo e crio as conexoes
                var listaNodoFiltrada = new List<Nodo>();

                foreach (var item in listaOrigens)
                {
                    var nodosConexao = listaNodos.Where(x => x.Nome == item.Destino).FirstOrDefault();
                    listaNodoFiltrada.Add(nodosConexao);
                }

                nodo.CriarConexoes(listaNodoFiltrada, listaOrigens);
            }


            var menorRota = Dijkstra(listaNodos, origem, destino);

            string retorno = $"\"Menor custo de {menorRota.origem.getName()} para {menorRota.destino.getName()} is: {menorRota.distancias[menorRota.destino]}\"";

            return retorno;
        }

        public class Graph
        {
            private List<Nodo> Nodos;

            public Graph(List<Nodo> nodo)
            {
                Nodos = nodo;
            }

            public void Add(Nodo n)
            {
                Nodos.Add(n);
            }

            public void Remove(Nodo n)
            {
                Nodos.Remove(n);
            }

            public List<Nodo> GetNodes()
            {
                return Nodos.ToList();
            }

            public int getCount()
            {
                return Nodos.Count;
            }
        }

        public class Nodo
        {
            public string Nome { get; }

            private Dictionary<Nodo, decimal> Conexoes { get; }

            public Nodo(string nome)
            {
                Nome = nome;
                Conexoes = new Dictionary<Nodo, decimal>();
            }

            public void CriarConexoes(IEnumerable<Nodo> Nodos, IEnumerable<Rota> Rotas)
            {
                foreach (var nodo in Nodos)
                {
                    var valor = Rotas
                        .Where(x => x.Destino == nodo.Nome)
                        .Select(x => x.Valor)
                        .FirstOrDefault();

                    Conexoes.Add(nodo, valor);
                }
            }

            public string getName() => Nome;

            public Dictionary<Nodo, decimal> obterConexoes()
            {
                return Conexoes;
            }
        }

        private static menorCaminho Dijkstra(List<Nodo> listaNodos, string origem, string destino) // GRAPH MINHA LISTA DE NODOS
        {
            var grafo = new Graph(listaNodos);
            int size = listaNodos.Count;
            Dictionary<Nodo, decimal> distancias = new();
            Dictionary<Nodo, Nodo> rotas = new();
            List<Nodo> todosOsNodos = grafo.GetNodes();

            foreach (Nodo n in todosOsNodos)
            {
                distancias.Add(n, decimal.MaxValue);
            }

            foreach (Nodo n in todosOsNodos)
            {
                rotas.Add(n, null);
            }

            var source = listaNodos.FirstOrDefault(x => x.Nome == origem);
            var destination = listaNodos.FirstOrDefault(x => x.Nome == destino);

            distancias[source] = 0;

            while (todosOsNodos.ToList().Count != 0)
            {
                Nodo menorCustoNodo = todosOsNodos.FirstOrDefault();

                foreach (var n in todosOsNodos)
                {
                    if (distancias[n] < distancias[menorCustoNodo])
                        menorCustoNodo = n;
                }

                foreach (var vizinho in menorCustoNodo.obterConexoes())
                {
                    if (distancias[menorCustoNodo] + vizinho.Value < distancias[vizinho.Key])
                    {
                        distancias[vizinho.Key] = vizinho.Value + distancias[menorCustoNodo];
                        rotas[vizinho.Key] = menorCustoNodo;
                    }
                }
                todosOsNodos.Remove(menorCustoNodo);
            }

            return new menorCaminho(source, destination, rotas, distancias);

        }
        private int menorDistancia(int[] custos, bool[] visitado)
        {
            int min = int.MaxValue;
            int minIndex = -1;

            for (int v = 0; v < 10; v++)
            {
                if (visitado[v] == false && custos[v] <= min)
                {
                    min = custos[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

        public record menorCaminho(Nodo origem, Nodo destino, Dictionary<Nodo, Nodo> rotas, Dictionary<Nodo, decimal> distancias);


    }

}
