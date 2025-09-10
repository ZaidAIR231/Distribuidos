using System;
using System.ServiceModel;
using NarutoApi.Dtos;

namespace NarutoApi.Validators;

public static class NinjaValidators
{

    private static void Req(bool ok, string msg)
    {
        if (!ok) throw new FaultException(msg);
    }

    public static CreateNinjaDto Validate(this CreateNinjaDto n)
    {
        Req(!string.IsNullOrWhiteSpace(n.Name), "Ninja name is required");
        Req(!string.IsNullOrWhiteSpace(n.Village), "Ninja village is required");
        Req(!string.IsNullOrWhiteSpace(n.Rank), "Ninja rank is required");
        Req(!string.IsNullOrWhiteSpace(n.NinJutsu), "NinJutsu is required");
        Req(n.Chakra is >= 1 and <= 100, "Chakra must be between 1 and 100");
        return n;
    }

    // Update
    public static UpdateNinjaDto Validate(this UpdateNinjaDto n)
    {
        Req(n.Id != Guid.Empty, "Ninja id is required");
        Req(!string.IsNullOrWhiteSpace(n.Name), "Ninja name is required");
        Req(!string.IsNullOrWhiteSpace(n.Village), "Ninja village is required");
        Req(!string.IsNullOrWhiteSpace(n.Rank), "Ninja rank is required");
        Req(!string.IsNullOrWhiteSpace(n.NinJutsu), "NinJutsu is required");
        Req(n.Chakra is >= 1 and <= 100, "Chakra must be between 1 and 100");
        return n;
    }

    public static SearchNinjaDto Validate(this SearchNinjaDto f)
    {
        var any = !string.IsNullOrWhiteSpace(f.Village)
               || !string.IsNullOrWhiteSpace(f.Rank)
               || !string.IsNullOrWhiteSpace(f.NinJutsu)
               || f.ChakraMin.HasValue
               || f.ChakraMax.HasValue;
        Req(any, "At least one filter (Village, Rank, NinJutsu, ChakraMin, ChakraMax) is required");

        if (f.ChakraMin.HasValue || f.ChakraMax.HasValue)
        {
            Req(!f.ChakraMin.HasValue || f.ChakraMin is >= 1 and <= 100, "ChakraMin must be between 1 and 100");
            Req(!f.ChakraMax.HasValue || f.ChakraMax is >= 1 and <= 100, "ChakraMax must be between 1 and 100");
            Req(!(f.ChakraMin.HasValue && f.ChakraMax.HasValue) || f.ChakraMin <= f.ChakraMax, "ChakraMin must be less than or equal to ChakraMax");
        }
        return f;
    }
}
