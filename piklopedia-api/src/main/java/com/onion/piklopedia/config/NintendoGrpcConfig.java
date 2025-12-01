package com.onion.piklopedia.config;

import com.onion.nintendo.grpc.NintendoServiceGrpc;
import io.grpc.ManagedChannel;
import io.grpc.ManagedChannelBuilder;
import jakarta.annotation.PreDestroy;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

/**
 * Configura el cliente gRPC hacia NintendoService.
 */
@Configuration
public class NintendoGrpcConfig {

    private ManagedChannel channel;

    /**
     * Crea el canal gRPC hacia el microservicio de Nintendo.
     */
    @Bean
    public ManagedChannel nintendoChannel(
            @Value("${nintendo.grpc.host}") String host,
            @Value("${nintendo.grpc.port}") int port
    ) {
        this.channel = ManagedChannelBuilder
                .forAddress(host, port)
                .usePlaintext()
                .build();

        return this.channel;
    }

    /**
     * Crea el stub bloqueante a partir del canal.
     */
    @Bean
    public NintendoServiceGrpc.NintendoServiceBlockingStub nintendoBlockingStub(
            ManagedChannel nintendoChannel
    ) {
        return NintendoServiceGrpc.newBlockingStub(nintendoChannel);
    }

    /**
     * Cierra el canal al apagar el contexto de Spring.
     */
    @PreDestroy
    public void shutdownChannel() {
        if (this.channel != null && !this.channel.isShutdown()) {
            this.channel.shutdown();
        }
    }
}
