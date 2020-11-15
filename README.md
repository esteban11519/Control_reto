# Control_reto

## Incluyendo el carro
Para incluir sprites, son como imágenes que se le pueden atribuir características de objetos del juego.
https://www.youtube.com/watch?v=xfp4kpM3Asw

## Colocando el muestreo

### Intento 1

```C#
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

        float last_time=Time.time;
        
            while ((last_time-current_time)<Ts)
            {       last_time=Time.time;
                Debug.Log(last_time-current_time);
            }
        
    }

Se me para por completo el programa.

Luego, Investigo que cada frame debe actualizar todas las variables antes de ser cambiado.

```

### Intento 2

Con corrutinas.

https://docs.unity3d.com/es/2018.4/Manual/Coroutines.html#:~:text=Cuando%20usted%20llama%20a%20una,su%20totalidad%20antes%20de%20retornar.&text=Sin%20embargo%2C%20es%20usualmente%20mas,para%20este%20tipo%20de%20tarea.

Son rutinas que me permiten pasar de frame sin acabarse completamente de realizar. Me llama la atención que puedo hacer que ciertas actividades se ejecuten cada tiempo por:

```C#
void Update()
{
        StartCoroutine("Fade");
    
}

IEnumerator Fade() 
{
    for (float f = 1f; f >= 0; f -= 0.1f) 
    {
        Color c = renderer.material.color;
        c.a = f;
        renderer.material.color = c;
        yield return new WaitForSeconds(.1f);
    }
}
```
Intentándolo...
```C#
         last_time=Time.time;
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
            StartCoroutine("car_velocity");



    IEnumerator car_velocity() 
{
        last_time=Time.time;
        while ((last_time-current_time)<Ts)
            {       
                last_time=Time.time;
            }    

        
         
         yield return null;

}
```

### Intento 3

```C#


 void Update()
    {
        // Solo se ejecuta en el tiempo de muestreo
        if(Time.time-last_time>=Ts)
        {   
            last_time=Time.time;
            Debug.Log(Time.time);
            i++;// Solo para fines de prueba y se utiliza en la función car_velocity()
            this.transform.position=new Vector3(sign*car_velocity(),0,0);

        }

            // Se fijan los límites de movimiento
            float newX=Mathf.Clamp(transform.position.x,MINIMO_X+PADDING,MAXIMO_X-PADDING);
            float newY=Mathf.Clamp(transform.position.y,MINIMO_Y+PADDING,MAXIMO_Y-PADDING);
            this.transform.position=new Vector3(newX,newY,transform.position.z);
        
    }

    // Función que determina la velocidad
    private float car_velocity()
    {
        if(i%10==0){
            sign=-sign;
            i=0;
            }   

        return i;
    }

```


Referencias

Imágen tomada de: https://es.wikipedia.org/wiki/Regla_graduada#/media/Archivo:Regla_H.svg

