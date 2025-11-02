package com.onion.piklopedia.domain.error;

public class ResourceNotFoundException extends RuntimeException {
  public ResourceNotFoundException(String message) { super(message); }
}
