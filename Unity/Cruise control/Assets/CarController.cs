using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed=5f; 

    // Según el número de cuadros que haya establecido en X y Y, Según también esté mi ubicación del objeto.
    public float PADDING=1f;
    float MAXIMO_X=14; 
    float MINIMO_X=-14;
    float MAXIMO_Y=10; 
    float MINIMO_Y=-10;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento  horizontal con las teclas

         float hInput= Input.GetAxis("Horizontal");
        this.transform.position+=new Vector3(hInput*speed*Time.deltaTime,0,0);
    
        
        // Movimiento vertical con las teclas
        float vInput= Input.GetAxis("Vertical");
        this.transform.position+=new Vector3(0,vInput*speed*Time.deltaTime,0);
    
        // Establecimiento de límites laterales

        float newX=Mathf.Clamp(transform.position.x,MINIMO_X+PADDING,MAXIMO_X-PADDING);
        float newY=Mathf.Clamp(transform.position.y,MINIMO_Y+PADDING,MAXIMO_Y-PADDING);
        this.transform.position=new Vector3(newX,newY,transform.position.z);

    }

}
