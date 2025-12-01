package com.onion.piklopedia.api.dto;

import jakarta.validation.constraints.*;

public class UpdatePikminDto {
  @NotBlank @Size(max=100)
  private String captainName;

  @NotBlank @Size(max=50)
  private String color;

  @Min(1) @Max(99)
  private int onionCount;

  @Size(max=100)
  private String habitat;

  // getters/setters
  public String getCaptainName() { return captainName; }
  public void setCaptainName(String captainName) { this.captainName = captainName; }
  public String getColor() { return color; }
  public void setColor(String color) { this.color = color; }
  public int getOnionCount() { return onionCount; }
  public void setOnionCount(int onionCount) { this.onionCount = onionCount; }
  public String getHabitat() { return habitat; }
  public void setHabitat(String habitat) { this.habitat = habitat; }
}
