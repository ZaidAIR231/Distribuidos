//
// Este archivo ha sido generado por Eclipse Implementation of JAXB v3.0.0 
// Visite https://eclipse-ee4j.github.io/jaxb-ri 
// Todas las modificaciones realizadas en este archivo se perderán si se vuelve a compilar el esquema de origen. 
// Generado el: 2025.09.16 a las 07:53:30 PM CST 
//


package com.onion.pikmins.ws;

import jakarta.xml.bind.annotation.XmlRegistry;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the com.onion.pikmins.ws package. 
 * <p>An ObjectFactory allows you to programatically 
 * construct new instances of the Java representation 
 * for XML content. The Java representation of XML 
 * content can consist of schema derived interfaces 
 * and classes representing the binding of schema 
 * type definitions, element declarations and model 
 * groups.  Factory methods for each of these are 
 * provided in this class.
 * 
 */
@XmlRegistry
public class ObjectFactory {


    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: com.onion.pikmins.ws
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link CreatePikminRequest }
     * 
     */
    public CreatePikminRequest createCreatePikminRequest() {
        return new CreatePikminRequest();
    }

    /**
     * Create an instance of {@link PikminInput }
     * 
     */
    public PikminInput createPikminInput() {
        return new PikminInput();
    }

    /**
     * Create an instance of {@link CreatePikminResponse }
     * 
     */
    public CreatePikminResponse createCreatePikminResponse() {
        return new CreatePikminResponse();
    }

    /**
     * Create an instance of {@link Pikmin }
     * 
     */
    public Pikmin createPikmin() {
        return new Pikmin();
    }

    /**
     * Create an instance of {@link GetPikminByIdRequest }
     * 
     */
    public GetPikminByIdRequest createGetPikminByIdRequest() {
        return new GetPikminByIdRequest();
    }

    /**
     * Create an instance of {@link GetPikminByIdResponse }
     * 
     */
    public GetPikminByIdResponse createGetPikminByIdResponse() {
        return new GetPikminByIdResponse();
    }

}
