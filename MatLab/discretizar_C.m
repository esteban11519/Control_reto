Cve=ss(C);
Ts=0.01;
Cd=c2d(Cve,Ts,'tustin');
format long
disp(Cd.A)
disp(Cd.B)
disp(Cd.C)
disp(Cd.D)
