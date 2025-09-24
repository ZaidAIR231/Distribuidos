using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Anbucriminals.Mappers;
using Anbucriminals.Models;
using NarutoApi.Infraestructure.SOAP.Contracts;
using Microsoft.Extensions.Logging;

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
}

  