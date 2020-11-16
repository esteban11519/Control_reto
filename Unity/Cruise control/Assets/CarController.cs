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
    

    // Constantes

    float M=1600f;  //Neto Mass [kV_g]
    float TM=190f;  //Torque MáxiMo [NM]
    float WM=420f;  //Velocidad anV_gualar MáxiMa rad/s
    float BETA=0.4f; //Coeficiente relativo al torque
    float ALFHA_4 = 12f; //Effective wheel radius 10
    float CR=0.01f; //Coeficiente de fricción de rodaMiento
    float RHO=1.3f; //Densidad del aire kV_g/M^3
    float CD=0.32f;  // Coeficiente de resistencia aerodináMica dependiento de la curva
    float A=2.4f;  // Área Frontal del carro [M^2]
    float V_g=9.8f;  //V_gravedad 9.8 M/s^2 



    // Datos nominales

    float vN=20f; // Velocidad en m/s
    float theta_N=0f;// Ángulo Nominal

        // Rotación
        public float theta=0f; // Ángulo de rotación en grados.
        float auxTheta=0f;
        public bool rotate=false;

    // Tiempo de muestreo

    float Ts=0.2f; // Tiempo de muestreo en segundos
    float Tolerancia_Ts; // Tolerancia de tiempo de muestreo
    
    // Tiempo primario de ejecución
    float last_time;
    

    // Datos nominales
        // uN a partir de vN
        float T_alpha_n_v; // Función de torque
        float uN; // uN Posición noMinal del Motor a partir de vN  
        
        float B_g; // Debida a la gravedad y la inclinación

    //Linealización Jacobiana
        float T_PRIMA;
        float lA; // A en variables de estado
        float lB; // B en variables de estado
        float lC;    // C en variables de estado
        float lD;    // D en variables de estado


    // Variables de estado

        float vn;
        float carVelocity;
        float vr; 
        float x1;
        float x2;
   
        float e;
        float x1n;
        float x2n;
        float u;
        float vn1;



    // En esta parte se inicializan los datos.
    void Start()
    {   
        // Velocidad de referencia
        vr=vN+3; // Una variación de 3 m/s en la velocidad
        carVelocity=vN;


        // Se instancia el carro de referencia y el ground
        carroReferencia= GameObject.Find("Opacity");
        ground= GameObject.Find("ground");
        // Se fija la posición de referencia
        carroReferencia.transform.position=new Vector3((6f*vr*3.6f/25f -12f),0,carroReferencia.transform.position.z);

        // Inicialización de variables de tiempo de muestreo
        Tolerancia_Ts=Ts*0.05f; //Tolerancia de tiempo de muestreo del 5%
        // Se registra el tiempo inicial
        last_time=Time.time;

        // Datos nominales
        // uN a partir de vN
        T_alpha_n_v= TM*(1-BETA*Mathf.Pow(ALFHA_4*vN/WM-1,2)); // Función de torque
        uN=(M*V_g*CR+RHO*CD*A*(Mathf.Pow(vN,2))/2)/( ALFHA_4*T_alpha_n_v); // uN Posición noMinal del Motor a partir de vN  
        
        B_g=V_g*Mathf.Cos(theta_N*Mathf.PI/180); // Debida a la gravedad

        //Linealización Jacobiana
        T_PRIMA=-2*TM*BETA*( ALFHA_4*vN/WM -1)* ALFHA_4/WM;
        lA=(uN* ALFHA_4*T_PRIMA-RHO*CD*vN*A)/M; // A es variables de estado
        lB= ALFHA_4*TM*(1-BETA*Mathf.Pow(ALFHA_4*vN/WM-1,2))/M; // B es variables de estado
        lC=1f;    // C es variables de estado
        lD=0f;    // D es variables de estado

        // Variables de estado del controlador

        vn=0f; // El controlador se diseñó con un cambio al escalón de magnitud 3. 
        x1=0f;
        x2=0f;

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
            // Acción controladora

                e=vr-carVelocity;
                
                // Implementación del controlador
                x1n=x1 +0.197844221975159f*x2+0.005038243521050f*e;
                x2n=0.978442219751588f*x2+0.050382435210502f*e;

                u=0.051586824663966f*x1+0.315114588675131f*x2+ 0.319823269597134f*e;
                
                

                // saturación
                if (u>1-uN)
                {
                u=1-uN;
                }
                else if(u<-uN)
                {
                u=-uN;
                }              

                // Cálculo de la velocidad sobre el modelo linealizado
                vn1=(Ts*lA+1)*vn+Ts*lB*u-Ts*B_g*auxTheta;
                
                carVelocity=lC*vn+lD*u+vN;

                Debug.Log("La velocidad del carro es: "+carVelocity +", La posición de la válvula es: "+ (u+uN)); 
            // Termina acción controladora. vn está en [m/s] y la posición está en km/h

            this.transform.position=new Vector3(Mathf.Cos(auxTheta*Mathf.PI/180)*(108*carVelocity/125-12)
            ,Mathf.Sin(auxTheta*Mathf.PI/180)*(108*carVelocity/125-12),transform.position.z);
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
                carroReferencia.transform.position=new Vector3(Mathf.Cos(auxTheta*Mathf.PI/180)*(6*vr/25-12)
                ,Mathf.Sin(auxTheta*Mathf.PI/180)*(6*vr/25-12),carroReferencia.transform.position.z);
                // Solo se hace la rotación una vez
                rotate=false;
            }

            //actualización de variables


            vn=vn1;
            x1=x1n;
            x2=x2n;

        
        
        }   
    }
    
}
