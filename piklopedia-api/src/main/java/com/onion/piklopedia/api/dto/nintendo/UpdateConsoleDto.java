package com.onion.piklopedia.api.dto.nintendo;

import jakarta.validation.constraints.*;
import java.util.List;

public record UpdateConsoleDto(

        @NotBlank(message = "name is required")
        String name,

        String codeName,

        @NotNull(message = "generation is required")
        @Min(value = 1, message = "generation must be > 0")
        Integer generation,

        @NotNull(message = "releaseYear is required")
        @Min(value = 1970, message = "releaseYear is too small")
        @Max(value = 2100, message = "releaseYear is too large")
        Integer releaseYear,

        @Size(max = 500, message = "description too long")
        String description,

        Boolean portable,

        @Email(message = "supportEmail must be a valid email")
        String supportEmail,

        List<@NotBlank(message = "region cannot be blank") String> regions
) {}
