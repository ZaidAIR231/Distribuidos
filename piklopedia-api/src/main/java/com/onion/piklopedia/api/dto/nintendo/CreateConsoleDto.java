package com.onion.piklopedia.api.dto.nintendo;

import jakarta.validation.constraints.*;

import java.util.List;

public record CreateConsoleDto(

        @NotBlank(message = "name is required")
        String name,

        // opcional
        String codeName,

        @NotNull(message = "generation is required")
        @Min(value = 1, message = "generation must be > 0")
        Integer generation,

        @NotNull(message = "releaseYear is required")
        @Min(value = 1970, message = "releaseYear is too small")
        @Max(value = 2100, message = "releaseYear is too large")
        Integer releaseYear,

        // opcional, pero con longitud máxima si viene
        @Size(max = 500, message = "description too long")
        String description,

        // opcional: si viene null, en el servicio puedes asumir false
        Boolean portable,

        // opcional, pero si se envía debe ser email
        @Email(message = "supportEmail must be a valid email")
        String supportEmail,

        // opcional: puedes permitir null o lista vacía
        List<@NotBlank(message = "region cannot be blank") String> regions
) {}
