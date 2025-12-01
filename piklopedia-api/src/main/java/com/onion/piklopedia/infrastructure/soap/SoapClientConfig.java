package com.onion.piklopedia.infrastructure.soap;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.oxm.jaxb.Jaxb2Marshaller;
import org.springframework.ws.client.core.WebServiceTemplate;
import org.springframework.ws.transport.http.HttpUrlConnectionMessageSender;

import java.time.Duration;

// cliente SOAP para comunicarse con el servicio Pikmin
@Configuration
public class SoapClientConfig {

  // URL base del servicio SOAP (por defecto, apunta al servicio en Docker)
  @Value("${pikmin.soap.base-url:http://pikminapi:8081/ws}")
  private String baseUrl;

  // Configura el marshaller JAXB (paquete generado de clases SOAP)
  @Bean
  public Jaxb2Marshaller marshaller() {
    Jaxb2Marshaller m = new Jaxb2Marshaller();
    m.setContextPath("com.onion.pikmin.ws");
    return m;
  }

  // Crea y configura el WebServiceTemplate con timeouts y URL base
  @Bean
  public WebServiceTemplate webServiceTemplate(Jaxb2Marshaller marshaller) {
    HttpUrlConnectionMessageSender sender = new HttpUrlConnectionMessageSender();
    sender.setConnectionTimeout(Duration.ofSeconds(5)); // tiempo máx conexión
    sender.setReadTimeout(Duration.ofSeconds(5));       // tiempo máx lectura

    WebServiceTemplate tpl = new WebServiceTemplate();
    tpl.setMarshaller(marshaller);
    tpl.setUnmarshaller(marshaller);
    tpl.setDefaultUri(baseUrl);
    tpl.setMessageSender(sender);
    return tpl;
  }
}
