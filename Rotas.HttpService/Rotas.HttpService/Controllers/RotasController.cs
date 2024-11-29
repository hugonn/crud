using Microsoft.AspNetCore.Mvc;
using Rotas.HttpService.Domínio.Rotas.Features.BuscarRota;
using Rotas.HttpService.Domínio.Rotas.Features.ExcluirRota;
using Rotas.HttpService.Domínio.Rotas.Features.IncluirNovaRota;

namespace Rotas.HttpService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public sealed class RotasController : ControllerBase
    {
        public record NovaRota(string Origem, string Destino, decimal Valor);
        public record InputRota(string Origem, string Destino);

        [HttpPost]
        public async Task<IActionResult> Incluir(
            [FromBody] NovaRota input,
            [FromServices] IncluirRotaHandler handler)
        {
            var command = new IncluirRotaCommand(
                input.Origem,
                input.Destino,
                input.Valor);

            var resultado = await handler.ExecutarAsync(command);

            return resultado.IsSuccess
                ? Ok(resultado.Value)
                : BadRequest(resultado.Error);

        }

        [HttpGet]
        public async Task<IActionResult> BuscarRota(
            [FromQuery] InputRota input,
            [FromServices] BuscarRotaHandler handler)
        {
            var command = new BuscarRotaCommand(
                input.Origem,
                input.Destino);

            var resultado = await handler.ExecutarAsync(command);

            return resultado.IsSuccess
                ? Ok(resultado.Value)
                : BadRequest(resultado.Error);
        }

        //public async Task<IActionResult> ExcluirRota(
        //    [FromBody] ExcluirRota input,
        //    [FromServices] ExcluirRotaHandler handler)
        //{
        //    var command = new ExcluirRotaCommand(
        //        input.Origem,
        //        input.Destino);

        //    var resultado = await handler.ExecutarAsync(command);

        //    return resultado.IsSuccess
        //        ? Ok(resultado.Value)
        //        : BadRequest(resultado.Error);
        //}

    }
}
