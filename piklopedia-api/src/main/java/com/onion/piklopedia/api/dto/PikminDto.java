package com.onion.piklopedia.api.dto;

import io.swagger.v3.oas.annotations.media.Schema;
import java.util.UUID;

// DTO de salida principal para respuestas de la API
@Schema(name = "PikminDto")
public record PikminDto(

        // Identificador único del Pikmin
        UUID id,

        // Nombre del capitán asociado
        String captainName,

        // Color del Pikmin
        String color,

        // Cantidad de onions
        int onionCount,

        // Hábitat del Pikmin
        String habitat
) {}
