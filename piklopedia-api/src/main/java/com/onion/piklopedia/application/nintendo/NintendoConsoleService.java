package com.onion.piklopedia.application.nintendo;

import com.onion.nintendo.grpc.ConsoleResponse;
import com.onion.nintendo.grpc.CreateConsoleRequest;
import com.onion.nintendo.grpc.DeleteConsoleRequest;
import com.onion.nintendo.grpc.DeleteConsoleResponse;
import com.onion.nintendo.grpc.GetConsoleByIdRequest;
import com.onion.nintendo.grpc.ListConsolesRequest;
import com.onion.nintendo.grpc.ListConsolesResponse;
import com.onion.nintendo.grpc.UpdateConsoleRequest;
import com.onion.piklopedia.api.dto.nintendo.ConsoleDto;
import com.onion.piklopedia.api.dto.nintendo.CreateConsoleDto;
import com.onion.piklopedia.api.dto.nintendo.UpdateConsoleDto;
import com.onion.piklopedia.infrastructure.grpc.nintendo.NintendoGrpcGateway;
// import lombok.RequiredArgsConstructor;  
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class NintendoConsoleService {

    private final NintendoGrpcGateway gateway;

   
    public NintendoConsoleService(NintendoGrpcGateway gateway) {
        this.gateway = gateway;
    }

    // ===== CREATE =====
    public ConsoleDto createConsole(CreateConsoleDto dto) {

        com.onion.nintendo.grpc.Console consoleMsg =
                com.onion.nintendo.grpc.Console.newBuilder()
                        .setName(dto.name())
                        .setCodeName(dto.codeName())
                        .setGeneration(dto.generation())
                        .setReleaseYear(dto.releaseYear())
                        .setDescription(dto.description())
                        .setPortable(dto.portable())
                        .setSupportEmail(dto.supportEmail())
                        .addAllRegions(dto.regions())
                        .build();

        CreateConsoleRequest request = CreateConsoleRequest.newBuilder()
                .setConsole(consoleMsg)
                .build();

        ConsoleResponse response = gateway.createConsole(request);

        return toDto(response.getConsole());
    }

    // ===== GET BY ID =====
    public ConsoleDto getConsoleById(String id) {

        GetConsoleByIdRequest request = GetConsoleByIdRequest.newBuilder()
                .setId(id)
                .build();

        ConsoleResponse response = gateway.getConsoleById(request);

        return toDto(response.getConsole());
    }

    // ===== LIST =====
    public List<ConsoleDto> listConsoles() {

        ListConsolesRequest request = ListConsolesRequest.newBuilder().build();

        ListConsolesResponse response = gateway.listConsoles(request);

        return response.getConsolesList()
                .stream()
                .map(this::toDto)
                .toList();
    }

    // ===== UPDATE =====
    public ConsoleDto updateConsole(String id, UpdateConsoleDto dto) {

        com.onion.nintendo.grpc.Console consoleMsg =
                com.onion.nintendo.grpc.Console.newBuilder()
                        .setName(dto.name())
                        .setCodeName(dto.codeName())
                        .setGeneration(dto.generation())
                        .setReleaseYear(dto.releaseYear())
                        .setDescription(dto.description())
                        .setPortable(dto.portable())
                        .setSupportEmail(dto.supportEmail())
                        .addAllRegions(dto.regions())
                        .build();

        
        UpdateConsoleRequest request = UpdateConsoleRequest.newBuilder()
                .setId(id)
                .setConsole(consoleMsg)
                .build();

        ConsoleResponse response = gateway.updateConsole(request);

        return toDto(response.getConsole());
    }

    // ===== DELETE =====
    public boolean deleteConsole(String id) {

        DeleteConsoleRequest request = DeleteConsoleRequest.newBuilder()
                .setId(id)
                .build();

        DeleteConsoleResponse response = gateway.deleteConsole(request);

        return response.getSuccess();
    }

    // ===== MAPPER gRPC -> DTO REST =====
    private ConsoleDto toDto(com.onion.nintendo.grpc.Console c) {
        return new ConsoleDto(
                c.getId(),
                c.getName(),
                c.getCodeName(),
                c.getGeneration(),
                c.getReleaseYear(),
                c.getDescription(),
                c.getPortable(),
                c.getSupportEmail(),
                c.getRegionsList()
        );
    }
}
