package com.onion.piklopedia.domain.model;

import jakarta.persistence.*;
import lombok.*;
import java.util.UUID;

@Entity
@Table(name = "pikmins")
@Getter @Setter @NoArgsConstructor @AllArgsConstructor @Builder
public class Pikmin {

    @Id
    private UUID id;

    @Column(name = "captain_name", nullable = false)
    private String captainName;

    @Column(nullable = false)
    private String color;

    @Column(name = "onion_count", nullable = false)
    private int onionCount;

    @Column(nullable = false)
    private String habitat;

    // getters & setters
    public UUID getId() { return id; }
    public void setId(UUID id) { this.id = id; }
    public String getCaptainName() { return captainName; }
    public void setCaptainName(String captainName) { this.captainName = captainName; }
    public String getColor() { return color; }
    public void setColor(String color) { this.color = color; }
    public int getOnionCount() { return onionCount; }
    public void setOnionCount(int onionCount) { this.onionCount = onionCount; }
    public String getHabitat() { return habitat; }
    public void setHabitat(String habitat) { this.habitat = habitat; }
}
