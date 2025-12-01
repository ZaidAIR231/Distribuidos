package com.onion.piklopedia.api.advice;

import jakarta.validation.ConstraintViolationException;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.FieldError;
import org.springframework.web.bind.MethodArgumentNotValidException;
import org.springframework.web.bind.annotation.*;
import com.onion.piklopedia.application.exception.ResourceConflictException;
import com.onion.piklopedia.application.exception.ResourceNotFoundException;

import java.util.HashMap;
import java.util.Map;

// Manejo global de excepciones para toda la API
@RestControllerAdvice
public class GlobalExceptionHandler {

  // Error de validación en DTOs con @Valid → 400
  @ExceptionHandler(MethodArgumentNotValidException.class)
  public ResponseEntity<?> handleValidation(MethodArgumentNotValidException ex) {
    Map<String, String> errors = new HashMap<>();
    for (FieldError fe : ex.getBindingResult().getFieldErrors()) {
      errors.put(fe.getField(), fe.getDefaultMessage());
    }
    return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(Map.of(
        "error", "Bad Request",
        "detail", errors
    ));
  }

  // Error de validación en parámetros → 400
  @ExceptionHandler(ConstraintViolationException.class)
  public ResponseEntity<?> handleConstraint(ConstraintViolationException ex) {
    return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(Map.of(
        "error", "Bad Request",
        "detail", ex.getMessage()
    ));
  }

  // Violación de integridad en base de datos → 409
  @ExceptionHandler(DataIntegrityViolationException.class)
  @ResponseStatus(HttpStatus.CONFLICT)
  public Map<String, Object> handleDataIntegrity(DataIntegrityViolationException ex) {
    return Map.of("error", "Conflict", "detail", "Violación de integridad de datos");
  }

  // Argumento inválido en lógica → 400
  @ExceptionHandler(IllegalArgumentException.class)
  @ResponseStatus(HttpStatus.BAD_REQUEST)
  public Map<String, Object> handleIllegalArg(IllegalArgumentException ex) {
    return Map.of("error", "Bad Request", "detail", ex.getMessage());
  }

  // Conflicto de recurso (duplicado, etc.) → 409
  @ExceptionHandler(ResourceConflictException.class)
  @ResponseStatus(HttpStatus.CONFLICT)
  public Map<String, Object> handleConflict(ResourceConflictException ex) {
    return Map.of("error", "Conflict", "detail", ex.getMessage());
  }

  // Recurso no encontrado → 404
  @ExceptionHandler(ResourceNotFoundException.class)
  @ResponseStatus(HttpStatus.NOT_FOUND)
  public Map<String, Object> handleNotFound(ResourceNotFoundException ex) {
    return Map.of("error", "Not Found", "detail", ex.getMessage());
  }
  
}
