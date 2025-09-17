package com.onion.pikmins.service;

import com.onion.pikmins.model.Pikmin;
import com.onion.pikmins.repository.PikminRepository;
import org.springframework.stereotype.Service;

import java.util.Optional;
import java.util.UUID;

@Service
public class PikminService {

    private final PikminRepository repository;

    public PikminService(PikminRepository repository) {
        this.repository = repository;
    }

    public Pikmin create(Pikmin pikmin) {
        if (pikmin.getId() == null) {
            pikmin.setId(UUID.randomUUID());
        }
        return repository.save(pikmin);
    }

    public Optional<Pikmin> getById(UUID id) {
        return repository.findById(id);
    }
}