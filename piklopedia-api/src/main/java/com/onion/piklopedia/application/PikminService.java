package com.onion.piklopedia.application;

import com.onion.piklopedia.api.dto.CreatePikminDto;
import com.onion.piklopedia.api.dto.PatchPikminDto;
import com.onion.piklopedia.api.dto.PikminDto;
import com.onion.piklopedia.api.dto.UpdatePikminDto;
import com.onion.piklopedia.api.mapper.PikminMapper;
import com.onion.piklopedia.domain.model.Pikmin;
import com.onion.piklopedia.domain.repository.PikminRepository;
import com.onion.piklopedia.infrastructure.soap.gateway.PikminSoapGateway;
import com.onion.pikmin.ws.GetPikminByIdRequest;
import com.onion.piklopedia.application.exception.ResourceConflictException;
import com.onion.piklopedia.application.exception.ResourceNotFoundException;

//import org.springframework.cache.annotation.CacheEvict;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.UUID;

// Lógica de negocio principal para Pikmin (CRUD + integración SOAP)
@Service
public class PikminService {

  // Dependencias inyectadas
  private final PikminRepository repo;
  private final PikminMapper mapper;
  private final PikminSoapGateway gateway;

  // ✅ Constructor explícito para inyección por constructor
  public PikminService(PikminRepository repo,
                       PikminMapper mapper,
                       PikminSoapGateway gateway) {
    this.repo = repo;
    this.mapper = mapper;
    this.gateway = gateway;
  }

  // Obtiene un Pikmin desde el servicio SOAP usando su UUID
  public PikminDto getById(UUID id) {
    var req = new GetPikminByIdRequest();
    req.setId(id.toString());
    var res = gateway.getById(req);
    var p = res.getPikmin();
    if (p == null) throw new ResourceNotFoundException("Pikmin not found: " + id);

    UUID pid = toUuid(p.getId());
    return new PikminDto(pid, p.getCaptainName(), p.getColor(), p.getOnionCount(), p.getHabitat());
  }

  // Convierte string a UUID seguro (acepta formatos con o sin guiones)
  private static UUID toUuid(String s) {
    if (s == null) throw new IllegalArgumentException("null id");
    try { return UUID.fromString(s.trim()); } catch (Exception ignore) {}
    String ng = s.trim().replace("-", "");
    if (!ng.matches("^[0-9a-fA-F]{32}$")) throw new IllegalArgumentException("Invalid UUID string: " + s);
    String withDashes = ng.replaceFirst(
        "(\\p{XDigit}{8})(\\p{XDigit}{4})(\\p{XDigit}{4})(\\p{XDigit}{4})(\\p{XDigit}{12})",
        "$1-$2-$3-$4-$5"
    );
    return UUID.fromString(withDashes);
  }

  // Crea un nuevo Pikmin y valida duplicados por nombre de capitán
  @Transactional
  // @CacheEvict(cacheNames = {"pikminById","pikminList"}, allEntries = true)
  public PikminDto create(CreatePikminDto dto) {
    if (repo.existsByCaptainNameIgnoreCase(dto.getCaptainName()))
      throw new ResourceConflictException("Ya existe un Pikmin con captainName=" + dto.getCaptainName());

    Pikmin entity = mapper.toEntity(dto);
    entity.setId(UUID.randomUUID());
    Pikmin saved = repo.save(entity);
    return mapper.toDto(saved);
  }

  // Actualiza completamente un Pikmin (PUT)
  @Transactional
  // @CacheEvict(cacheNames = {"pikminById","pikminList"}, allEntries = true)
  public PikminDto update(UUID id, UpdatePikminDto dto) {
    Pikmin entity = repo.findById(id)
        .orElseThrow(() -> new ResourceNotFoundException("Pikmin no encontrado"));

    // Verifica duplicado (captainName + color) excluyendo el mismo id
    if (repo.existsByCaptainNameAndColorAndIdNot(dto.getCaptainName(), dto.getColor(), id))
      throw new ResourceConflictException("Ya existe un Pikmin con ese captainName y color");

    entity.setCaptainName(dto.getCaptainName());
    entity.setColor(dto.getColor());
    entity.setOnionCount(dto.getOnionCount());
    entity.setHabitat(dto.getHabitat());

    return mapper.toDto(repo.save(entity));
  }

  // Actualiza parcialmente un Pikmin (PATCH)
  @Transactional
  // @CacheEvict(cacheNames = {"pikminById","pikminList"}, allEntries = true)
  public PikminDto patch(UUID id, PatchPikminDto dto) {
    Pikmin entity = repo.findById(id)
        .orElseThrow(() -> new ResourceNotFoundException("Pikmin no encontrado"));

    String newCaptain = dto.getCaptainName() != null ? dto.getCaptainName() : entity.getCaptainName();
    String newColor   = dto.getColor()       != null ? dto.getColor()       : entity.getColor();

    if ((!newCaptain.equals(entity.getCaptainName()) || !newColor.equals(entity.getColor()))
        && repo.existsByCaptainNameAndColorAndIdNot(newCaptain, newColor, id))
      throw new ResourceConflictException("Ya existe un Pikmin con ese captainName y color");

    if (dto.getCaptainName() != null) entity.setCaptainName(dto.getCaptainName());
    if (dto.getColor() != null)       entity.setColor(dto.getColor());
    if (dto.getOnionCount() != null)  entity.setOnionCount(dto.getOnionCount());
    if (dto.getHabitat() != null)     entity.setHabitat(dto.getHabitat());

    return mapper.toDto(repo.save(entity));
  }

  // Elimina un Pikmin por su UUID
  @Transactional
  // @CacheEvict(cacheNames = {"pikminById","pikminList"}, allEntries = true)
  public void delete(UUID id) {
    if (!repo.existsById(id)) {
      throw new ResourceNotFoundException("Pikmin no encontrado");
    }
    repo.deleteById(id);
  }
}
