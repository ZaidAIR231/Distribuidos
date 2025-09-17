package com.onion.pikmins.mapper;

import com.onion.pikmins.model.Pikmin;
import com.onion.pikmins.ws.PikminInput;

public class PikminMapper {

    public static Pikmin toEntity(PikminInput input) {
        Pikmin e = new Pikmin();
        e.setCaptainName(input.getCaptainName());
        e.setColor(input.getColor());
        e.setOnionCount(input.getOnionCount());
        e.setHabitat(input.getHabitat());
        return e;
    }

    public static com.onion.pikmins.ws.Pikmin toWs(Pikmin entity) {
        com.onion.pikmins.ws.Pikmin ws = new com.onion.pikmins.ws.Pikmin();
        if (entity.getId() != null) ws.setId(entity.getId().toString());
        ws.setCaptainName(entity.getCaptainName());
        ws.setColor(entity.getColor());
        ws.setOnionCount(entity.getOnionCount());
        ws.setHabitat(entity.getHabitat());
        return ws;
    }
}