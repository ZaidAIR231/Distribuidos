package com.onion.piklopedia.api.mapper;

import com.onion.piklopedia.api.dto.CreatePikminDto;
import com.onion.piklopedia.api.dto.PikminDto;
import com.onion.piklopedia.domain.model.Pikmin;
import org.springframework.stereotype.Component;

// Convierte entre entidades y DTOs de Pikmin
@Component
public class PikminMapper {

  // Convierte un CreatePikminDto en entidad Pikmin
  public Pikmin toEntity(CreatePikminDto dto) {
    Pikmin p = new Pikmin();
    p.setCaptainName(dto.getCaptainName());
    p.setColor(dto.getColor());
    p.setOnionCount(dto.getOnionCount());
    p.setHabitat(dto.getHabitat());
    return p;
  }

  // Convierte una entidad Pikmin a PikminDto (salida)
  public PikminDto toDto(Pikmin p) {
    return new PikminDto(p.getId(), p.getCaptainName(), p.getColor(), p.getOnionCount(), p.getHabitat());
  }
}
