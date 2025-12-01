package com.onion.piklopedia.api.advice;

import java.time.Instant;
import java.util.Map;

/**
 * Modelo estándar de error para toda la API.
 */
public class ApiError {

    private final Instant timestamp;
    private final int status;
    private final String error;      // "Bad Request", "Not Found", etc.
    private final String code;       // código interno: VALIDATION_ERROR, GRPC_NOT_FOUND, etc.
    private final String message;    // mensaje general
    private final String path;       // endpoint
    private final Map<String, String> fieldErrors; // errores por campo (opcional)

    public ApiError(int status,
                    String error,
                    String code,
                    String message,
                    String path,
                    Map<String, String> fieldErrors) {
        this.timestamp = Instant.now();
        this.status = status;
        this.error = error;
        this.code = code;
        this.message = message;
        this.path = path;
        this.fieldErrors = fieldErrors;
    }

    public Instant getTimestamp() {
        return timestamp;
    }

    public int getStatus() {
        return status;
    }

    public String getError() {
        return error;
    }

    public String getCode() {
        return code;
    }

    public String getMessage() {
        return message;
    }

    public String getPath() {
        return path;
    }

    public Map<String, String> getFieldErrors() {
        return fieldErrors;
    }
}
