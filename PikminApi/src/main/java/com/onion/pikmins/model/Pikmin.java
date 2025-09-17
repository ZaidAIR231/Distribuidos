package com.onion.pikmins.model;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

import java.util.UUID;

@Entity
@Table(name = "pikmins")
public class Pikmin {

    @Id
    @Column(name = "id", nullable = false)
    private UUID id;

    @Column(name = "captain_name", nullable = false, length = 100)
    private String captainName;

    @Column(name = "color", nullable = false, length = 40)
    private String color;

    @Column(name = "onion_count", nullable = false)
    private Integer onionCount;

    @Column(name = "habitat", nullable = false, length = 100)
    private String habitat;

    public Pikmin() {}

    public UUID getId() { return id; }
    public void setId(UUID id) { this.id = id; }

    public String getCaptainName() { return captainName; }
    public void setCaptainName(String captainName) { this.captainName = captainName; }

    public String getColor() { return color; }
    public void setColor(String color) { this.color = color; }

    public Integer getOnionCount() { return onionCount; }
    public void setOnionCount(Integer onionCount) { this.onionCount = onionCount; }

    public String getHabitat() { return habitat; }
    public void setHabitat(String habitat) { this.habitat = habitat; }
}