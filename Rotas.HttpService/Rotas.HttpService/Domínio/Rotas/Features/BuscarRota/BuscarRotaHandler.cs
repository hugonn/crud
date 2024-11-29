using CSharpFunctionalExtensions;

namespace Rotas.HttpService.Domínio.Rotas.Features.BuscarRota
{
    public sealed class BuscarRotaHandler(RotaRepository rotaRepository)
    {
        public async Task<Result<string>> ExecutarAsync(BuscarRotaCommand command)
        {
            if (command.Origem is null)
                return Result.Failure<string>("Endereço de origem não pode ser nulo");

            if (command.Destino is null)
                return Result.Failure<string>("Endereço de destino não pode ser nulo");

            var rotas = await rotaRepository.ObterRotas();

            var rotaMenorCusto = Rota.BuscarRotaComMenorCusto(rotas, command.Origem, command.Destino);

            if (rotaMenorCusto.IsSuccess)
                return Result.Success(rotaMenorCusto.Value);

            return Result.Failure<string>("Erro ao incluir nova rota");

        }
    }
}
