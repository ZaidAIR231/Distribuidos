package com.onion.piklopedia.config;

import io.swagger.v3.oas.models.OpenAPI;
import io.swagger.v3.oas.models.info.Info;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

// Configuración de Swagger/OpenAPI para la documentación de la API
@Configuration
public class OpenApiConfig {

  // Define la información general del esquema OpenAPI (Swagger UI)
  @Bean
  public OpenAPI apiInfo() {
    return new OpenAPI().info(new Info()
        .title("Piklopedia API")           // Título visible en Swagger
        .version("v1")                     // Versión del API
        .description("REST facade for Pikmin SOAP")); // Descripción corta
  }
}
