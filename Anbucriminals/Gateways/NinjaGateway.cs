using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Anbucriminals.Mappers;
using Anbucriminals.Models;
using NarutoApi.Infraestructure.SOAP.Contracts;
using Microsoft.Extensions.Logging;
using Anbucriminals.Exceptions;
using NarutoApi.Infrastructure.Soap.Dtos;


namespace Anbucriminals.Gateways;

public class NinjaGateway : INinjaGateway
{
    private readonly INinjaService _ninjaContract;
    private readonly ILogger<NinjaGateway> _logger;

    public NinjaGateway(IConfiguration configuration, ILogger<NinjaGateway> logger)
    {
        var url = configuration.GetValue<string>("NinjaService:Url")
                  ?? throw new InvalidOperationException("Missing config: NinjaService:Url");

        var binding = new BasicHttpBinding
        {
            OpenTimeout = TimeSpan.FromSeconds(5),
            SendTimeout = TimeSpan.FromSeconds(10),
            ReceiveTimeout = TimeSpan.FromSeconds(10),
            MaxReceivedMessageSize = 1024 * 1024
        };

        var endpoint = new EndpointAddress(url);

        _ninjaContract = new ChannelFactory<INinjaService>(binding, endpoint).CreateChannel();
        _logger = logger;
    }

    public async Task<Ninja?> GetNinjaByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var dto = await _ninjaContract.GetNinjaById(id, cancellationToken);

            if (dto is null)
            {
                _logger.LogWarning("Ninja not found (null DTO) for id {NinjaId}", id);
                return null;
            }

            return dto.ToModel();
        }
        catch (FaultException ex) when (ex.Message.Contains("Ninja not found", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Ninja not found (Fault) for id {NinjaId}", id);
            return null;
        }
    }

    public async Task<Ninja> CreateNinjaAsync(Ninja ninja, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sending request to SOAP API, creating ninja: {Name}", ninja.Name);

            var createdDto = await _ninjaContract.CreateNinja(ninja.ToRequest(), cancellationToken);
            return createdDto.ToModel();
        }
        catch (FaultException ex)
        {
            _logger.LogError(ex, "SOAP fault while creating ninja {Name}", ninja.Name);
            throw new UpstreamUnavailableException("Error creating ninja");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating ninja {Name}", ninja.Name);
            throw new UpstreamUnavailableException("Error creating ninja");
        }
    }

    public async Task<Ninja> UpdateNinjaAsync(Ninja ninja, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sending request to SOAP API, updating ninja: {Name}", ninja.Name);

            var updatedDto = await _ninjaContract.UpdateNinja(ninja.ToUpdateRequest(), cancellationToken);
            return updatedDto.ToModel();
        }
        catch (FaultException ex) when (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            throw new NinjaNotFoundException(ninja.Id);
        }
        catch (FaultException ex) when (ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
        {
            throw new NinjaAlreadyExistsException(ninja.Name);
        }
        catch (FaultException ex)
        {
            _logger.LogError(ex, "SOAP fault while updating ninja {Id}", ninja.Id);
            throw new UpstreamUnavailableException("Error updating ninja");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating ninja {Id}", ninja.Id);
            throw new UpstreamUnavailableException("Error updating ninja");
        }
    }

    public async Task DeleteNinjaAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sending request to SOAP API, deleting ninja: {Id}", id);
            await _ninjaContract.DeleteNinja(id, cancellationToken);
        }
        catch (FaultException ex) when (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning(ex, "Ninja not found: {Id}", id);
            throw new NinjaNotFoundException(id);
        }
        catch (FaultException ex)
        {
            _logger.LogError(ex, "SOAP fault while deleting ninja {Id}", id);
            throw new UpstreamUnavailableException("Error deleting ninja");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting ninja {Id}", id);
            throw new UpstreamUnavailableException("Error deleting ninja");
        }
    }
 public async Task<(IList<Ninja> ninjas, int totalRecords)> GetNinjasAsync(
    string? village,
    string? rank,
    string? ninJutsu,
    int? chakraMin,
    int? chakraMax,
    int pageNumber,
    int pageSize,
    string orderBy,
    string orderDirection,
    CancellationToken cancellationToken)
{
    try
    {
        var request = new NarutoApi.Infrastructure.Soap.Dtos.SearchNinjaDto
        {
            Village   = village,
            Rank      = rank,
            NinJutsu  = ninJutsu,
            ChakraMin = chakraMin,
            ChakraMax = chakraMax,
            // si tu SOAP aún no acepta paginación/orden, NO envíes estos campos
            // PageNumber = pageNumber,
            // PageSize   = pageSize,
            // OrderBy    = orderBy,
            // OrderDirection = orderDirection
        };

        var listDto = await _ninjaContract.GetNinjas(request, cancellationToken); // PagedNinjaResponseDto

        // ⬇️ AQUÍ EL FIX: mapear la LISTA interna, no el contenedor
        var ninjas = (listDto?.Ninjas ?? new List<NarutoApi.Infrastructure.Soap.Dtos.NinjaResponseDto>())
            .Select(NinjaMapper.ToModel)
            .ToList();

        var total = listDto?.TotalRecords ?? ninjas.Count;
        return (ninjas, total);
    }
    catch (FaultException ex)
    {
        _logger.LogError(ex, "SOAP fault while listing ninjas");
        throw new UpstreamUnavailableException("Error fetching ninjas");
    }
}

}
