model Cruise_control_lineal_2
  // Constantes
  parameter Real m=1600 "Neto Mass [kg]";
  parameter Real Tm=190 "Torque Máximo [Nm]";
  parameter Real wm=400 "Velocidad angualar máxima rad/s";
  parameter Real beta=0.4 "Coeficiente relativo al torque";
  //parameter Real alfa_1 = 40 "Effective wheel radius 40";
  //parameter Real alfa_2 = 25 "Effective wheel radius 25";
  //parameter Real alfa_3 = 16 "Effective wheel radius 16";
  //parameter Real alfa_4 = 12 "Effective wheel radius 12";
  parameter Real alfa_5 = 10 "Effective wheel radius 10";
  parameter Real Cr=0.01 "Coeficiente de fricción de rodamiento";
  parameter Real rho=1.3 "Densidad del aire kg/m^3";
  parameter Real Cd=0.32 "Coeficiente de resistencia aerodinámica dependiento de la curva";
  parameter Real A=2.4 "Área Frontal del carro [m^2]";
  parameter Real pi=3.141592654 "Valor de pi";
  parameter Real g=9.8 "gravedad 9.8 m/s^2"; 
   
  // Valores nominales
  /* vN a partir de uN
  parameter Real uN=0.1687 "Posición del acelerador nominal";
  parameter Real a=alfa_5*uN*Tm*beta*(alfa_5/wm)^2+ rho*Cd*A/2;
  parameter Real b=-2*(alfa_5/wm)*alfa_5*uN*Tm*beta;
  parameter Real c=m*g*sin(theta)+m*g*Cr+(beta-1)*alfa_5*uN*Tm;
  parameter Real vN_1=-b/(2*a)+sqrt(b^2-4*a*c)/(2*a);
  parameter Real vN_2=-b/(2*a)-sqrt(b^2-4*a*c)/(2*a);
  parameter Real vN=vN_1;
  
  */
  
  // uN a partir de vN
  
  parameter Real vN=20 "Velocidad nominal";
  parameter Real T_alpha_n_v= Tm*(1-beta*(alfa_5*vN/wm-1)^2) "Función de torque";
  parameter Real uN=(m*g*Cr+rho*Cd*A*(vN^2)/2)/(alfa_5*T_alpha_n_v);  
  
  
    // Linealización Jacobiana
  parameter Real LA=(uN*(alfa_5^2)*2*Tm*beta*(alfa_5*vN/wm -1)*alfa_5/wm-rho*Cd*vN)/m;
  parameter Real LB=alfa_5*Tm*(1-beta*(alfa_5*vN/wm -1)^2)/m;
  parameter Real LC=1;
  parameter Real LD=0;
  
  
  // Constantes del controlador
  parameter Real kp = 0.5 "Constante proporcional";
  parameter Real ki = 0.1 "Constante integral";
  parameter Real kd = 0 "Constante derivativa";
  
  // Definicion de estados
  Real v(start = vN) "Velocidad del carro";
  Real vD(start =0) "Arranca desde cero la desviación de la bola";
  
  // Definición de entradas
  Real theta(start = 0) "Grados de inclinación";
  Real u(start = uN) "Posición del acelerador";
  Real uD(start = 0) "Desviaciación de la posición del acelerador";
  Real uD_lin(start = 0) "Desviaciación de la posición del acelerador en el modelo lineal";
  
  // Definición de salidas
  Real y(start = vN) "velocidad del carro sistema no lineal";
  Real yD(start = 0) "velocidad de desviación del carro en el sistema lineal";
  Real y_lin "velocidad estimada con el modelo lineal (yest= yD+vN)";
  
  // Variables de control
  //Real referencia "referencia";
  //Real referenciaD "referenciaD";
  Real I(start = 0) "Accion integral";
  Real error(start = 0) "error en el sistema no lineal";
  Real errorD(start = 0) "error en el sistema lineal";
  Real ID_lin(start = 0) "integral para el sistema lineal";
  
  
  // parametros de la señal de referencia
  parameter Real amplitud = 4*pi/180;
  Boolean periodo;
  Real onda_cuadrada(start = -amplitud);

  equation
   // Onda cuadrada para la señal de referencia
  periodo = sample(10, 25);
  when periodo then
    onda_cuadrada = -pre(onda_cuadrada);
  end when;

  // Se hace la suposición que la velocidad es mayor a cero
  
  /* controlador y ecuaciones para el sistema no lineal */
  theta=(amplitud+onda_cuadrada)/2;
  error = vN - y;
  der(I) = ki * error;
  uD = kp * error + I "Señal de control de desviación";
  u = uD + uN;
  
  der(v) = (alfa_5*u*Tm*(1-beta*(alfa_5*v/wm - 1)^2)-m*g*Cr-      rho*Cd*A*(v^2)/2-m*g*sin(theta))/m;
  y = v;
  
  /* Ecuaciones del sistema lineal con controlador */
  
  
  errorD = 0-yD;
  der(ID_lin) = ki * errorD;
  uD_lin= kp * errorD + ID_lin;
  
  der(vD) = LA*vD+LB*uD_lin-g*theta;
  yD = vD;  
  y_lin = yD + vN;
  
    annotation(
    experiment(StartTime = 0, StopTime = 110, Tolerance = 1e-9, Interval = 0.0002));

 end Cruise_control_lineal_2;
  