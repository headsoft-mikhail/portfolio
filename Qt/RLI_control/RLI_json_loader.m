clear all; clc; close all;
filename = 'C:\Users\Afanasiev_MA\Desktop\2021.06.21_15-07-04_data.json';
jsonData = jsondecode(fileread(filename));
cellData = struct2array(jsonData.Data);
numData = zeros(size(cellData,1),size(cellData,2));
for i=1:size(cellData,1)
    for j=1:size(cellData,2)
        numData(i,j) = str2num(cell2mat(cellData(i,j)));
    end
end
for i=1:size(cellData,2)/4
    measData.distances(:,i) = numData(:,4*1-2);
    measData.amplitudes(:,i) = numData(:,4*i-3);
    measData.noises(:,i) = numData(:,4*i);
    measData.doplers(:,i) = numData(:,4*1-1);
end
clear i j cellData numData;

i=1;
plot(measData.distances(:,i),measData.amplitudes(:,i))
hold on
plot(measData.distances(:,i),measData.noises(:,i))



