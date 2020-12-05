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
using UnityEngine.SceneManagement;
using TMPro;



public class CarController : MonoBehaviour
{
    
    private Mainmenu _Mainmenu;
    private float aux_vr;
   
    
    // Objetos 
        
        // Carro de referencia
        GameObject carroReferencia;
        // El piso.
        GameObject ground;
        
         // Velocity
        TextMeshProUGUI Velocity;
        TextMeshProUGUI Tiempo;
        TextMeshProUGUI VelU;
        
        // Audio
        AudioSource throttle;
        AudioSource arranque;


        

    // El margen
    float PADDING=1f;
    
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

    float vN; // Velocidad en m/s

        // Rotación
        //public float tiempoComienzoPendiente=25f; // Tiempo en el que la pendiente comienza [s] (Se aconseja que sea mayor o igual al de establecimiento) 
        float theta=0; // Ángulo de la pendiente en grados.
        float auxTheta=0f;
        bool rotate=false;

    // Tiempos
    float t;
    float Ts=0.2f; // Tiempo de muestreo en segundos
    float Tolerancia_Ts; // Tolerancia de tiempo de muestreo
    
    // Tiempo primario de ejecución
    float last_time;
    float start_time;

    
    // Datos nominales
        // uN a partir de vN
        float T_alpha_n_v; // Función de torque
        float uN; // uN Posición noMinal del Motor a partir de vN      

    // Constantes del controlador

        double kp=-0.062236110091776;
        double ki=0.120562962484935;
        double kd=3.432664023631747;
        double N=0.374034763054009;
    
     // Variables de estado del controlador mediante transformación bilineal
    	// Matriz A
        double A11;
        double A12;
        double A21;
        double A22;
        // Matriz B
        double B11;
        double B21;
        // Matriz C
        double C11;
        double C12;
        // Matriz D
        double D;

    // Variables de estado

        double vn;
        double x1;
        double x2;
   
        double e;
        double x1n;
        double x2n;
        double u;
        double vn1;

    // Velocidad del carrito


        float carVelocity=0; // Velocidad del carro en [m/s]
        float vr;           // Velocidad de referencia en [m/s]
        float step=10f; // Variación del punto nominal [km/h]
        bool doStep=false;

  // Se cargan primero que todo, después se carga start();

  // Tratamiento de errores

        bool linealizado_bilineal=true;
        double P;
        double I;
        double Iant=0;
        double De;
        double eant=0;
        
        
    void Awake(){
        //Funciones para traer los valores de la interfaz
        _Mainmenu=FindObjectOfType<Mainmenu>();
         
    }

