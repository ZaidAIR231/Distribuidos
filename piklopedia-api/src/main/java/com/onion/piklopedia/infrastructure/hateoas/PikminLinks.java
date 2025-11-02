package com.onion.piklopedia.infrastructure.hateoas;

import com.onion.piklopedia.api.controller.PikminController;
import org.springframework.hateoas.Link;

import java.util.UUID;

import static org.springframework.hateoas.server.mvc.WebMvcLinkBuilder.linkTo;
import static org.springframework.hateoas.server.mvc.WebMvcLinkBuilder.methodOn;

public class PikminLinks {

    public static Link self(UUID id) {
        return linkTo(methodOn(PikminController.class).getById(id)).withSelfRel();
    }

    public static Link collection() {
        return linkTo(methodOn(PikminController.class)
                .list(0, 10, null, null)).withRel("collection");
    }
}
