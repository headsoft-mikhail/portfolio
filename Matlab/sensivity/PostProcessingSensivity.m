function varargout = PostProcessingSensivity(varargin)
% Begin initialization code - DO NOT EDIT
gui_Singleton = 1;
gui_State = struct('gui_Name',       mfilename, ...
                   'gui_Singleton',  gui_Singleton, ...
                   'gui_OpeningFcn', @PostProcessingSensivity_OpeningFcn, ...
                   'gui_OutputFcn',  @PostProcessingSensivity_OutputFcn, ...
                   'gui_LayoutFcn',  [] , ...
                   'gui_Callback',   []);
if nargin && ischar(varargin{1})
    gui_State.gui_Callback = str2func(varargin{1});
end

if nargout
    [varargout{1:nargout}] = gui_mainfcn(gui_State, varargin{:});
else
    gui_mainfcn(gui_State, varargin{:});
end
% End initialization code - DO NOT EDIT

% --- Executes just before PostProcessingSensivity is made visible.
function PostProcessingSensivity_OpeningFcn(hObject, eventdata, handles, varargin)
handles.output = hObject;
global THDataFile;
THDataFile=0;
global PDFDataFile;
PDFDataFile=0;
global noiseData;
noiseData=0;
global ExpDataFiles;
ExpDataFiles=0;
global NoiseDataFile;
NoiseDataFile=0;
% Update handles structure
guidata(hObject, handles);
set(handles.figure1,'position',[40 40 82 48]);  %положение окна на экране
set(handles.figure1,'Name', 'Расчет чувствительности');  %заголовок окна

% --- Outputs from this function are returned to the command line.
function varargout = PostProcessingSensivity_OutputFcn(hObject, eventdata, handles) 
varargout{1} = handles.output;

% --- Executes on button press in AddExpDataButton.
function AddExpDataButton_Callback(hObject, eventdata, handles)
% set(handles.ResetExpDataButton, 'Enable', 'off')
global ExpDataFiles;
ExpDataFiles=uigetfile({'.mat'}, 'Загрузка измеренного КУ антенного элемента');
if ExpDataFiles==0
    if strcmp(get(handles.AddedExpFilesText, 'String'), 'данные не загружены')==0
        ExpDataFiles=get(handles.AddedExpFilesText, 'String');
    end
    return;
else
    set(handles.AddedExpFilesText, 'String', ExpDataFiles);
    set(handles.ResetExpDataButton, 'Enable', 'on');
end

% --- Executes on button press in AddNoiseDataButton.
function AddNoiseDataButton_Callback(hObject, eventdata, handles)
global NoiseDataFile;
NoiseDataFile=uigetfile({'.mat'}, 'Загрузка шумовой дорожки');
if NoiseDataFile==0
    if strcmp(get(handles.AddedNoiseFilesText, 'String'), 'данные не загружены')==0
        NoiseDataFile=get(handles.AddedNoiseFilesText, 'String');
    end
    return;
else
    set(handles.AddedNoiseFilesText, 'String', NoiseDataFile);
    set(handles.ResetExpDataButton, 'Enable', 'on');
end
    
% --- Executes on button press in CountExpButton.
function CountExpButton_Callback(hObject, eventdata, handles)
global ExpDataFiles;
global NoiseDataFile;
global SensivityData;
if ExpDataFiles==0
    errordlg('Не выбран файл с измеренным КУ','Ошибка');
    return;
end 
if NoiseDataFile==0
    errordlg('Не выбран файл с шумовой дорожкой','Ошибка') ;  
    return;
