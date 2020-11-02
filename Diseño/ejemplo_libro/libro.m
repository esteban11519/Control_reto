% Tomado de la página 9-27

a=0.01;
b=1.32;
kp=0.5;
ki=0.1;

P=tf(b,[1 a]);
C=tf([kp ki],[1 0]);

%% Función de transferencia del error con respecto a la variación de la pendiente (slope)

T_es=feedback(P,C);

step(0.5*T_es);

%% Función de transferecnia del control con respecto a la variación de la pendiente 
T_us=feedback(P*C,1);
step(0.5*T_us);

