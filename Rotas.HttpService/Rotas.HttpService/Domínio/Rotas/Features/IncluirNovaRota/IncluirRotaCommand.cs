namespace Rotas.HttpService.Domínio.Rotas.Features.IncluirNovaRota
{
    public record IncluirRotaCommand(string Origem, string Destino, decimal Valor);
}
