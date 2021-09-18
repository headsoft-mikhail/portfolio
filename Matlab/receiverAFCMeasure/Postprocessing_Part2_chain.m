clc; 
%close all; 
clearvars;

%входные данные
filename='measures_part2_20002_3.mat';
output_num = str2num(filename(22));
device_num = str2num(filename(20));
outputCount = 4;
devicesCount = 2;

%переменные значения
load(filename);
treshold = 0.87;
rpuBW = 270;
signalBW = 10; % MHZ ширина сигнала генератора для исключения ее из расчетов
colors = [[1,0,0];[0,1,0];[0,0,1];[0,0,0];[0.6500,0.2250,0.0980];[0.2660,0.5740,0.1880];[0,0.4470,0.5410];[0.35,0.35,0.35]];
signalBWpoints = signalBW * (analyzer.PointsNum - 1)/(analyzer.FreqStop-analyzer.FreqStart);


counter=0;

AFC = [];
if ((output_num==1) && (device_num ==1))
    close all;
    figure('Name','Part2: amplitude.', 'Position', [100 + counter, 500 - counter, 1000, 400]);
end
counter = counter + 20;
hold on;
grid on;
grid minor;

for i=2:2:size(measData.Spectrums,1)
    for j=1:size(measData.Spectrums,2)
        spectrumTMP = squeeze(measData.Spectrums(i, j, :));
        
        if  rpu.Freqs(i) < 9250    % нужно инвертировать
            genSignalCenterFreq = 1750 - generator.DeltaFreqs(j);
        else                       % не  нужно инвертировать
            genSignalCenterFreq = 1750 + generator.DeltaFreqs(j);
        end
        
        % поиск индекса частоты, на которой принят сигнал генератора
        minDeviation = 999999;
        for k=1:size(analyzer.Freqs,2)
            if (analyzer.Freqs(k) - genSignalCenterFreq) < minDeviation
                minDeviation = abs(analyzer.Freqs(k) - genSignalCenterFreq);
                genSignalCenterPoint = k;
            end
        end
        
        startSignalPoint = ceil(genSignalCenterPoint - signalBWpoints/2);
        stopSignalPoint = ceil(genSignalCenterPoint + signalBWpoints/2);
        if startSignalPoint<1
            startSignalPoint=1;
        end
        if stopSignalPoint>size(spectrumTMP,1)
            stopSignalPoint=size(spectrumTMP,1);
        end
        signalAmp(i,j) = max(spectrumTMP( startSignalPoint:stopSignalPoint ));
        signalFreq(i,j) = rpu.Freqs(i) + generator.DeltaFreqs(j);
        pointNum = find(spectrumTMP == signalAmp(i,j));
        
    end
    %plot(signalFreq(i,:), signalAmp(i,:), 'DisplayName', ['device ', num2str(device_num),' channel ', num2str(channel_num), ' Frpu=', num2str(rpu.Freqs(i))], 'Color', colors(device_num, channel_num));
    plot(signalFreq(i,:), signalAmp(i,:), 'DisplayName', ['device ', num2str(device_num),' output ', num2str(output_num)], 'Color', colors((device_num-1)*outputCount + output_num, :));
    
    legend;
    
end
