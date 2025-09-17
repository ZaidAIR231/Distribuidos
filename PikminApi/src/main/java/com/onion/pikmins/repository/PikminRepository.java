package com.onion.pikmins.repository;

import com.onion.pikmins.model.Pikmin;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

public interface PikminRepository extends JpaRepository<Pikmin, UUID> {
}