end
load(NoiseDataFile);
load(ExpDataFiles);
SensivityData.freqsExp = max(min(NoiseData.freqs), min(GainData.freqs)):1:min(max(NoiseData.freqs), max(GainData.freqs));
SensivityData.gainExp = interp1(GainData.freqs, GainData.gain, SensivityData.freqsExp,'linear');
SensivityData.noiseExp = interp1(NoiseData.freqs, NoiseData.noise, SensivityData.freqsExp,'linear');
%расчет
SensivityData.sensivityExp=((10^6)*SensivityData.freqsExp.*sqrt(1e3*str2num(get(handles.BandEdit, 'String')))*(9.73*sqrt(2)/600).*10.^((SensivityData.noiseExp-SensivityData.gainExp)/20))/sqrt(str2num(get(handles.InputBandEdit,'String')))
axes(handles.axes1);
plot(SensivityData.freqsExp,SensivityData.sensivityExp, 'Color', get(handles.ColorButton, 'BackgroundColor'));
xlabel('Частота, МГц', 'FontName', 'Arial');
ylabel('Чувствительность, мкВ/м',  'FontName', 'Arial');
title(['Чувствительность в полосе ' get(handles.BandEdit, 'String') ' кГц'], 'FontName', 'Arial');
grid on;
hold on;


function ResetExpDataButton_Callback(hObject, eventdata, handles)
global ExpDataFiles;
ExpDataFiles=0;
global NoiseDataFile;
NoiseDataFile=0;
set(handles.AddedExpFilesText, 'String', 'данные не загружены');
set(handles.AddedNoiseFilesText, 'String', 'данные не загружены');
set(handles.ResetExpDataButton, 'Enable', 'off');

% --- Executes on button press in AddPDFDataButton.
function AddPDFDataButton_Callback(hObject, eventdata, handles)
global PDFDataFile
PDFDataFile=uigetfile({'.mat'}, 'Загрузка данных усилителя');
if PDFDataFile==0
    if strcmp(get(handles.AddedPDFFilesText, 'String'), 'данные не загружены')==0
        PDFDataFile=get(handles.AddedPDFFilesText, 'String');
    end
    return;
else
set(handles.AddedPDFFilesText, 'String', PDFDataFile)
set(handles.ResetTHDataButton, 'Enable', 'on');
end

% --- Executes on button press in AddTHDataButton.
function AddTHDataButton_Callback(hObject, eventdata, handles)
global THDataFile
THDataFile=uigetfile({'.mat'}, 'Загрузка данных моделирования антенного элемента');
if  THDataFile==0
if strcmp(get(handles.AddedTHFilesText, 'String'), 'данные не загружены')==0
        THDataFile=get(handles.AddedTHFilesText, 'String');
    end
    return;
else
set(handles.AddedTHFilesText, 'String', THDataFile);
set(handles.ResetTHDataButton, 'Enable', 'on');
end


% --- Executes on button press in CountTHButton.
function CountTHButton_Callback(hObject, eventdata, handles)
%%   Расчёт
global THDataFile;
global PDFDataFile;
global SensivityData;

if PDFDataFile==0;
    errordlg('Не выбран файл с характеристиками усилителя','Ошибка');
    return;
end 
if THDataFile==0;
    errordlg('Не выбран файл с расчетными данными антенны','Ошибка') ;  
    return;
end

load(THDataFile);
load(PDFDataFile);
% частоты для теоретического графика
SensivityData.freqsTH=max(min(PDFData.freqs),min(THData.freqs)):1:min(max(PDFData.freqs),max(THData.freqs));
% расчетные параметры АЭ 
SensivityData.gainTH=interp1(THData.freqs,THData.gain,SensivityData.freqsTH,'linear'); 
% параметры усилителя по описанию
SensivityData.noisePDF=interp1(PDFData.freqs,PDFData.noise,SensivityData.freqsTH,'linear');
% расчет чувствительности [мкВ/м]
SensivityData.sensivityTH=0.00145*SensivityData.freqsTH.*sqrt(str2num(get(handles.BandEdit, 'String'))).*10.^((SensivityData.noisePDF-SensivityData.gainTH)/20);
% вывод графика
axes(handles.axes1);
plot(SensivityData.freqsTH,SensivityData.sensivityTH, 'Color', get(handles.ColorButton, 'BackgroundColor'),'LineStyle', '--');
xlabel('Частота, МГц', 'FontName', 'Arial');
ylabel('Чувствительность, мкВ/м',  'FontName', 'Arial');
title(['Чувствительность в полосе ' get(handles.BandEdit, 'String') ' кГц'], 'FontName', 'Arial');
grid on;
hold on;



