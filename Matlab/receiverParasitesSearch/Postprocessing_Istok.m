clc; 
close all; 
clearvars;

%переменные значения
load('measures_istok_3_high.mat')

tresholdMedianCoefficient = 0;
tresholdShift = -30;
ignoreBW = 10; % MHZ ширина сигнала генератора для исключения ее из расчетов
ignoreBWpoints = ignoreBW * (analyzer.PointsNum - 1)/(analyzer.FreqStop-analyzer.FreqStart);

ignoreLowFreq = 1300; % MHz исключение нулевой частоты из расчетов
ignoreLowFreqNum = ceil(ignoreLowFreq * (analyzer.PointsNum - 1)/(analyzer.FreqStop-analyzer.FreqStart));
ignoreHighFreq = 2200; % MHz исключение нулевой частоты из расчетов
ignoreHighFreqNum = ceil(ignoreHighFreq * (analyzer.PointsNum - 1)/(analyzer.FreqStop-analyzer.FreqStart));

counter=0; % смещение нового окна с графиками

for i=1:size(measData.Spectrums,1)
    channelFreq = 4*heterodin.Freqs(i)-22150;
    figure('Name',['Violations. ' 'Fhet=' num2str(heterodin.Freqs(i)) '; Fch=' num2str(channelFreq)], 'Position', [100 + counter, 500 - counter, 1000, 400]);
    counter = counter + 20;
    hold on;
    grid on;
    grid minor;
    n=0;
    numerationShift = 0;
    for j=1:size(measData.Spectrums,2)
        spectrumTMP = squeeze(measData.Spectrums(i, j, :));
        
        % находим номер частоты, на которой должен быть полезный выходной сигнал
        usefulSignalPointNum = 0;
        outputFreq = 4*(heterodin.Freqs(i)-5100) - generator.Freqs(j);
      
        % отбрасываем игнорируемую полосу
        spectrumTMP(ignoreHighFreqNum:size(spectrumTMP,1)) = [];
        spectrumTMP(1:ignoreLowFreqNum) = [];
        
        % если частота полезного сигнала попадает в рассматриваемую полосу
        if ((outputFreq>=ignoreLowFreq) && (outputFreq <=ignoreHighFreq))
            % ищем номер частоты полезного сигнала
            for k = 1:analyzer.PointsNum
                if (outputFreq>=analyzer.Freqs(k))
                    usefulSignalPointNum = k;
                end
            end
            % если точка найдена, определяем игнорируемую область спектра
            if (usefulSignalPointNum>0)
                startIgnorePoint = ceil(usefulSignalPointNum - ignoreBWpoints/2);
                stopIgnorePoint = ceil(usefulSignalPointNum + ignoreBWpoints/2);
                
                if startIgnorePoint<ignoreLowFreqNum
                    startIgnorePoint=ignoreLowFreqNum+1;
                end
                if stopIgnorePoint>ignoreHighFreqNum
                    stopIgnorePoint=ignoreHighFreqNum-1;
                end
                
                % из спектра уже вырезана нижняя и верхняя части, поэтому
                % нумерация сдвинута на ignoreLowFreqNum
                spectrumTMP( startIgnorePoint-ignoreLowFreqNum : stopIgnorePoint-ignoreLowFreqNum ) = [];
            end
        end
        


        sortedSpectrum = sort(spectrumTMP);
        median = sortedSpectrum(ceil(size(sortedSpectrum,1)/2));
        treshold = median*tresholdMedianCoefficient + tresholdShift;
        
        maxVal = sortedSpectrum(size(sortedSpectrum,1));
        plotNow = 1;
        
        while (maxVal >= treshold)
            n=n+1;
            maxIndex = find(squeeze(measData.Spectrums(i, j, :)) == maxVal);
            MAX(i, n, :) = [heterodin.Freqs(i)/1000 channelFreq/1000 generator.Freqs(j)/1000 analyzer.Freqs(maxIndex)/1000 maxVal];
            sortedSpectrum(size(sortedSpectrum,1)) = [];
            maxVal = sortedSpectrum(size(sortedSpectrum,1));

            if (plotNow>0)
                plotNow=0;
                plotLegend = ['Fgen=' num2str(generator.Freqs(j))];
                if (usefulSignalPointNum>0)
                    plotLegend = [plotLegend ', Signal at ' num2str(outputFreq)];
                end
                plot(measData.Freqs, squeeze(measData.Spectrums(i, j, :)), 'DisplayName', plotLegend);
                %plot([measData.Freqs(1), measData.Freqs(size(measData.Freqs, 2))], [treshold, treshold], 'DisplayName', ['treshold for Fgen=' num2str(generator.Freqs(j))])
                xlim([measData.Freqs(1) measData.Freqs(size(measData.Freqs, 2))]);
            end
        end
    end
    plot([measData.Freqs(1), measData.Freqs(size(measData.Freqs, 2))], [treshold, treshold], 'DisplayName', 'treshold')
    legend;
    if size(get(gca, 'children'),1) < 2
        close(gcf);
        counter = counter - 20;
    end
end

clc;
for i=1:size(MAX,1)
    
    viewData = squeeze(MAX(i,:,:));
    loopCount = size(viewData,1);
    for h=1:loopCount
        if (viewData(loopCount-h+1, 3) == 0)
            viewData(loopCount-h+1, :) = [];
        end
    end
    viewData
end;

