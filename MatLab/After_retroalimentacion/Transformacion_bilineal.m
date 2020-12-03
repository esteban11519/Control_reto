clear; clc; 
syms z Ts kp ki kd N;
td=kd/N;

% Según la tranformación bilineal
s=2*(1-z^{-1})/(Ts*(1+z^{-1}));
C= kp+ ki/s + kd*s/(td*s+1); 

Cz=simplify(C);
[N,D] = numden(Cz);
Nz=coeffs(N,z);
Dz=coeffs(D,z);

% Esto es de la forma H(z)= (b1+b2z+b3z^2)/(a1+a2z+a3z^2)
b1=Nz(1);b2=Nz(2);b3=Nz(3);
a1=Dz(1);a2=Dz(2);a3=Dz(3);

% Luego se pasa a la forma H(z)= (b0+b1z^{-1}+b2z^{-2})/(1+a1z^{-1}+a2z^{-2})
b0=b3/a3; b1n=b2/a3; b2n=b1/a3;
a1n=a2/a3; a2n=a1/a3;


% Matrices en variables de estado

A=[0 1;-a2n -a1n];
B=[0; 1];
C=[b2n-b0*a2n b1n-a1n*b0];
D=b0;


%% Area de prueba

% Eso modificar esta área delimitada
Ts=0.2; % Tiempo de muestreo
kp=-0.062236110091776;   % Constante proporcional
ki=0.120562962484935;   % Constante integrativa
kd=3.432664023631747;   % Constante derivativa
N=0.374034763054009;    % Alejamiento del cero al polo del derivador

% Eso modificar esta área delimitada

A=[0 1;double(subs(-a2n)) double(subs(-a1n))];
B=[0; 1];
C=[double(subs(b2n-b0*a2n)) double(subs(b1n-a1n*b0))];
D=double(subs(b0));



% Asiganción de constantes


input='e';
output='u';
valor_nomal='u_N';

sprintf('%s=%.15f*%s+%.15f*%s+%.15f*%s;','xn1',A(1,1),'x1',A(1,2),'x2',B(1,1),input)
sprintf('%s=%.15f*%s+%.15f*%s+%.15f*%s;','xn2',A(2,1),'x1',A(2,2),'x2',B(2,1),input)
sprintf('%s=%.15f*%s+%.15f*%s+%.15f*%s+%s;',output,C(1,1),'x1',C(1,2),'x2',D,input,valor_nomal)

% Valores propios de las matrices
eig_A=eig(A);
sprintf('eig de A es: %.15f  ,  %.15f',eig_A(1,1),eig_A(2,1))

% Función de transferencia

% Función de transferencia

[num,den]=ss2tf(A,B,C,D);
num
den