% --- Executes on button press in ResetTHDataButton.
function ResetTHDataButton_Callback(hObject, eventdata, handles)
global THDataFiles;
global PDFDataFiles;
THDataFiles=0;
PDFDataFiles=0;
set(handles.AddedTHFilesText, 'String', 'данные не загружены');
set(handles.AddedPDFFilesText, 'String', 'данные не загружены');
set(handles.ResetTHDataButton, 'Enable', 'off');


function BandEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function BandEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'));
    set(hObject,'BackgroundColor','white');
end

% --- Executes on button press in FAQButton.
function FAQButton_Callback(hObject, eventdata, handles)
helpdlg({'Загрузите необходимые данные: программа поддерживает';...
    'последовательную загрузку нескольких файлов с данными эксперимента.';...
    'Укажите полосу частот и произведите расчет.'; ...
    'Для сравнения результатов с теоретическими данными,';...
    'добавьте КУ усилителя и расчетный КУ антенного элемента,';...
    'укажите полосу частот, произведите расчет и сохраните результат';...
    'Формат данных для ввода:';
    'Коэффициент усиления (измеренный): GainData.gain, GainData.freqs';
    'Шумовая дорожка: NoiseData.freqs,NoiseData.noise';
    'Данные усилителя: PDFData.gain, PDFData.noise, PDFData.freqs';
    'Коэффициент усиления (теоретический): THData.gain, THData.freqs';
    },'Памятка'); 

% --- Executes on button press in SaveDataButton.
function SaveDataButton_Callback(hObject, eventdata, handles)
global SensivityData
toSave=uiputfile({'.mat'}, 'Сохранение данных');
if toSave==0
    return;
else
    save(toSave, 'SensivityData')
end;

% --- Executes on button press in ExitButton.
function ExitButton_Callback(hObject, eventdata, handles)
close;

% --- Executes on button press in ClearAllButton.
function ClearAllButton_Callback(hObject, eventdata, handles)
global THDataFile;
THDataFile=0;
global PDFDataFile;
PDFDataFile=0;
global noiseData;
noiseData=0;
global ExpDataFiles;
ExpDataFiles=0;
global NoiseDataFile;
NoiseDataFile=0;
clear SensivityData
global SensivityData;
clear SensivityData
set(handles.AddedTHFilesText, 'String', 'данные не загружены');
set(handles.AddedPDFFilesText, 'String', 'данные не загружены');
set(handles.ResetTHDataButton, 'Enable', 'off');
set(handles.AddedExpFilesText, 'String', 'данные не загружены');
set(handles.AddedNoiseFilesText, 'String', 'данные не загружены');
set(handles.ResetExpDataButton, 'Enable', 'off');
cla(handles.axes1,'reset')


% --- Executes on button press in ClearGraphButton.
function ClearGraphButton_Callback(hObject, eventdata, handles)
cla(handles.axes1,'reset')

function InputBandEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function InputBandEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

% --- Executes on button press in ColorButton.
function ColorButton_Callback(hObject, eventdata, handles)
if get(handles.ColorButton, 'BackgroundColor')==[0 0 0]
    set(handles.ColorButton, 'BackgroundColor',[0 0 1])
    return
end
if get(handles.ColorButton, 'BackgroundColor')==[0 0 1]
    set(handles.ColorButton, 'BackgroundColor',[1 0 0])
    return
end
if get(handles.ColorButton, 'BackgroundColor')==[1 0 0]
    set(handles.ColorButton, 'BackgroundColor',[0 0.5 0])
    return
end
if get(handles.ColorButton, 'BackgroundColor')==[0 0.5 0]
    set(handles.ColorButton, 'BackgroundColor',[0 1 1])
    return
end
if get(handles.ColorButton, 'BackgroundColor')==[0 1 1]
    set(handles.ColorButton, 'BackgroundColor',[1 0 1])
    return
