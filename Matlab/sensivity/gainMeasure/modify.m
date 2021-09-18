clear all;
clc;
load('bik_10003000_az.mat')
GainData.KP = measData.KU';
GainData.freqs = measData.freqs;
GainData.distToAnt = measData.distToAntenna;
GainData.antType = 'LINDGREN';
clear measData;
measData = GainData;
clear GainData;
save kp_bik_10003000.mat measData;
clc;