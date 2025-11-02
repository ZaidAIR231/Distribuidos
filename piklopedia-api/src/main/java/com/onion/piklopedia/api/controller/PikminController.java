package com.onion.piklopedia.api.controller;

import com.onion.piklopedia.api.dto.CreatePikminDto;
import com.onion.piklopedia.api.dto.PatchPikminDto;
import com.onion.piklopedia.api.dto.PikminDto;
import com.onion.piklopedia.api.dto.UpdatePikminDto;
import com.onion.piklopedia.application.PikminService;
import jakarta.validation.Valid;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.WebDataBinder;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.support.ServletUriComponentsBuilder;

import java.beans.PropertyEditorSupport;
import java.util.List;
import java.util.UUID;

// Controlador REST principal para manejar Pikmin (CRUD completo)
@RestController
@RequestMapping("/pikmin")
public class PikminController {

    // Inyección del servicio de negocio
    private final PikminService service;
    public PikminController(PikminService service) { this.service = service; }

    // GET /pikmin/{id} → devuelve un Pikmin por su UUID
    @GetMapping(value = "/{id}", name = "GetPikminById")
    public ResponseEntity<PikminDto> getById(@PathVariable UUID id) {
        return ResponseEntity.ok(service.getById(id));
    }

    // GET /pikmin → listado paginado (a futuro con sort y filter)
    @GetMapping(name = "ListPikmin")
    public ResponseEntity<List<PikminDto>> list(
            @RequestParam(name="page", defaultValue="0") Integer page,
            @RequestParam(name="pageSize", defaultValue="10") Integer pageSize,
            @RequestParam(name="sort", required=false) String sort,
            @RequestParam(name="filter", required=false) String filter) {
        // Paginado pendiente de implementar
        return ResponseEntity.ok(List.of());
    }

    // Permite recibir UUID con o sin guiones en los path params
    @InitBinder
    public void initBinder(WebDataBinder binder) {
        binder.registerCustomEditor(UUID.class, new PropertyEditorSupport() {
            @Override public void setAsText(String text) {
                String s = text == null ? "" : text.trim().replace("-", "");
                if (!s.matches("^[0-9a-fA-F]{32}$")) throw new IllegalArgumentException("Invalid UUID");
                s = s.replaceFirst("(\\p{XDigit}{8})(\\p{XDigit}{4})(\\p{XDigit}{4})(\\p{XDigit}{4})(\\p{XDigit}{12})","$1-$2-$3-$4-$5");
                setValue(UUID.fromString(s));
            }
        });
    }

    // POST /pikmin → crea un nuevo Pikmin y devuelve 201 + Location
    @PostMapping(name = "CreatePikmin")
    public ResponseEntity<PikminDto> create(@Valid @RequestBody CreatePikminDto dto) {
        PikminDto created = service.create(dto);
        var location = ServletUriComponentsBuilder
                .fromCurrentRequest()
                .path("/{id}")
                .buildAndExpand(created.id())
                .toUri();
        return ResponseEntity.created(location).body(created);
    }

    // PUT /pikmin/{id} → reemplaza completamente un Pikmin
    @PutMapping(value = "/{id}", name = "UpdatePikmin")
    public ResponseEntity<PikminDto> update(
            @PathVariable UUID id,
            @Valid @RequestBody UpdatePikminDto dto) {
        return ResponseEntity.ok(service.update(id, dto));
    }

    // PATCH /pikmin/{id} → actualiza parcialmente un Pikmin
    @PatchMapping(value = "/{id}", name = "PatchPikmin")
    public ResponseEntity<PikminDto> patch(@PathVariable UUID id, @Valid @RequestBody PatchPikminDto dto) {
        var updated = service.patch(id, dto);
        return ResponseEntity.ok(updated);
    }

    // DELETE /pikmin/{id} → elimina un Pikmin (204 No Content)
    @DeleteMapping(value = "/{id}", name = "DeletePikmin")
    public ResponseEntity<Void> delete(@PathVariable UUID id) {
        service.delete(id);
        return ResponseEntity.noContent().build();
    }

}
