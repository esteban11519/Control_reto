/*
Comentarios generales
-Se realizan una serie de cálculos para determinar la posición de los carros en función de la inclinación.
-Se tuvieron que fijar los pivotes del suelo (ground) y de los carritos para que la rotación se diera en cero.
-El número de pixeles por unidad deben ajustarse para que los cuadros elegidos se ajusten a nuestra imagen de fondo (background)
-Con el background de 700x500, el ground por 780x457 que contiene 13 unidades (0 a 100 km/h y el margen), además el sistema está 
coordenado como Dominio=[-14,14] y Rango de [-10,10] cuadros ajustándose perfectamente a las dimensiones del background. La conversión
de velocidad en km/h a cuadros es:

Y= 6X/25-12

Donde,

Y: Cuadros
X= Velocidad en km/h

Recordar, que la rotación se da en el pivote coordenado como [0,0].

Hay cosas que solo se pueden hacer con la ayuda de Dios y esta no es la excepción.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Objetos 
        // Carro de referencia
        GameObject carroReferencia;
        // El piso.
        GameObject ground;

    // El margen
    public float PADDING=1f;
    
    /* 
    Establecimiento de límites, posición está dado por cuadros en la interfaz de Unity, no por pixeles.
    */
    float MAXIMO_X=14; 
    float MINIMO_X=-14;
    float MAXIMO_Y=10; 
    float MINIMO_Y=-10;
    
    // Datos nominales

    public float vN=30f; // Velocidad en km/h
        // Rotación
        public float theta=0f; // Ángulo de rotación en grados.
        float auxTheta=0f;
        public bool rotate=false;

    // Tiempo de muestreo

    float Ts=1f; // Tiempo de muestreo en segundos
    float Tolerancia_Ts; // Tolerancia de tiempo de muestreo
    
        // Datos de prueba
        int i;

    // Tiempo primario de ejecución
    float last_time;
    
    // En esta parte se inicializan los datos.
    void Start()
    {
        // Se instancia el carro de referencia y el ground
        carroReferencia= GameObject.Find("Opacity");
        ground= GameObject.Find("ground");
        // Se fija la posición de referencia
        carroReferencia.transform.position=new Vector3((6f*vN/25f -12f),0,carroReferencia.transform.position.z);

        // Inicialización de variables de tiempo de muestreo
        Tolerancia_Ts=Ts*0.05f; //Tolerancia de tiempo de muestreo del 5%
        // Se registra el tiempo inicial
        last_time=Time.time;
        i=0;
        
    }

    // Actualiza los frames periódicamente
    void Update()
    {
        // Prueba de tiempo: Cada 0.02 segundos se actualiza
        //Debug.Log(Time.time-last_time);
        // Solo se ejecuta en el tiempo de muestreo
        if((Time.time-last_time)>=(Ts-Tolerancia_Ts))
        {   
            last_time=Time.time;
            // tiempo de ejecución
            Debug.Log(Time.time);
            i++;// Solo para fines de prueba y se utiliza en la función car_velocity()
            /*  
                car_velocity() está en [m/s]. Vector3() tiene tres argumentos: Vector3(posx,posy,posz).
            */
            this.transform.position=new Vector3(Mathf.Cos(auxTheta*Mathf.PI/180)*(108*car_velocity()/125-12)
            ,Mathf.Sin(auxTheta*Mathf.PI/180)*(108*car_velocity()/125-12),transform.position.z);
            // Se fijan los límites de movimiento
            float newX=Mathf.Clamp(transform.position.x,MINIMO_X+PADDING,MAXIMO_X-PADDING);
            float newY=Mathf.Clamp(transform.position.y,MINIMO_Y+PADDING,MAXIMO_Y-PADDING);
            // transform.position.z estrae la posición que tengo en transform
            this.transform.position=new Vector3(newX,newY,transform.position.z);
            

            // Se establece el ángulo de rotación
            if(rotate)
            {
                auxTheta=theta;
                /* 
                Se hace la rotación del objeto ground. Dado que los sprites Opacity y Car están dentro de ground, cuando se gira ground 
                se giran ambos.
                */  
                ground.transform.Rotate(new Vector3(0,0,auxTheta));
                // Se adapta la posición del carrito de referencia, según el ángulo y la velocidad donde vN está en km/h.
                carroReferencia.transform.position=new Vector3(Mathf.Cos(auxTheta*Mathf.PI/180)*(6*vN/25-12)
                ,Mathf.Sin(auxTheta*Mathf.PI/180)*(6*vN/25-12),carroReferencia.transform.position.z);
                // Solo se hace la rotación una vez
                rotate=false;
            }
        
        
        }   
    }

    // Función que determina la velocidad (Colocar en m/s)
    private float car_velocity()
    {
        if(i%11==0){
            i=0;
            }   

        return (float)i*10f/3.6f;
    }
    
}
