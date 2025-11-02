package com.onion.piklopedia.infrastructure.soap.gateway;

import com.onion.pikmin.ws.GetPikminByIdRequest;
import com.onion.pikmin.ws.GetPikminByIdResponse;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Component;
import org.springframework.ws.client.core.WebServiceTemplate;

@Component
@RequiredArgsConstructor
public class PikminSoapGatewayImpl implements PikminSoapGateway {
    private final WebServiceTemplate webServiceTemplate;

    @Override
    public GetPikminByIdResponse getById(GetPikminByIdRequest request) {
        return (GetPikminByIdResponse) webServiceTemplate.marshalSendAndReceive(request);
    }
}
