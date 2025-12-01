package com.onion.piklopedia.infrastructure.cache;

import org.springframework.cache.CacheManager;
import org.springframework.cache.annotation.CachingConfigurerSupport;
import org.springframework.cache.concurrent.ConcurrentMapCacheManager;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;


@Configuration
public class RedisConfig extends CachingConfigurerSupport {
  @Bean
  public CacheManager cacheManager() {
    return new ConcurrentMapCacheManager("pikmin"); 
  }
}
