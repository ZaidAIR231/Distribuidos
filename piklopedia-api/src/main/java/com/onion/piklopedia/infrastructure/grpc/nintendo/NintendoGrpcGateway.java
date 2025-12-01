package com.onion.piklopedia.infrastructure.grpc.nintendo;

import com.onion.nintendo.grpc.ConsoleResponse;
import com.onion.nintendo.grpc.CreateConsoleRequest;
import com.onion.nintendo.grpc.DeleteConsoleRequest;
import com.onion.nintendo.grpc.DeleteConsoleResponse;
import com.onion.nintendo.grpc.GetConsoleByIdRequest;
import com.onion.nintendo.grpc.ListConsolesRequest;
import com.onion.nintendo.grpc.ListConsolesResponse;
import com.onion.nintendo.grpc.UpdateConsoleRequest;

/**
 * Contrato del cliente gRPC hacia NintendoService.
 */
public interface NintendoGrpcGateway {

    /**
     * Invoca gRPC CreateConsole.
     */
    ConsoleResponse createConsole(CreateConsoleRequest request);

    /**
     * Invoca gRPC GetConsoleById.
     */
    ConsoleResponse getConsoleById(GetConsoleByIdRequest request);

    /**
     * Invoca gRPC ListConsoles.
     */
    ListConsolesResponse listConsoles(ListConsolesRequest request);

    /**
     * Invoca gRPC UpdateConsole.
     */
    ConsoleResponse updateConsole(UpdateConsoleRequest request);

    /**
     * Invoca gRPC DeleteConsole.
     */
    DeleteConsoleResponse deleteConsole(DeleteConsoleRequest request);
}

