package com.onion.piklopedia.api.dto.nintendo;

import java.util.List;

public record ConsoleDto(
        String id,
        String name,
        String codeName,
        int generation,
        int releaseYear,
        String description,
        boolean portable,
        String supportEmail,
        List<String> regions
) {}
