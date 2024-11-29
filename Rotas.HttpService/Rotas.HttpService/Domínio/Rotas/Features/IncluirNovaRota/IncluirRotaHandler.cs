using CSharpFunctionalExtensions;

namespace Rotas.HttpService.Domínio.Rotas.Features.IncluirNovaRota;

public sealed class IncluirRotaHandler(RotaRepository rotaRepository)
{
    public async Task<Result<string>> ExecutarAsync(IncluirRotaCommand command)
    {
        if (command.Origem is null)
            return Result.Failure<string>("Endereço de origem não pode ser nulo");

        if (command.Destino is null)
            return Result.Failure<string>("Endereço de destino não pode ser nulo");

        if (command.Valor.Equals(null))
            return Result.Failure<string>("Valor da rota não pode ser nulo"); 

        var rotaExistente = await rotaRepository.ObterRota(command.Origem, command.Destino); 
        if (rotaExistente)
            return Result.Failure<string>("Rota já existente");

        var rota = Rota.Criar(command.Origem, command.Destino, command.Valor);

        var resultado = await rotaRepository.IncluirRota(rota.Value);

        if(resultado)
            return Result.Success("Rota inserida com sucesso");

        return Result.Failure<string>("Erro ao incluir nova rota");
    }
}



