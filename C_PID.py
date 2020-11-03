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

ts=0.01 # Tiempo de muestreo
# Parámetros del controlador
KP=1

# Parámetros de simulación

T_total=7 #tiempo de simulación [s]
 # tiempo de simulación

velocidad=[]
tiempo=[]

# ciclo de simulación
class simula():
    def __init__(self,vn,un,T_total,ts):
        self.vn = vn
        self.un=un
        self.T_total =T_total
        self.ts = ts
        self.x1=0
        self.x2=0
        self.velocidad=[]
    def sim(self):
        t=0
        while  t<=T_total:
            self.velocidad.append(self.vn)
            tiempo.append(t)
            e=vN-self.vn
            
            # Implementación del controlador
            x1n=self.x1+0.009951406120761*self.x2+0.000101049831201*e
            x2n=0*self.x1+0.990281224152192*self.x2+0.020209966240205*e
            u=2.034598800407653*self.x1+1.914777076705006*self.x2+ 5.521238825269089*e + uN
            self.x1=x1n
            self.x2=x2n
            
            # saturación
            if u>1:
                u=1
            elif u<0:
                u=0
            
            # Cálculo de la velocidad sobre el modelo linealizado
            vn1=(self.ts*lA+1)*self.vn+self.ts*lB*u-self.ts*B_g*theta
            self.vn=vn1
            t+= ts
            pl.plot(tiempo,self.velocidad)
            pl.grid()
            
        
PID= simula(20,uN,T_total,ts)
vell=PID.velocidad
PID.sim()
pl.show()
    

















