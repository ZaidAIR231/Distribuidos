//
// Este archivo ha sido generado por Eclipse Implementation of JAXB v3.0.0 
// Visite https://eclipse-ee4j.github.io/jaxb-ri 
// Todas las modificaciones realizadas en este archivo se perderán si se vuelve a compilar el esquema de origen. 
// Generado el: 2025.09.16 a las 07:53:30 PM CST 
//


package com.onion.pikmins.ws;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlType;


/**
 * <p>Clase Java para PikminInput complex type.
 * 
 * <p>El siguiente fragmento de esquema especifica el contenido que se espera que haya en esta clase.
 * 
 * <pre>
 * &lt;complexType name="PikminInput"&gt;
 *   &lt;complexContent&gt;
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType"&gt;
 *       &lt;sequence&gt;
 *         &lt;element name="captainName" type="{http://www.w3.org/2001/XMLSchema}string"/&gt;
 *         &lt;element name="color" type="{http://www.w3.org/2001/XMLSchema}string"/&gt;
 *         &lt;element name="onionCount" type="{http://www.w3.org/2001/XMLSchema}int"/&gt;
 *         &lt;element name="habitat" type="{http://www.w3.org/2001/XMLSchema}string"/&gt;
 *       &lt;/sequence&gt;
 *     &lt;/restriction&gt;
 *   &lt;/complexContent&gt;
 * &lt;/complexType&gt;
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "PikminInput", propOrder = {
    "captainName",
    "color",
    "onionCount",
    "habitat"
})
public class PikminInput {

    @XmlElement(required = true)
    protected String captainName;
    @XmlElement(required = true)
    protected String color;
    protected int onionCount;
    @XmlElement(required = true)
    protected String habitat;

    /**
     * Obtiene el valor de la propiedad captainName.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getCaptainName() {
        return captainName;
    }

    /**
     * Define el valor de la propiedad captainName.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setCaptainName(String value) {
        this.captainName = value;
    }

    /**
     * Obtiene el valor de la propiedad color.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getColor() {
        return color;
    }

    /**
     * Define el valor de la propiedad color.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setColor(String value) {
        this.color = value;
    }

    /**
     * Obtiene el valor de la propiedad onionCount.
     * 
     */
    public int getOnionCount() {
        return onionCount;
    }

    /**
     * Define el valor de la propiedad onionCount.
     * 
     */
    public void setOnionCount(int value) {
        this.onionCount = value;
    }

    /**
     * Obtiene el valor de la propiedad habitat.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getHabitat() {
        return habitat;
    }

    /**
     * Define el valor de la propiedad habitat.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setHabitat(String value) {
        this.habitat = value;
    }

}