    // En esta parte se inicializan los datos.
    void Start()
    {   
        
        // Se toman los valores de las escenas Menu2
        aux_vr= _Mainmenu.getSpeed() ;
        M=_Mainmenu.getMass();
        theta=_Mainmenu.getSlope_var();
        step=_Mainmenu.getSpeed_var();
        
        
        kp=_Mainmenu.getKp();
        ki=_Mainmenu.getKi();
        kd=_Mainmenu.getKd();
        N=_Mainmenu.getN();
        //print("aux_vr: "+aux_vr+" M: "+ M+" theta: "+theta+" step: "+step+" kp: "+kp+" ki: "+ ki+" kd: "+kd+" N: "+N);
        // Fin de toma de datos


        vN=aux_vr/3.6f; // Se calcula el punto de operación 3 m/s por debajo de la velocidad de referencia
        // Velocidad de referencia
        vr=vN; // Una variación de 3 m/s en la velocidad


        // Se instancia el carro de referencia, el ground y los objetos de Canvas
        carroReferencia= GameObject.Find("Opacity");
        ground= GameObject.Find("ground");
        
        Velocity=GameObject.Find("Velocity").GetComponent<TextMeshProUGUI>();
        Tiempo=GameObject.Find("Tiempo").GetComponent<TextMeshProUGUI>();
        VelU=GameObject.Find("VelU").GetComponent<TextMeshProUGUI>();
        
        throttle=GameObject.Find("throttle").GetComponent<AudioSource>();
        arranque=GameObject.Find("arranque").GetComponent<AudioSource>();
        
        throttle.Play();
        arranque.Play();
        arranque.volume=0.5f;

        // Se fija la posición de referencia
        carroReferencia.transform.position=new Vector3((6f*vr*3.6f/25f -12f),0,carroReferencia.transform.position.z);
       
        // Inicialización de variables de tiempo de muestreo
        Tolerancia_Ts=Ts*0.05f; //Tolerancia de tiempo de muestreo del 5%

        // Datos nominales
        // uN a partir de vN
        // Función de torque
            T_alpha_n_v= TM*(1-BETA*Mathf.Pow(ALFHA_4*vN/WM-1,2)); 
        
            // uN Posición noMinal del Motor a partir de vN 
            uN=(M*V_g*CR+RHO*CD*A*(Mathf.Pow(vN,2))/2)/( ALFHA_4*T_alpha_n_v);  
        
        // Matrices de estado
        if(kd!=0)
        {
            // Matriz A
            A11=0;
            A12=1;
            A21=-(4*kd - 2*N*Ts)/(4*kd + 2*N*Ts);
            A22=(8*kd)/(4 *kd + 2*N*Ts);
            // Matriz B
            B11=0;
            B21=1;
            // Matriz C
            C11=(4*N*kd + 4*kd*kp - 2*N*Ts*kp - 2*Ts*kd*ki + N*Ts*Ts*ki)/(4*kd + 2*N*Ts) - 
            ((4*kd - 2*N*Ts)*(4*N*kd + 4*kd*kp + 2*N*Ts*kp + 2*Ts*kd*ki + N*Ts*Ts*ki))/((4*kd + 2*N*Ts)*(4*kd + 2*N*Ts));
            C12=(8*kd*(4*N*kd + 4*kd*kp + 2*N*Ts*kp + 2*Ts*kd*ki + N*Ts*Ts*ki))/((4*kd + 2*N*Ts)*(4*kd + 2*N*Ts)) -
             (- 2*N*ki*Ts*Ts + 8*N*kd + 8*kd*kp)/(4*kd + 2*N*Ts);
            // Matriz D
            D=(4*N*kd + 4*kd*kp + 2*N*Ts*kp + 2*Ts*kd*ki + N*Ts*Ts*ki)/(4*kd + 2*N*Ts);
        }else {
            linealizado_bilineal=false;
        }
            
        

        // Variables de estado del controlador

        vn=0; // El controlador se diseñó con un cambio al escalón de magnitud 3. 
        x1=0;
        x2=0;
        last_time=Time.time;
        start_time=Time.time;
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

                // en [m/s]
                e=vr-vn;
                
                if(linealizado_bilineal){
                // Implementación del controlador
                x1n=A11*x1+A12*x2+B11*e;
                x2n=A21*x1+A22*x2+B21*e;

                u=C11*x1+C12*x2+ D*e + uN;
                }
                else 
                {
                    P=kp*e;
                    I=Iant+ki*Ts*e;
                    De=kd*(e-eant)/Ts;
                    u=P+I+De;
                }
                // Debug.Log("u:"+u);

                // saturación
                if (u>1)
                {
                u=1;
                }
                else if(u<0)
                {
                u=0;
                }               

                // Cálculo de la velocidad sobre el modelo linealizado
                vn1= vn+ Ts*(ALFHA_4*u*TM*(1-BETA*((ALFHA_4*vn/WM-1)*(ALFHA_4*vn/WM-1)))-
                M*V_g*CR-RHO*CD*A*(vn*vn)/2-M*V_g*Mathf.Sin(auxTheta*Mathf.PI/180f))/M;

                carVelocity=(float)vn;

                // Asignaciones de texto
                
                Tiempo.text="t: "+(Time.time-start_time).ToString("F3")+" [s]";
                
                Velocity.text="u: "+u.ToString("F3")
                                +"\n"+"vr: "+(vr*3.6f).ToString("F3") +" [km/h]";
                
                VelU.text="v: "+ (carVelocity*3.6f).ToString("F3") +" [km/h]";
                
                // Control de volumen

                if(u==0) 
                {
                    arranque.volume=0.5f;
                }
                else {
                    throttle.volume=(float)u;
                    arranque.volume=0;
                }
             // Imprimir por consola
             //   Debug.Log("t= "+(Time.time-start_time)+"[s] "+" v: "+carVelocity +" [m/s] "+"u: "+ u); 
            // Termina acción controladora. vn está en [m/s] y la posición está en km/h

            this.transform.position=new Vector3(Mathf.Cos(auxTheta*Mathf.PI/180)*(108*carVelocity/125-12)
            ,Mathf.Sin(auxTheta*Mathf.PI/180)*(108*carVelocity/125-12),transform.position.z);
            // Se fijan los límites de movimiento
            float newX=Mathf.Clamp(transform.position.x,MINIMO_X+PADDING,MAXIMO_X-PADDING);
            float newY=Mathf.Clamp(transform.position.y,MINIMO_Y+PADDING,MAXIMO_Y-PADDING);
            // transform.position.z estrae la posición que tengo en transform
            this.transform.position=new Vector3(newX,newY,transform.position.z);
            
            if(doStep)
            {
                vr+=step/3.6f;
                carroReferencia.transform.position=new Vector3(Mathf.Cos(auxTheta*Mathf.PI/180f)*(6f*3.6f*vr/25f-12f)
                ,Mathf.Sin(auxTheta*Mathf.PI/180f)*(6*3.6f*vr/25f-12f),carroReferencia.transform.position.z);
                doStep=false;
            }

             // Se establece el ángulo de rotación
            
            if(rotate)
            {
                auxTheta+=theta;
                /* 
                Se hace la rotación del objeto ground. Dado que los sprites Opacity y Car están dentro de ground, cuando se gira ground 
                se giran ambos.
                */  
                ground.transform.Rotate(new Vector3(0,0,theta));
                // Se adapta la posición del carrito de referencia, según el ángulo y la velocidad donde vN está en km/h.
                carroReferencia.transform.position=new Vector3(Mathf.Cos(auxTheta*Mathf.PI/180f)*(6f*3.6f*vr/25f-12f)
                ,Mathf.Sin(auxTheta*Mathf.PI/180f)*(6*3.6f*vr/25f-12f),carroReferencia.transform.position.z);
                // Solo se hace la rotación una vez
                rotate=false;
            }

          

            //actualización de variables

            if(linealizado_bilineal){
                vn=vn1;
                x1=x1n;
                x2=x2n;
            }else{
                vn=vn1;
                Iant=I;
                eant=e;
            }
            
        
        
        }   
    }

    public void cambiar_pendiente(bool rotate)
    {
             this.rotate=rotate;
    }

    public void cambiar_velocidad(bool doStep)
    {
             this.doStep=doStep;
    }

    public void cambiar_escena(string nombre)
    {
        SceneManager.LoadScene (nombre);
        
    }

       public void salir()
    {
        Application.Quit();     
    }    
    
}
