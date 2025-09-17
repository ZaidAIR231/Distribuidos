package com.onion.pikmins.validator;

import com.onion.pikmins.ws.PikminInput;

public class PikminValidator {
    public static void validate(PikminInput input) {
        if (input == null) throw new PikminValidationException("pikmin input requerido");
        if (isBlank(input.getCaptainName())) throw new PikminValidationException("captainName requerido");
        if (isBlank(input.getColor())) throw new PikminValidationException("color requerido");
        if (isBlank(input.getHabitat())) throw new PikminValidationException("habitat requerido");
        // onionCount es 'int' (no puede ser null)
        if (input.getOnionCount() < 0)
            throw new PikminValidationException("onionCount debe ser >= 0");
    }

    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }
}