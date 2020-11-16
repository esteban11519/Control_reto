clear
clc
load C_PID.mat % Se carga el controlador

% Tiempo continuo
m=1600 ; %Neto Mass [kg]
Tm=190 ; %Torque Máximo [Nm]
wm=420 ; %Velocidad angualar máxima rad/s
beta=0.4 ; %Coeficiente relativo al torque
alfa_5 = 12 ;  %Effective wheel radius 10
Cr=0.01 ; %Coeficiente de fricción de rodamiento
rho=1.3 ;%Densidad del aire kg/m^3
Cd=0.32 ; % Coeficiente de resistencia aerodinámica dependiento de la curva
A=2.4 ; % Área Frontal del carro [m^2]
g=9.8 ; %gravedad 9.8 m/s^2 
   
  % Valores nominales
  % uN a partir de vN
  
  vN=16.7 ; %Velocidad nominal [m/s]
  T_alpha_n_v= Tm*(1-beta*(alfa_5*vN/wm-1)^2); % Función de torque
  uN=(m*g*Cr+rho*Cd*A*(vN^2)/2)/(alfa_5*T_alpha_n_v); % uN Posición nominal del motor a partir de vN  
  theta_N=0;
  
  %Linealización Jacobiana
  T_prima=-2*Tm*beta*(alfa_5*vN/wm -1)*alfa_5/wm;
  LA=(uN*alfa_5*T_prima-rho*Cd*vN*A)/m;
  LB=alfa_5*Tm*(1-beta*(alfa_5*vN/wm -1)^2)/m;
  LC=1;
  LD=0;
  
  G=tf(LB,[1 -LA]);
  
  
  %% Discretización
  Ts=0.2;
  
  G_D=c2d(G,Ts);
  [z_GD,p_GD,k_GD]=zpkdata(G_D);  
  

  
  %Controlador PI
  %k_p = 1 ; %Constante proporcional
  %k_i = 0 ; %Constante integral
  %kd = 0 ; %Constante derivativa
  
  %C=tf([k_p k_i],[1 0]);
  [z_CC,p_CC,k_CC]=zpkdata(C);
  
  C_D=c2d(C,Ts,'tustin');
  [z_CD,p_CD,k_CD]=zpkdata(C_D);
  
  
  %% Simulaciones del controlador
  
  
  slope_grades=5;
  t_c=-g*cos(theta_N)/(LB) *(slope_grades*pi/180);
  
  F=3;
  
  subplot(2,1,1)
  T_yr=feedback(G*C,1);
  step(3*T_yr+vN);
  xlabel('Tiempo[s]'), ylabel('Velocidad[m/s]');
  title("Entrada: "+F+" r");
  
  subplot(2,1,2)
  T_ur=feedback(C,G);
  step(F*T_ur+uN);
  xlabel('Tiempo[s]'), ylabel('Válvula reguradora \mu');
  title("Entrada: "+F+" r");
