close all;
clear all; 
clc;
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%% VARIABLE SETTINGS
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% MK6 fileName
fileNameMK6 = '924_2020_10_01_14_13_18'; 
% RLI fileName 
fileNameRLI = '2020.10.01_14-24-47_Log'; 
% other
RLI_Latitude = 60.414003; % RLI latitude
RLI_Longitude = 30.469665; % RLI longitude
RLI_Elevation = 15; % RLI longitude
useFirstPointCoordinates = false; % "true" if want to use MK6 first point as RLI position
useCalculatedElevation = true; % "true" if want to use MK6_Altitude instead of RLI_Elevation
R = 6372795; % Earth radius
L = 1120; 
Lmin=0.9*L;
Lmax=1.1*L;
outputFileName = 'output_data';

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%% LOAD FILES
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% load MK6 data
% DOT separator
fileNameMK6 = [fileNameMK6 '.log'];
delimiter = ',';
formatSpec = '%*s%f%f%*s%f%*s%*s%*s%*s%f%*s%*s%*s%*s%*s%*s%*s%*s%*s%*s%*s%*s%f%f%*s%*s%*s%*s%[^\n\r]';
fileID = fopen(fileNameMK6,'r');
dataArray = textscan(fileID, formatSpec, 'Delimiter', delimiter, 'EmptyValue' ,NaN, 'ReturnOnError', false);
fclose(fileID);
MK6_data.Time = dataArray{:, 1};
MK6_data.Latitude = dataArray{:, 2};
MK6_data.Longitude = dataArray{:, 3};
MK6_data.Altitude = dataArray{:, 4};
MK6_data.Speed = dataArray{:, 5};
MK6_data.Course = dataArray{:, 6};
clearvars fileNameMK6 delimiter formatSpec fileID dataArray ans;
% load RLI data
fileNameRLI = [fileNameRLI '.txt'];
delimiter = ' ';
formatSpec = '%f%f%f%f%f%f%f%[^\n\r]';
fileID = fopen(fileNameRLI,'r');
dataArray = textscan(fileID, formatSpec, 'Delimiter', delimiter, 'MultipleDelimsAsOne', true, 'EmptyValue' ,NaN, 'ReturnOnError', false);
fclose(fileID);
RLI_data.Time = dataArray{:, 1};
RLI_data.Distance_m = dataArray{:, 2};
RLI_data.Dopler_mps = dataArray{:, 3};
RLI_data.Amplitude_dB = dataArray{:, 4};
RLI_data.Noise_dB = dataArray{:, 5};
RLI_data.SNR = dataArray{:, 6};
RLI_data.maxDistance = dataArray{:, 7};
clearvars fileNameRLI delimiter formatSpec fileID dataArray ans;

Time_forTXT = MK6_data.Time;
clc;

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%% FORMAT TIME & COORDINATES
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
RLI_data.Hours = fix(RLI_data.Time/10000);
RLI_data.Minutes = fix(RLI_data.Time/100) - fix(RLI_data.Time/10000)*100;
RLI_data.Seconds = RLI_data.Time - (fix(RLI_data.Time/100) - fix(RLI_data.Time/10000)*100)*100 - fix(RLI_data.Time/10000)*10000;
RLI_data.Time = RLI_data.Hours*60*60 + RLI_data.Minutes*60 + RLI_data.Seconds;

MK6_data.Hours = fix(MK6_data.Time/10000);
MK6_data.Minutes = fix(MK6_data.Time/100) - fix(MK6_data.Time/10000)*100;
MK6_data.Seconds = MK6_data.Time - (fix(MK6_data.Time/100) - fix(MK6_data.Time/10000)*100)*100 - fix(MK6_data.Time/10000)*10000;
MK6_data.Time = MK6_data.Hours*60*60 + MK6_data.Minutes*60 + MK6_data.Seconds;
MK6_data.Latitude = fix(MK6_data.Latitude/100) + (MK6_data.Latitude - fix(MK6_data.Latitude/100)*100)/60;
MK6_data.Longitude = fix(MK6_data.Longitude/100) + (MK6_data.Longitude - fix(MK6_data.Longitude/100)*100)/60;
MK6_data.Altitude = MK6_data.Altitude - MK6_data.Altitude(1);
MK6_data.Speed = MK6_data.Speed * 0.514;

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%% SELECT TIME-MATCHING POINTS INDEXES
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
index=0;
selectedIndexes = zeros(min(size(MK6_data.Time,1), size(RLI_data.Time,1)),2);
for i=1:size(MK6_data.Time,1)
    deltaTime = 0.2;
    saveI = 0;
    saveJ = 0;
    for j=1:size(RLI_data.Time,1)
        if deltaTime > abs(MK6_data.Time(i)- RLI_data.Time(j))
            deltaTime = abs(MK6_data.Time(i)- RLI_data.Time(j));
            saveI = i;
            saveJ = j;
        end
    end
    if deltaTime < 0.2
        index=index+1;
        selectedIndexes(index,1) = saveI;
        selectedIndexes(index,2) = saveJ;
    end
