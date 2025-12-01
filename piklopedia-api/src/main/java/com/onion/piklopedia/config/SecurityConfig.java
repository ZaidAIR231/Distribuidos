package com.onion.piklopedia.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.http.HttpMethod;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.web.SecurityFilterChain;

@Configuration
public class SecurityConfig {

  @Bean
  SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {
    http
      .csrf(csrf -> csrf.disable())
      .authorizeHttpRequests(auth -> auth
        // Endpoints públicos (Swagger y health)
        .requestMatchers(
          "/v3/api-docs/**",
          "/swagger-ui/**",
          "/swagger-ui.html",
          "/actuator/health",
          "/health/**"
        ).permitAll()

        // Lectura protegida por scope "read"
        .requestMatchers(HttpMethod.GET, "/pikmin/**").hasAuthority("SCOPE_read")
        .requestMatchers(HttpMethod.GET, "/pikmin").hasAuthority("SCOPE_read")

        // Escritura protegida por scope "write"
        .requestMatchers(HttpMethod.POST,   "/pikmin/**").hasAuthority("SCOPE_write")
        .requestMatchers(HttpMethod.PUT,    "/pikmin/**").hasAuthority("SCOPE_write")
        .requestMatchers(HttpMethod.PATCH,  "/pikmin/**").hasAuthority("SCOPE_write")
        .requestMatchers(HttpMethod.DELETE, "/pikmin/**").hasAuthority("SCOPE_write")

        // Cualquier otra ruta autenticada
        .anyRequest().authenticated()
      )
      .oauth2ResourceServer(oauth -> oauth.jwt());

    return http.build();
  }
}
