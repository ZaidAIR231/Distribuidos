package com.onion.piklopedia.config;

import org.springframework.core.convert.converter.Converter;
import org.springframework.stereotype.Component;

import java.util.UUID;

@Component
public class UuidStringToUuidConverter implements Converter<String, UUID> {

  @Override
  public UUID convert(String source) {
    if (source == null) return null;
    String s = source.trim();
    // Si viene sin guiones (32 hex), lo formateamos
    if (s.matches("^[0-9a-fA-F]{32}$")) {
      s = s.replaceFirst(
        "^(\\p{XDigit}{8})(\\p{XDigit}{4})(\\p{XDigit}{4})(\\p{XDigit}{4})(\\p{XDigit}{12})$",
        "$1-$2-$3-$4-$5"
      );
    }
    return UUID.fromString(s); // aquí fallará si no es UUID válido
  }
}
