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
Ts=0.1 # Tiempo de muestreo
# Parámetros del controlador
KP=1

# Parámetros de simulación

T_total=10 #tiempo de simulación [s]
t=0; # tiempo de simulación

velocidad=[] # lista de velocidades
tiempo=[] # Lista de tiempo trascurrido

# ciclo de simulación

while t<=T_total:
    velocidad.append(vn)
    tiempo.append(t)
    e=vN-vn
    u=KP*e+uN
    
    #Saturaciones
    if u>1:
        u=1
    elif u<0:
        u=0
    
    #Cálculo de la velocidad sobre el modelo linealizado
    vn1=(Ts*lA+1)*vn+Ts*lB*u-Ts*B_g*theta
    vn=vn1
    t+=Ts
    pl.xlabel('tiempo')
    pl.ylabel('velocidad')
    pl.plot(tiempo,velocidad)
    pl.grid()
pl.show()
    

















