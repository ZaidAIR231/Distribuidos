package com.onion.piklopedia.api;

import com.onion.piklopedia.infrastructure.soap.gateway.PikminSoapGateway;
import com.onion.pikmin.ws.GetPikminByIdRequest;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

// Endpoint de salud para probar comunicación con el SOAP
@RestController
@RequiredArgsConstructor
@RequestMapping("/health")
public class PikminHealthController {

    // Gateway que consume el servicio SOAP
    private final PikminSoapGateway gateway;

    // GET /health/soap → verifica si el servicio SOAP responde correctamente
    @GetMapping("/soap")
    public ResponseEntity<?> pingSoap() {
        var req = new GetPikminByIdRequest();
        req.setId(UUID.randomUUID().toString());
        try {
            gateway.getById(req);
            return ResponseEntity.ok(new Status("ok", "SOAP reachable"));
        } catch (Exception e) {
            return ResponseEntity.status(502)
                    .body(new Status("down", "SOAP unreachable: " + e.getClass().getSimpleName()));
        }
    }

    // Objeto simple de estado para la respuesta
    record Status(String status, String detail) {}
}

