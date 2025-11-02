package com.onion.piklopedia.domain.error;

public class ResourceConflictException extends RuntimeException {
  public ResourceConflictException(String message) { super(message); }
}
