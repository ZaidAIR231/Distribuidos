package com.onion.pikmins.endpoint;

import com.onion.pikmins.config.WebServiceConfig;
import com.onion.pikmins.mapper.PikminMapper;
import com.onion.pikmins.model.Pikmin;
import com.onion.pikmins.service.PikminService;
import com.onion.pikmins.validator.PikminValidator;
import com.onion.pikmins.ws.CreatePikminRequest;
import com.onion.pikmins.ws.CreatePikminResponse;
import com.onion.pikmins.ws.GetPikminByIdRequest;
import com.onion.pikmins.ws.GetPikminByIdResponse;
import org.springframework.ws.server.endpoint.annotation.Endpoint;
import org.springframework.ws.server.endpoint.annotation.PayloadRoot;
import org.springframework.ws.server.endpoint.annotation.RequestPayload;
import org.springframework.ws.server.endpoint.annotation.ResponsePayload;

import java.util.UUID;

@Endpoint
public class PikminEndpoint {

    private final PikminService service;

    public PikminEndpoint(PikminService service) {
        this.service = service;
    }

    @PayloadRoot(namespace = WebServiceConfig.NAMESPACE_URI, localPart = "createPikminRequest")
    @ResponsePayload
    public CreatePikminResponse create(@RequestPayload CreatePikminRequest request) {
        PikminValidator.validate(request.getPikmin());
        Pikmin entity = PikminMapper.toEntity(request.getPikmin());
        Pikmin saved = service.create(entity);

        CreatePikminResponse response = new CreatePikminResponse();
        response.setPikmin(PikminMapper.toWs(saved));
        return response;
    }

    @PayloadRoot(namespace = WebServiceConfig.NAMESPACE_URI, localPart = "getPikminByIdRequest")
    @ResponsePayload
    public GetPikminByIdResponse getById(@RequestPayload GetPikminByIdRequest request) {
        UUID id = UUID.fromString(request.getId());
        Pikmin found = service.getById(id).orElseThrow(() -> new RuntimeException("Pikmin not found: " + id));
        GetPikminByIdResponse response = new GetPikminByIdResponse();
        response.setPikmin(PikminMapper.toWs(found));
        return response;
    }
}