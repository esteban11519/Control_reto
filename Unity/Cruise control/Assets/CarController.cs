using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    GameObject player;

    public float speed=5f; 
    // Según el número de cuadros que haya establecido en X y Y, Según también esté mi ubicación del objeto.
    public float PADDING=1f;
    float MAXIMO_X=14; 
    float MINIMO_X=-14;
    float MAXIMO_Y=10; 
    float MINIMO_Y=-10;
 
    
    // Tiempo de muestreo
    float sign=1;
    float Ts=1f; // Tiempo de muestreo en segundos
    int i;
    
    // Start is called before the first frame update
    void Start()
    {
        // Se instancia el carro de referencia
        player= GameObject.Find("Opacity");
        i=0;
        
    }

    // Update is called once per frame
    void Update()
    {
        float current_time=Time.time;
        // Movimiento  horizontal con las teclas
        // Se mueve el objeto player horizontalmente
         float hInput= Input.GetAxis("Horizontal");
        player.transform.position+=new Vector3(hInput*speed*Time.deltaTime,0,0);
        
        i++;
        if(i%10==0){
            sign=-sign;
        }
        // Movimiento vertical con las teclas
        this.transform.position+=new Vector3(0,sign*(i%10),0);
        
        // Establecimiento de límites laterales

        float newX=Mathf.Clamp(transform.position.x,MINIMO_X+PADDING,MAXIMO_X-PADDING);
        float newY=Mathf.Clamp(transform.position.y,MINIMO_Y+PADDING,MAXIMO_Y-PADDING);
        this.transform.position=new Vector3(newX,newY,transform.position.z);
        Debug.Log(Time.time);
        //Print the time of when the function is first called.

        /*float last_time=Time.time;
        // No funciona
        while ((last_time-current_time)<Ts)
        {       last_time=Time.time;
                Debug.Log(last_time-current_time);
        }
        */
    }

    
}
