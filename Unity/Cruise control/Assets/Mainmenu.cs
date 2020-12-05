using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mainmenu : MonoBehaviour
{
    public static Mainmenu mainmenu;

    public Text SpeedText; 
    public Text MassText; 
    public Text SlopeText;
    //private CarController car;

    
    GameObject Variables;
    private float  Speed2=10f;   
    private float  mass2=1600f; 
    private float  Slope2=0;
    private float  Slope_var=5f;
    private float  Speed_var=10f;
    private double  Kp=0;
    private double  Ki=0;
    private double  Kd=0;
    private double N=0;

    void Awake(){

       // DontDestroyOnLoad(gameObject);
        if(mainmenu==null)
        {
            mainmenu=this;
            DontDestroyOnLoad(gameObject);
        }else if(mainmenu!=this){
            Destroy(gameObject);
        }
        
    }

    public void PlayGame (string escena){
        SceneManager.LoadScene(escena);
    }

    public void Quit ()
    {
        Debug.Log("Quit!");
        Application.Quit();

    }
    
    public void setSpeed(float Speed)
    {
        
        SpeedText.text = "Speed: " + Speed.ToString("0.00") + "Km/h";
        //car.updateSpeed(Speed);   
        if(Speed!=float.NaN) Speed2=Speed;

    }

    public void setMass(float mass){
        MassText.text = "Mass: " + mass.ToString("0.00") + "Kg";
        if(mass!=float.NaN) mass2 = mass;
    }
    public void setSlope(float slope){
        SlopeText.text = "Slope: " + slope.ToString("0.00") ;
        if(slope!=float.NaN) Slope2 = slope;
        
    }
    public void Speed_variation(string speed ){
            if(speed!=null) this.Speed_var = float.Parse(speed);
          
    }
    public void Slope_variation(string slope ){
            if(slope!=null) this.Slope_var = float.Parse(slope);
           

    }
    public void setkp(string kp ){
        if(kp!=null)    Kp=double.Parse(kp);
        
    }
    public void setki(string ki ){
        if(ki!=null)    Ki=double.Parse(ki);
        
    }
    public void setkd(string kd ){
        if(kd!=null)    Kd=double.Parse(kd);
        
    }
    public void setN(string N ){
        if(N!=null)    this.N=double.Parse(N);
         

    }
    
    public float getSpeed(){
        return this.Speed2;
    }

    public float getMass(){
        return this.mass2;
    }
    public float getSlope(){
        return this.Slope2;
    }
    public float getSpeed_var(){
        return this.Speed_var;
    }
    public float getSlope_var(){
    return this.Slope_var;
    }
    public double getKp(){
    return this.Kp;
    }
    public double getKi(){
    return this.Ki;
    }
    public double getKd(){
    return this.Kd;
    }
    public double getN(){
    return this.N;
    }

}


