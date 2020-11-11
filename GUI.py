    # -*- coding: utf-8 -*-
"""
Created on Sun Oct 25 22:36:58 2020

@author: DACR9
"""
import math
import tkinter as tk
from PIL import ImageTk,Image

import time
M=1600  #Neto Mass [kV_g]
TM=190  #Torque MáxiMo [NM]
WM=420  #Velocidad anV_gualar MáxiMa rad/s
BETA=0.4 #Coeficiente relativo al torque
ALFHA_4 = 12 #Effective wheel radius 10
CR=0.01 #Coeficiente de fricción de rodaMiento
RHO=1.3 #Densidad del aire kV_g/M^3
CD=0.32  # Coeficiente de resistencia aerodináMica dependiento de la curva
A=2.4  # Área Frontal del carro [M^2]
V_g=9.8  #V_gravedad 9.8 M/s^2 
   
  # Valores noMinales
  # uN a partir de vN
vN=20  #Velocidad noMinal
T_alpha_n_v= TM*(1-BETA*(ALFHA_4*vN/WM-1)**2) # Función de torque
uN=(M*V_g*CR+RHO*CD*A*(vN**2)/2)/( ALFHA_4*T_alpha_n_v) # uN Posición noMinal del Motor a partir de vN  
theta_N=0
B_g=V_g*math.cos(theta_N)
  
  #Linealización Jacobiana
T_PRIMA=-2*TM*BETA*( ALFHA_4*vN/WM -1)* ALFHA_4/WM
lA=(uN* ALFHA_4*T_PRIMA-RHO*CD*vN*A)/M
lB= ALFHA_4*TM*(1-BETA*( ALFHA_4*vN/WM -1)**2)/M
lC=1
lD=0
KP=1
ts=0.01
T_total=7
def rampa():
    thet = theta.get()
    x1= 100
    x2= 100+400*math.cos(float(thet)*math.pi/180)
    y1= 400 
    y2= 400-400*math.sin(float(thet)*math.pi/180)
    # confirm=math.atan2(abs(y2-y1), abs(x1-x2))*180/math.pi
#Create canvas line (x1,y1,x2,y2,fill=color)
    canvas.create_line(x1,y1,x2,y2,fill='Red')
    canvas.create_line(100,400,x2,400,fill='blue')
    canvas.create_line(x2,400,x2,y2,fill='Red')
    #label1=tk.Label(root, text="Pendiente:")
    #tk.Label(root, text="Velocidad")
    #canvas.create_window(200, 180, window=label1)
def carimage ():
    vnom=vel.get()
    thet= theta.get()
    img = Image.open(r"C:\Users\DACR9\Documents\9 semestre\Control\Local_reto\car.png")
    img = img.resize((50, 50), Image.ANTIALIAS)
    img = img.rotate(float(thet))
    global image2
    image2  = ImageTk.PhotoImage(img)
    car = canvas.create_image(100,390, image = image2) 
    PID= C_PID.simula(float(vnom),uN,T_total,ts,float(thet))
    PID.sim()
    velocidad = PID.velocidad
    
    for i in  velocidad:
          x=100+(float(i)*4)*math.cos(float(thet)*math.pi/180)
          y=390-(float(i)*4)*math.sin(float(thet)*math.pi/180)
          canvas.move(car,x,y)
          root.update()
          time.sleep(ts)
          
        
    

def V_nominal():
    vnom=vel.get()
    thet= theta.get()
    img = Image.open(r"C:\Users\DACR9\Documents\9 semestre\Control\Local_reto\opacity.png")
    img = img.resize((50, 50), Image.ANTIALIAS)
    img = img.rotate(float(thet))
    global image
    image  = ImageTk.PhotoImage(img)
    x=100+(float(vnom)*4)*math.cos(float(thet)*math.pi/180)
    y=390-(float(vnom)*4)*math.sin(float(thet)*math.pi/180)
    canvas.create_image(x,y, image = image)
    
    
    
    
root= tk.Tk()
canvas = tk.Canvas(root, width = 500 , height = 500)
canvas.pack()
root.title('Cruise control')
root.geometry("500x500")
#leer ángulo
theta = tk.Entry(root)
vel = tk.Entry(root)
canvas.create_window(0, 50, window= theta)
canvas.create_window(0, 75, window= vel)
#imprimir triangulo
boton = tk.Button(text='Pendiente', command=rampa)
canvas.create_window(100, 50, window= boton)
#imprimir carrito opaco
boton2 = tk.Button(text='Velocidad', command=V_nominal)
canvas.create_window(100, 75,window= boton2 )

Go = tk.Button(text='GO', command=carimage)
canvas.create_window(450, 75,window= Go )      
           
root.mainloop()


# while True:
#     try:
         
#         root.update()
#     except:
#         break


