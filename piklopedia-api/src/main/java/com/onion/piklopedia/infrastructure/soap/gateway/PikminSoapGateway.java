package com.onion.piklopedia.infrastructure.soap.gateway;

import com.onion.pikmin.ws.GetPikminByIdRequest;
import com.onion.pikmin.ws.GetPikminByIdResponse;

public interface PikminSoapGateway {
    GetPikminByIdResponse getById(GetPikminByIdRequest request);
}
