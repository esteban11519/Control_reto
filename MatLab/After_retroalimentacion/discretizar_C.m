clear
clc
load C_PID.mat
Ts=0.2;
Cve=ss(C);
Cd=c2d(Cve,Ts,'tustin');
format long
disp(Cd.A)
disp(Cd.B)
disp(Cd.C)
disp(Cd.D)

[num,den]=tfdata(C,'v');

% Cálculo de parámetros
% Profesor Bermeo
a=num(1);
b=num(2);
c=num(3);
pd=den(2);
ki=c/pd;
kp=(b-ki)/pd;
kd=(a-kp)/pd;
N=pd*kd;

sprintf('%s=%.15f*%s+%.15f*%s+%.15f*%s;','xn1',Cd.A(1,1),'x1',Cd.A(1,2),'x2',Cd.B(1,1),'error')
sprintf('%s=%.15f*%s+%.15f*%s+%.15f*%s;','xn2',Cd.A(2,1),'x1',Cd.A(2,2),'x2',Cd.B(2,1),'error')
sprintf('float %s=%.15f*%s+%.15f*%s+%.15f*%s+tempCmd_nominal;','tempCmd',Cd.C(1,1),'x1',Cd.C(1,2),'x2',Cd.D,'error')

% Valores propios de las matrices
eig_A=eig(Cd.A);
sprintf('eig de Cd.A es: %.15f  ,  %.15f',eig_A(1,1),eig_A(2,1))

% Función de transferencia

[num,den]=ss2tf(Cd.A,Cd.B,Cd.C,Cd.D);
num
den
