using System.ServiceModel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PokemonApi.Models;
using SoapCore;
using PokemonApi.Dtos;

namespace PokemonApi.Validators;

public static class PokemonValidator{
    public static CreatePokemonDto ValidateName(this CreatePokemonDto pokemon) =>
    string.IsNullOrEmpty(pokemon.Name) ? throw new FaultException("Pokemon name is required") : pokemon;

    public static CreatePokemonDto ValidateType(this CreatePokemonDto pokemon) =>
    string.IsNullOrEmpty(pokemon.Type) ? throw new FaultException("Pokemon Type is required") : pokemon;

    public static CreatePokemonDto ValidateLevel(this CreatePokemonDto pokemon) =>
    pokemon.Level <= 0 ? throw new FaultException("Pokemon level is required") : pokemon;
}