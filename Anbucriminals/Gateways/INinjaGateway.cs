using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Anbucriminals.Models;

namespace Anbucriminals.Gateways;

public interface INinjaGateway
{
    Task<Ninja?> GetNinjaByIdAsync(Guid id, CancellationToken cancellationToken);
}
