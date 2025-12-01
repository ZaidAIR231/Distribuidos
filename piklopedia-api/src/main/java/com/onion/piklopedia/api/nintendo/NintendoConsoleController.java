package com.onion.piklopedia.api.nintendo;

import com.onion.piklopedia.api.dto.nintendo.ConsoleDto;
import com.onion.piklopedia.api.dto.nintendo.CreateConsoleDto;
import com.onion.piklopedia.api.dto.nintendo.UpdateConsoleDto;
import com.onion.piklopedia.application.nintendo.NintendoConsoleService;
import jakarta.validation.Valid;
import jakarta.validation.constraints.Pattern;
import jakarta.validation.constraints.Size;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.net.URI;
import java.util.List;

@RestController
@RequestMapping("/nintendo/consoles")
@Validated // <- importante para validar @PathVariable
public class NintendoConsoleController {

    private final NintendoConsoleService service;

    public NintendoConsoleController(NintendoConsoleService service) {
        this.service = service;
    }

    // POST /nintendo/consoles
    @PostMapping
    public ResponseEntity<ConsoleDto> create(@Valid @RequestBody CreateConsoleDto dto) {
        ConsoleDto created = service.createConsole(dto);
        return ResponseEntity
                .created(URI.create("/nintendo/consoles/" + created.id()))
                .body(created);
    }

    // GET /nintendo/consoles/{id}
    @GetMapping("/{id}")
    public ResponseEntity<ConsoleDto> getById(
            @PathVariable
            @Size(min = 24, max = 24, message = "id must be 24 characters long")
            @Pattern(regexp = "^[0-9a-fA-F]{24}$", message = "id must be a valid hex ObjectId")
            String id
    ) {
        ConsoleDto console = service.getConsoleById(id);
        return ResponseEntity.ok(console);
    }

    // GET /nintendo/consoles
    @GetMapping
    public ResponseEntity<List<ConsoleDto>> list() {
        List<ConsoleDto> consoles = service.listConsoles();
        return ResponseEntity.ok(consoles);
    }

    // PUT /nintendo/consoles/{id}
    @PutMapping("/{id}")
    public ResponseEntity<ConsoleDto> update(
            @PathVariable
            @Size(min = 24, max = 24, message = "id must be 24 characters long")
            @Pattern(regexp = "^[0-9a-fA-F]{24}$", message = "id must be a valid hex ObjectId")
            String id,
            @Valid @RequestBody UpdateConsoleDto dto
    ) {
        ConsoleDto updated = service.updateConsole(id, dto);
        return ResponseEntity.ok(updated);
    }

    // DELETE /nintendo/consoles/{id}
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> delete(
            @PathVariable
            @Size(min = 24, max = 24, message = "id must be 24 characters long")
            @Pattern(regexp = "^[0-9a-fA-F]{24}$", message = "id must be a valid hex ObjectId")
            String id
    ) {
        boolean success = service.deleteConsole(id);
        if (success) {
            return ResponseEntity.noContent().build();
        } else {
            return ResponseEntity.notFound().build();
        }
    }
}
