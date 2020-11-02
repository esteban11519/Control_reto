# -*- codinV_g utf-8 -*-
"""
CReated on Sun Nov  1 210958 2020

@author Esteban
"""

import math as m
import pylab as pl

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
B_g=V_g*m.cos(theta_N)
  
  #Linealización Jacobiana
T_PRIMA=-2*TM*BETA*( ALFHA_4*vN/WM -1)* ALFHA_4/WM
lA=(uN* ALFHA_4*T_PRIMA-RHO*CD*vN*A)/M
lB= ALFHA_4*TM*(1-BETA*( ALFHA_4*vN/WM -1)**2)/M
lC=1
lD=0

# Variables adicionales

theta=5*m.pi/180
vn=vN

# Parámetros del controlador

x1=0
x2=0
Ts=0.01 # Tiempo de muestreo

# Parámetros de simulación

T_total=7 #tiempo de simulación [s]
t=0; # Variable de simulación

velocidad=[]
throttle_position=[]
tiempo=[]


# ciclo de simulación

while t<=T_total:
    velocidad.append(vn)
    tiempo.append(t)
    
    e=vN-vn
    
    # Implementación del controlador
    x1n=x1+0.009951406120761*x2+0.000101049831201*e
    x2n=0*x1+0.990281224152192*x2+0.020209966240205*e
    u=2.034598800407653*x1+1.914777076705006*x2+ 5.521238825269089*e + uN
    x1=x1n
    x2=x2n
    
    
    # saturación
    if u>1:
        u=1
    elif u<0:
        u=0
    
    # se van guardando las posiciones de la válvula de flujo de gasolina
    throttle_position.append(u)
    
    # Cálculo de la velocidad sobre el modelo linealizado
    vn1=(Ts*lA+1)*vn+Ts*lB*u-Ts*B_g*theta
    vn=vn1
    t+=Ts
    
    if T_total<=t:
        pl.subplot(211)
        pl.plot(tiempo,velocidad)
        pl.xlabel('Tiempo [s]')
        pl.ylabel('Velocidad [m/s]')
        pl.grid()
        
        pl.subplot(212)
        pl.plot(tiempo,throttle_position)
        pl.xlabel('Tiempo [s]')
        pl.ylabel(' control throttle position')
        pl.grid()
    
    
pl.show()
    

















