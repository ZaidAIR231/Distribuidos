package com.onion.piklopedia.api.dto;

import jakarta.validation.constraints.*;

// DTO para actualizaciones parciales (PATCH)
public class PatchPikminDto {

  // Nombre del capitán (opcional, máx 100)
  @Size(max = 100)
  private String captainName;   // null = no cambia

  // Color del Pikmin (opcional, máx 50)
  @Size(max = 50)
  private String color;         // null = no cambia

  // Cantidad de onions (1–99, opcional)
  @Min(1) @Max(99)
  private Integer onionCount;   // null = no cambia

  // Hábitat del Pikmin (opcional, máx 100)
  @Size(max = 100)
  private String habitat;       // null = no cambia

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
