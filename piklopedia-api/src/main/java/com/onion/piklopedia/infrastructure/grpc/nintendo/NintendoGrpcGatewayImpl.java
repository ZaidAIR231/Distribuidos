package com.onion.piklopedia.infrastructure.grpc.nintendo;

import com.onion.nintendo.grpc.ConsoleResponse;
import com.onion.nintendo.grpc.CreateConsoleRequest;
import com.onion.nintendo.grpc.DeleteConsoleRequest;
import com.onion.nintendo.grpc.DeleteConsoleResponse;
import com.onion.nintendo.grpc.GetConsoleByIdRequest;
import com.onion.nintendo.grpc.ListConsolesRequest;
import com.onion.nintendo.grpc.ListConsolesResponse;
import com.onion.nintendo.grpc.NintendoServiceGrpc;
import com.onion.nintendo.grpc.UpdateConsoleRequest;
import org.springframework.stereotype.Component;

/**
 * Implementación del gateway hacia el microservicio gRPC de Nintendo.
 *
 * Esta clase envuelve al stub gRPC generado por Protobuf y expone métodos
 * tipados que el resto de la aplicación puede usar sin conocer los detalles
 * de gRPC.
 */
@Component
public class NintendoGrpcGatewayImpl implements NintendoGrpcGateway {

    /**
     * Stub bloqueante generado a partir de nintendo.proto.
     * Se inyecta desde la configuración de gRPC (NintendoGrpcConfig),
     * donde se construye el canal y el stub.
     */
    private final NintendoServiceGrpc.NintendoServiceBlockingStub stub;

    /**
     * Inyección por constructor.
     * Spring Boot crea esta clase y le pasa el stub listo para usarse.
     */
    public NintendoGrpcGatewayImpl(NintendoServiceGrpc.NintendoServiceBlockingStub stub) {
        this.stub = stub;
    }

    /**
     * Llama al método gRPC CreateConsole en el servidor Nintendo.
     */
    @Override
    public ConsoleResponse createConsole(CreateConsoleRequest request) {
        return stub.createConsole(request);
    }

    /**
     * Llama al método gRPC GetConsoleById para obtener una consola por id.
     */
    @Override
    public ConsoleResponse getConsoleById(GetConsoleByIdRequest request) {
        return stub.getConsoleById(request);
    }

    /**
     * Llama al método gRPC ListConsoles para obtener el listado de consolas.
     */
    @Override
    public ListConsolesResponse listConsoles(ListConsolesRequest request) {
        return stub.listConsoles(request);
    }

    /**
     * Llama al método gRPC UpdateConsole para actualizar una consola existente.
     */
    @Override
    public ConsoleResponse updateConsole(UpdateConsoleRequest request) {
        return stub.updateConsole(request);
    }

    /**
     * Llama al método gRPC DeleteConsole para eliminar una consola por id.
     */
    @Override
    public DeleteConsoleResponse deleteConsole(DeleteConsoleRequest request) {
        return stub.deleteConsole(request);
    }
}