end
if get(handles.ColorButton, 'BackgroundColor')==[1 0 1]
    set(handles.ColorButton, 'BackgroundColor',[0.93 0.69 0.13])
    return
end
if get(handles.ColorButton, 'BackgroundColor')==[0.93 0.69 0.13]
    set(handles.ColorButton, 'BackgroundColor',[0 0 0])
    return
end


% --- Executes on button press in OpenFigButton.
function OpenFigButton_Callback(hObject, eventdata, handles)
    FIG=figure(1);
    handleLines=get(handles.axes1, 'Children');
    handleColor=get(get(handles.axes1, 'Children'),'Color');
    handleLineStyle=get(get(handles.axes1, 'Children'),'LineStyle');
    for line=1:size(handleLines)
        if size(handleLines)==1
            plot(get(handleLines(line),'XData').', get(handleLines(line),'YData').', 'Color', handleColor, 'LineStyle', handleLineStyle);
        else
            plot(get(handleLines(line),'XData').', get(handleLines(line),'YData').', 'Color', handleColor{line}, 'LineStyle', handleLineStyle{line});
        end
        hold on;
    end
    grid on;
    xlabel('Частота, МГц', 'FontName', 'Arial');
    ylabel('Чувствительность, мкВ/м',  'FontName', 'Arial');
    title(['Чувствительность в полосе ' get(handles.BandEdit, 'String') ' кГц'], 'FontName', 'Arial');

    
% --- Executes on button press in SaveGraphButton.
function SaveGraphButton_Callback(hObject, eventdata, handles)
% axes(handles.axes1)
toSave=uiputfile({'.fig'}, 'Сохранение графика');
if toSave==0
    return;
else
    SaveFigure=figure;
    handleLines=get(handles.axes1, 'Children');
    handleColor=get(get(handles.axes1, 'Children'),'Color');
    handleLineStyle=get(get(handles.axes1, 'Children'),'LineStyle');
    for line=1:size(handleLines)
        if size(handleLines)==1
            plot(get(handleLines(line),'XData').', get(handleLines(line),'YData').', 'Color', handleColor, 'LineStyle', handleLineStyle);
        else
            plot(get(handleLines(line),'XData').', get(handleLines(line),'YData').', 'Color', handleColor{line}, 'LineStyle', handleLineStyle{line});
        end
        hold on;
    end
    grid on;
    xlabel('Частота, МГц', 'FontName', 'Arial');
    ylabel('Чувствительность, мкВ/м',  'FontName', 'Arial');
    title(['Чувствительность в полосе ' get(handles.BandEdit, 'String') ' кГц'], 'FontName', 'Arial');
    saveas(SaveFigure, toSave);
    close;

end
    
    
% --- Executes on button press in LoadMatButton.
function LoadMatButton_Callback(hObject, eventdata, handles)
global SensivityData
toLoad=uigetfile({'.mat'}, 'Загрузка чувствительности')
if toLoad==0
    return
end
clear SensivityData;
load(toLoad, 'SensivityData');
if isfield(SensivityData,'gainExp')
    figure(1)
    plot(SensivityData.freqsExp,SensivityData.sensivityExp, 'Color', get(handles.ColorButton, 'BackgroundColor'))
    grid on
    hold on
    xlabel('Частота, МГц', 'FontName', 'Arial');
    ylabel('Чувствительность, мкВ/м',  'FontName', 'Arial');
    title(['Чувствительность в полосе ' get(handles.BandEdit, 'String') ' кГц'], 'FontName', 'Arial');
end
if isfield(SensivityData,'gainTH')
    figure(1)
    plot(SensivityData.freqsTH,SensivityData.sensivityTH, 'Color', get(handles.ColorButton, 'BackgroundColor'))
    grid on
    hold on
    xlabel('Частота, МГц', 'FontName', 'Arial');
    ylabel('Чувствительность, мкВ/м',  'FontName', 'Arial');
    title(['Чувствительность в полосе ' get(handles.BandEdit, 'String') ' кГц'], 'FontName', 'Arial');
end
clear SensivityData;