end

% resize arrays after time selection
selectedIndexes = selectedIndexes(1:index,:);
MK6_Time = zeros(index,1);
MK6_Latitude = zeros(index,1);
MK6_Longitude = zeros(index,1);
MK6_Altitude = zeros(index,1);
MK6_Speed = zeros(index,1);
MK6_Distance_m = zeros(index,1);
MK6_Elevation = zeros(index,1);

RLI_Time = zeros(index,1);
RLI_Distance_m = zeros(index,1);
RLI_Dopler_mps = zeros(index,1);

Time = zeros(index,1);

if (useFirstPointCoordinates)
    RLI_Latitude = MK6_data.Latitude(1);
    RLI_Longitude = MK6_data.Longitude(1);
end

clear j;
clear deltaTime;
clear timeCutter;
clear saveI;
clear saveJ;

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%% SELECT BY DISTANCE FROM TIME-MATCHING POINTS
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
timeCutter = min(MK6_data.Time(selectedIndexes(1,1)), RLI_data.Time(selectedIndexes(1,2)));
index = 0;

filteredIndexes = selectedIndexes;
for i=1:size(selectedIndexes,1)
    current_distance = ...   %%% calculate distance
        R * 2*asin( sqrt( ...
        power(sin((pi/180)*(MK6_data.Latitude(selectedIndexes(i,1))-RLI_Latitude)/2), 2)...
        + cos(RLI_Latitude*pi/180) ...
        * cos(MK6_data.Latitude(selectedIndexes(i,1))*pi/180)...
        * power(sin((pi/180)*(MK6_data.Longitude(selectedIndexes(i,1)) - RLI_Longitude)/2), 2)...
        ));
    if ((current_distance>=Lmin) && (current_distance<=Lmax)) % select by distance
        index = index+1;
        Time(index) = Time_forTXT(selectedIndexes(i,1));
        MK6_Time(index) = MK6_data.Time(selectedIndexes(i,1)) - timeCutter;
        MK6_Latitude(index) = MK6_data.Latitude(selectedIndexes(i,1));
        MK6_Longitude(index) = MK6_data.Longitude(selectedIndexes(i,1));
        MK6_Altitude(index) = MK6_data.Altitude(selectedIndexes(i,1));
        MK6_Speed(index) = MK6_data.Speed(selectedIndexes(i,1));
        MK6_Distance_m(index) = current_distance;
        MK6_Elevation(index) = (180/pi) * atan(MK6_Altitude(index)/current_distance);
        
        RLI_Time(index) = RLI_data.Time(selectedIndexes(i,2)) - timeCutter;
        RLI_Distance_m(index) = RLI_data.Distance_m(selectedIndexes(i,2));
        RLI_Dopler_mps(index) = RLI_data.Dopler_mps(selectedIndexes(i,2));
    else
        filteredIndexes(index+1,:) = [];
    end
end

clear timeCutter;
clear L;
clear current_distance;
clear RLI_data;
clear MK6_data;
clear Time_forTXT;
clear selectedIndexes;

% resize arrays after distance selection
MK6_Time = MK6_Time(1:index);
MK6_Latitude = MK6_Latitude(1:index);
MK6_Longitude = MK6_Longitude(1:index);
MK6_Altitude = MK6_Altitude(1:index);
MK6_Speed = abs(MK6_Speed(1:index));
MK6_Distance_m = MK6_Distance_m(1:index);
MK6_Elevation = MK6_Elevation(1:index);

RLI_Time = RLI_Time(1:index);
RLI_Distance_m = RLI_Distance_m(1:index);
RLI_Dopler_mps = abs(RLI_Dopler_mps(1:index));

