package com.onion.piklopedia.api.dto;

import io.swagger.v3.oas.annotations.media.Schema;
import jakarta.validation.constraints.*;

// DTO para crear un nuevo Pikmin (usado en POST)
@Schema(name = "CreatePikminDto")
public class CreatePikminDto {

  // Nombre del capitán responsable (obligatorio)
  @NotBlank(message = "captainName es obligatorio")
  private String captainName;

  // Color del Pikmin (obligatorio)
  @NotBlank(message = "color es obligatorio")
  private String color;

  // Número de onions asociados (1–99)
  @NotNull(message = "onionCount es obligatorio")
  @Min(value = 1, message = "onionCount mínimo 1")
  @Max(value = 99, message = "onionCount máximo 99")
  private Integer onionCount;

  // Hábitat del Pikmin (opcional)
  private String habitat;

  // Getters y setters
  public String getCaptainName() { return captainName; }
  public void setCaptainName(String captainName) { this.captainName = captainName; }

  public String getColor() { return color; }
  public void setColor(String color) { this.color = color; }

  public Integer getOnionCount() { return onionCount; }
  public void setOnionCount(Integer onionCount) { this.onionCount = onionCount; }

  public String getHabitat() { return habitat; }
  public void setHabitat(String habitat) { this.habitat = habitat; }
}
