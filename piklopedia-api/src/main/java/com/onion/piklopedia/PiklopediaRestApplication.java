package com.onion.piklopedia;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cache.annotation.EnableCaching;

@SpringBootApplication
@EnableCaching
public class PiklopediaRestApplication {
  public static void main(String[] args) {
    SpringApplication.run(PiklopediaRestApplication.class, args);
  }
}