Time = Time(1:index);

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%% CALCULATE STD
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
RLI_DistanceFlat_m = zeros(index,1);
RLI_Dopler_Flat_mps = zeros(index,1);

deltaDistance = zeros(index,1);
deltaDopler = zeros(index,1);

distanceSTDsum = 0;
doplerSTDsum = 0;

for i=1:size(filteredIndexes,1)
    if (useCalculatedElevation == true)
        RLI_DistanceFlat_m(i) = RLI_Distance_m(i)*cos((pi/180) * MK6_Elevation(i));
        RLI_Dopler_Flat_mps(i) = RLI_Dopler_mps(i) * cos((pi/180) * MK6_Elevation(i));
    else
        RLI_DistanceFlat_m(i) = RLI_Distance_m(i)*cos((pi/180) * RLI_Elevation);
        RLI_Dopler_Flat_mps(i) = RLI_Dopler_mps(i) * cos((pi/180) * RLI_Elevation);
    end
    
    deltaDistance(i) =  abs(RLI_DistanceFlat_m(i) - MK6_Distance_m(i));
    deltaDopler(i) =  abs(RLI_Dopler_Flat_mps(i) - MK6_Speed(i)); 
    
    distanceSTDsum = distanceSTDsum + power(deltaDistance(i), 2);
    doplerSTDsum = doplerSTDsum + power(deltaDopler(i), 2);
end
    
STD_distance = sqrt(distanceSTDsum/(index-1));
STD_dopler = sqrt(doplerSTDsum/(index-1));

clear distanceSTDsum;
clear doplerSTDsum;

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%% SAVE .TXT
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
file_id=fopen([outputFileName '.txt'],'w');
for i=1:index
    fprintf(file_id, '%10.3f\t%7.1f\t%7.1f\t%5.1f\t%5.1f\n', ...
        Time(i), ...
        MK6_Distance_m(i), RLI_DistanceFlat_m(i), ...
        MK6_Speed(i), RLI_Dopler_Flat_mps(i));
end
fclose(file_id);

clear i;
clear file_id;
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%% PLOT GRAPHICS
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
if (index>0)
    
    figure('units','normalized','outerposition',[0 0 1 1]);
    
    subplot(2,2,1);
    plot(MK6_Time, MK6_Distance_m, 'Color', 'red', 'Marker', '.');
    hold on;
    grid on;
    plot(RLI_Time, RLI_DistanceFlat_m, 'Color', 'blue', 'Marker', '.');
    plot([RLI_Time(1) RLI_Time(size(RLI_Time, 1))], [Lmin Lmin], 'Color', 'black', 'LineStyle', ':', 'LineWidth', 2);
    plot([RLI_Time(1) RLI_Time(size(RLI_Time, 1))], [Lmax Lmax], 'Color', 'black', 'LineStyle', ':', 'LineWidth', 2);
    title('Äàëüíîñòü');
    legend('ÄÀËÜÍÎÑÒÜ_Ì_Ê_6', 'ÄÀËÜÍÎÑÒÜ_Ğ_Ë_È');
    
    
    subplot(2,2,2);
    plot(MK6_Time, deltaDistance, 'Color', 'blue', 'Marker', '.');
    grid on;
    title(['ÎØÈÁÊÀ_ä_à_ë_ü_í' ', ÑÊÎ_ä_à_ë_ü_í = '  num2str(STD_distance)]);
    
    subplot(2,2,3);
    plot(MK6_Time, MK6_Speed, 'Color', 'red', 'Marker', '.');
    hold on;
    grid on;
    plot(RLI_Time, RLI_Dopler_Flat_mps, 'Color', 'blue', 'Marker', '.');
    title('Äîïëåğ');
    legend('ÄÎÏËÅĞ_Ì_Ê_6', 'ÄÎÏËÅĞ_Ğ_Ë_È');
    
    subplot(2,2,4);
    plot(MK6_Time, deltaDopler, 'Color', 'blue', 'Marker', '.');
    grid on;
    title(['ÎØÈÁÊÀ_ä_î_ï_ë_å_ğ' ', ÑÊÎ_ä_î_ï_ë_å_ğ = ' num2str(STD_dopler)]);
    
    saveas(gcf, outputFileName,'png');
end

clear Lmin;
clear Lmax;
clear R;
clear index;
clear uotputFileName;


