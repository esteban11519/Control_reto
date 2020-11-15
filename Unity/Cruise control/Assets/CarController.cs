using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Se crea un objeto que va a ser el carro de referencia
    GameObject carroReferencia;

    // Según el número de cuadros que haya establecido en X y Y, Según también esté mi ubicación del objeto.
    public float PADDING=1f;
    
    // Establecimiento de límites
    float MAXIMO_X=14; 
    float MINIMO_X=-14;
    float MAXIMO_Y=10; 
    float MINIMO_Y=-10;
    
    // Datos nominales

    public float vN=0;
    
    // Tiempo de muestreo

    float Ts=1f; // Tiempo de muestreo en segundos
    float Tolerancia_Ts; // Tolerancia de tiempo de muestreo
    
        // Datos de prueba
        int i;
        float sign; 
    
    // Tiempo primario de ejecución
    float last_time;
    
    // En esta parte se inicializan los datos.
    void Start()
    {
        // Se instancia el carro de referencia
        carroReferencia= GameObject.Find("Opacity");

        // Se fija la posición de referencia
        carroReferencia.transform.position=new Vector3(vN,0,0);

        // Inicialización de variables de tiempo de muestreo
        Tolerancia_Ts=Ts*0.05f; //Tolerancia de tiempo de muestreo del 5%
        // Se registra el tiempo inicial
        last_time=Time.time;
        i=0;
        sign=1;
        
    }

    // Actualiza los frames periódicamente
    void Update()
    {
        // Solo se ejecuta en el tiempo de muestreo
        if((Time.time-last_time)>=(Ts-Tolerancia_Ts))
        {   
            last_time=Time.time;
            Debug.Log(Time.time);
            i++;// Solo para fines de prueba y se utiliza en la función car_velocity()
            this.transform.position=new Vector3(sign*car_velocity(),0,0);
            // Se fijan los límites de movimiento
            float newX=Mathf.Clamp(transform.position.x,MINIMO_X+PADDING,MAXIMO_X-PADDING);
            float newY=Mathf.Clamp(transform.position.y,MINIMO_Y+PADDING,MAXIMO_Y-PADDING);
            this.transform.position=new Vector3(newX,newY,transform.position.z);
        
        }   
    }

    // Función que determina la velocidad
    private float car_velocity()
    {
        if(i%14==0){
            sign=-sign;
            i=0;
            }   

        return i;
    }
    
}
