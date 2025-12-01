//
// Este archivo ha sido generado por Eclipse Implementation of JAXB v3.0.0 
// Visite https://eclipse-ee4j.github.io/jaxb-ri 
// Todas las modificaciones realizadas en este archivo se perderán si se vuelve a compilar el esquema de origen. 
// Generado el: 2025.10.30 a las 10:15:27 PM CST 
//


package com.onion.pikmin.ws;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlRootElement;
import jakarta.xml.bind.annotation.XmlType;


/**
 * <p>Clase Java para anonymous complex type.
 * 
 * <p>El siguiente fragmento de esquema especifica el contenido que se espera que haya en esta clase.
 * 
 * <pre>
 * &lt;complexType&gt;
 *   &lt;complexContent&gt;
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType"&gt;
 *       &lt;sequence&gt;
 *         &lt;element name="pikmin" type="{http://pikmin.onion.com/ws}Pikmin" minOccurs="0"/&gt;
 *       &lt;/sequence&gt;
 *     &lt;/restriction&gt;
 *   &lt;/complexContent&gt;
 * &lt;/complexType&gt;
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "", propOrder = {
    "pikmin"
})
@XmlRootElement(name = "getPikminByIdResponse")
public class GetPikminByIdResponse {

    protected Pikmin pikmin;

    /**
     * Obtiene el valor de la propiedad pikmin.
     * 
     * @return
     *     possible object is
     *     {@link Pikmin }
     *     
     */
    public Pikmin getPikmin() {
        return pikmin;
    }

    /**
     * Define el valor de la propiedad pikmin.
     * 
     * @param value
     *     allowed object is
     *     {@link Pikmin }
     *     
     */
    public void setPikmin(Pikmin value) {
        this.pikmin = value;
    }

}
