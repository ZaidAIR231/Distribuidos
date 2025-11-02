package com.onion.piklopedia.domain.repository;

import com.onion.piklopedia.domain.model.Pikmin;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

public interface PikminRepository extends JpaRepository<Pikmin, UUID> {

  boolean existsByCaptainNameIgnoreCase(String captainName);

  boolean existsByCaptainNameAndColorAndIdNot(String captainName, String color, UUID id);
}
