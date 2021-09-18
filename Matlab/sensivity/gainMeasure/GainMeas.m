function varargout = GainMeas(varargin)
% Begin initialization code - DO NOT EDIT
gui_Singleton = 1;
gui_State = struct('gui_Name',       mfilename, ...
    'gui_Singleton',  gui_Singleton, ...
    'gui_OpeningFcn', @GainMeas_OpeningFcn, ...
    'gui_OutputFcn',  @GainMeas_OutputFcn, ...
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


% --- Executes just before GainMeas is made visible.
function GainMeas_OpeningFcn(hObject, eventdata, handles, varargin)
handles.output = hObject;
guidata(hObject, handles);
set(handles.figure1,'position',[40 40 134.5 52]);  %положение окна на экране
set(handles.figure1,'Name', 'Измерение коэффициента усиления');  %заголовок окна
global but
but=0;
global opuFlag
opuFlag=0;
global exitFlag
exitFlag=0;
global cableLoss
cableLoss.KP=zeros(1,28);
cableLoss.freqs=str2num(get(handles.FreqStartEdit, 'String'))...
    :str2num(get(handles.FreqStepEdit, 'String'))...
    :str2num(get(handles.FreqStopEdit, 'String'));
global measData
measData.KP=zeros(28,1);
measData.freqs=cableLoss.freqs;
measData.antType='HK116';
measData.distToAnt=str2num(get(handles.DistToAntEdit,'String'));
axes(handles.axes1);
plot(measData.freqs, measData.KP);
xlabel('Частота, МГц', 'FontName', 'Arial','FontSize',7);
ylabel('КП, дБ',  'FontName', 'Arial','FontSize',7);
title('Коэффициент передачи', 'FontName', 'Arial','FontSize',7);
grid on;
axes(handles.axes2);
plot(cableLoss.freqs, cableLoss.KP);
xlabel('Частота, МГц', 'FontName', 'Arial','FontSize',7);
ylabel('КП, дБ',  'FontName', 'Arial','FontSize',7);
title('Коэффициент передачи кабеля', 'FontName', 'Arial','FontSize',7);
grid on;
axes(handles.axes3);
grid on;
if get(handles.HoldBox,'Value')==1
    hold on;
else
    hold off;
end
xlabel('Частота, МГц', 'FontName', 'Arial','FontSize',7);
ylabel('КУ, дБ',  'FontName', 'Arial','FontSize',7);
title('Коэффициент усиления, дБ', 'FontName', 'Arial','FontSize',7);

% --- Outputs from this function are returned to the command line.
function varargout = GainMeas_OutputFcn(hObject, eventdata, handles)
varargout{1} = handles.output;

% --- Executes on button press in KPMeasureButton.
function KPMeasureButton_Callback(hObject, eventdata, handles)
global measData
measData.freqStart=str2num(get(handles.FreqStartEdit, 'String'));
measData.freqStop=str2num(get(handles.FreqStopEdit, 'String'));
measData.freqStep=str2num(get(handles.FreqStepEdit, 'String'));
measData.averageFactor=str2num(get(handles.AverEdit, 'String'));
if measData.freqStart>measData.freqStop
    errordlg('Начальная частота больше конечной!','Ошибка!');
    textOUT('Ошибка!',1,handles);
    textOUT('--------------------------------',1,handles);
    return;
end
COM=999;
switch get(handles.AnalyzerMenu,'Value')
    case 1
        measData.analyzerType='ZVA 40';
        if measData.freqStop>40000|measData.freqStart<10
            errordlg('Недопустимое значение частоты для выбранного анализатора!','Ошибка!');
            textOUT('Ошибка!',1,handles);
            textOUT('--------------------------------',1,handles);
            return;
        end
    case 2
        measData.analyzerType='Anritsu MS2028C'
        if measData.freqStop>20000|measData.freqStart<0.005
            errordlg('Недопустимое значение частоты для выбранного анализатора!','Ошибка!');
            textOUT('Ошибка!',1,handles);
            textOUT('--------------------------------',1,handles);
            return;
        end
    case 3
        measData.analyzerType='FSH 6';
        measData.freqStep=(measData.freqStop-measData.freqStart)/300;
        set(handles.FreqStepEdit,'String', measData.freqStep);
        COM=str2num(get(handles.ComEdit,'String'));
        if measData.freqStop>6000|measData.freqStart<0.1
            errordlg('Недопустимое значение частоты для выбранного анализатора!','Ошибка!');
            textOUT('Ошибка!',1,handles);
            textOUT('--------------------------------',1,handles);
            return;
        end
end
measData.freqs=measData.freqStart:measData.freqStep:measData.freqStop;
measData.freqPoints=size(measData.freqs,2);
if str2num(get(handles.DistToAntEdit, 'String'))==0
    errordlg('Недопустимое значение расстояния между антеннами!','Ошибка!');
    textOUT('Ошибка!',1,handles);
    textOUT('--------------------------------',1,handles);
    return;
end
measData.distToAnt=str2num(get(handles.DistToAntEdit, 'String'));
switch get(handles.AntTypeMenu,'Value')
    case 1
        measData.antType='HK116';
    case 2
        measData.antType='HL223';
    case 3
        measData.antType='HL223hor';
    case 4
        measData.antType='П6-040';
    case 5
        measData.antType='П6-023';
    case 6
        measData.antType='LINDGREN';
    case 7
        measData.antType='VULB';
end

if strcmp(num2str(GetGain(measData.freqStart,measData.antType)),'NaN')...
        ||strcmp(num2str(GetGain(measData.freqStop,measData.antType)),'NaN')
    errordlg('Недопустимые значения частот для выбранной измерительной антенны!','Ошибка!');
    textOUT('Ошибка!',1,handles);
    textOUT('--------------------------------',1,handles);
    return;
end
textOUT(['Выбрана антенна ',measData.antType],1,handles);

%измерение КП
measData.KP=MEAS(measData,COM,handles);

if get(handles.AnalyzerMenu,'Value')==3  %затычка для FSH6
    return;
end

axes(handles.axes1);
plot(measData.freqs, measData.KP);
xlabel('Частота, МГц', 'FontName', 'Arial','FontSize',7);
ylabel('КП, дБ',  'FontName', 'Arial','FontSize',7);
title('Коэффициент передачи', 'FontName', 'Arial','FontSize',7);
grid on;
textOUT('Измерение завершено!',1,handles)
textOUT('--------------------------------',1,handles);

% --- Executes on button press in EvalGainButton.
function EvalGainButton_Callback(hObject, eventdata, handles)
global measData
global cableLoss
global GainData
if get(handles.CableBox, 'Value')==1
    cableLoss.KP=zeros(size(measData.freqs,2),1);
    cableLoss.freqs=measData.freqs;
    axes(handles.axes2);
    plot(measData.freqs,cableLoss.KP);
    xlabel('Частота, МГц', 'FontName', 'Arial','FontSize',7);
    ylabel('КП, дБ',  'FontName', 'Arial','FontSize',7);
    title('Коэффициент передачи кабеля', 'FontName', 'Arial','FontSize',7);
    grid on;
end
if str2num(get(handles.DistToAntEdit, 'String'))==0
    errordlg('Недопустимое значение расстояния между антеннами!','Ошибка!');
    textOUT('Ошибка!',1,handles);
    textOUT('--------------------------------',1,handles);
    return;
end
measData.distToAnt=str2num(get(handles.DistToAntEdit, 'String'));
tmp=interp1(cableLoss.freqs,cableLoss.KP', measData.freqs);
if strcmp(num2str(tmp(1)),'NaN')|strcmp(num2str(tmp(size(tmp,2))),'NaN')==1
    errordlg('Не совпадают частотные диапазоны измерений КП антенны и кабеля!','Ошибка!');
    textOUT('Ошибка!',1,handles);
    textOUT('--------------------------------',1,handles);
    return;
end
GainData.gain=measData.KP-GetGain(measData.freqs,measData.antType)...
    -20*log10((300./measData.freqs)/(4*pi*measData.distToAnt))...
    -interp1(cableLoss.freqs,cableLoss.KP', measData.freqs);
GainData.freqs=measData.freqs;
textOUT('Расчет выполнен!',1,handles);
textOUT('--------------------------------',1,handles);
axes(handles.axes3);
if get(handles.HoldBox,'Value')==1
    hold on;
else
    hold off;
end
plot(GainData.freqs,GainData.gain,'Color', get(handles.ColorButton, 'BackgroundColor'));
grid on;
xlabel('Частота, МГц', 'FontName', 'Arial','FontSize',7);
ylabel('КУ, дБ',  'FontName', 'Arial','FontSize',7);
title('Коэффициент усиления, дБ', 'FontName', 'Arial','FontSize',7);
if get(handles.ColorBox, 'Value')==1
    ColorButton_Callback(hObject, eventdata, handles);
end
textOUT('Измерение завершено!',1,handles);
set(handles.SaveButton,'Enable', 'on');



% --- Executes on button press in SaveButton.
function SaveButton_Callback(hObject, eventdata, handles)
global GainData
global measData
toSave=uiputfile({'.mat'}, 'Сохранение данных');
if toSave==0
    return;
    textOUT('Не сохранено!',1,handles);
    textOUT('--------------------------------',1,handles);
else
    save(toSave, 'GainData');
    save(toSave, 'measData','-append');
    textOUT('Сохранено!',1,handles);
    textOUT('--------------------------------',1,handles);
end;


% --- Executes on button press in HelpButton.
function HelpButton_Callback(hObject, eventdata, handles)
helpdlg({'Выберите нужные параметры измерения';...
    'Выберите вариант учета потерь в кабеле';...
    'Произведите измерения и рассчитайте КУ';...
    'Не забудьте сохранить результат!';...
    'Загружаемый mat-файл с данными КП должен содержать';...
    'структуру measData c полями KP, freqs, distToAnt, antType.';...
    'Загружаемый mat-файл с данными кабеля должен содержать';...
    'структуру CableLoss c полями KP и freqs.';...
    'Загружаемый mat-файл c данными КУ должен содержать';...
    'структуру GainData с полями gain и freqs.'; },'Памятка');

% --- Executes on button press in ExitButton.
function ExitButton_Callback(hObject, eventdata, handles)
answer=questdlg('Сохранить последние настройки перед выходом?',...
    'Выйход','Выход','Выйти и сохранить','Отмена', 'Выход');
if strcmp(answer,'Отмена')==1
    return;
elseif strcmp(answer,'Выйти и сохранить')==1
    freqStart=get(handles.FreqStartEdit, 'String');
    freqStep=get(handles.FreqStepEdit, 'String');
    freqStop=get(handles.FreqStopEdit, 'String');
    averageFactor=get(handles.AverEdit, 'String');
    antType=get(handles.AntTypeMenu, 'Value');
    analyzerType=get(handles.AnalyzerMenu,'Value');
    distToAnt=get(handles.DistToAntEdit,'String');
    save('LastSettings.mat','freqStart','freqStop','freqStep',...
        'analyzerType','antType','distToAnt','averageFactor');
end
global exitFlag
exitFlag=1;
close all;

% --- Executes on button press in ClearButton.
function ClearButton_Callback(hObject, eventdata, handles)
cla(handles.axes3,'reset');
grid on;
if get(handles.HoldBox,'Value')==1
    hold on;
else
    hold off;
end
xlabel('Частота, МГц', 'FontName', 'Arial','FontSize',7);
ylabel('КУ, дБ',  'FontName', 'Arial','FontSize',7);
title('Коэффициент усиления, дБ', 'FontName', 'Arial','FontSize',7);

% --- Executes on button press in CableBox.
function CableBox_Callback(hObject, eventdata, handles)
global cableLoss
global measData
if get(handles.CableBox, 'Value')==1
    set(handles.LoadCableButton, 'Enable', 'off');
    set(handles.MeasCableButton, 'Enable', 'off');
    set(handles.CableSaveButton,'Enable','off');
    axes(handles.axes2);
    TMP.KP=zeros(size(measData.freqs,2),1);
    TMP.freqs=measData.freqs;
    plot(TMP.freqs, TMP.KP);
    xlabel('Частота, МГц', 'FontName', 'Arial','FontSize',7);
    ylabel('КП, дБ',  'FontName', 'Arial','FontSize',7);
    title('Коэффициент передачи кабеля', 'FontName', 'Arial','FontSize',7);
    grid on;
end
if get(handles.CableBox, 'Value')==0
    set(handles.LoadCableButton, 'Enable', 'on');
    set(handles.MeasCableButton, 'Enable', 'on');
    set(handles.CableSaveButton,'Enable','on');
    axes(handles.axes2);
    plot(cableLoss.freqs, cableLoss.KP);
    xlabel('Частота, МГц', 'FontName', 'Arial','FontSize',7);
    ylabel('КП, дБ',  'FontName', 'Arial','FontSize',7);
    title('Коэффициент передачи кабеля', 'FontName', 'Arial','FontSize',7);
    grid on;
end

% --- Executes on button press in MeasCableButton.
function MeasCableButton_Callback(hObject, eventdata, handles)
global cableLoss
cableData.freqStart=str2num(get(handles.FreqStartEdit, 'String'));
cableData.freqStop=str2num(get(handles.FreqStopEdit, 'String'));
cableData.freqStep=str2num(get(handles.FreqStepEdit, 'String'));
if cableData.freqStart>cableData.freqStop
    errordlg('Начальная частота больше конечной!','Ошибка!');
    textOUT('Ошибка!',1,handles);
    textOUT('--------------------------------',1,handles);
    return;
end
COM=999
switch get(handles.AnalyzerMenu,'Value')
    case 1
        cableData.analyzerType='ZVA 40';
    case 2
        cableData.analyzerType='Anritsu MS2028C';
    case 3
        cableData.analyzerType='FSH 6';
        cableData.freqStep=(cableData.freqStop-cableData.freqStart)/300;
        set(handles.FreqStepEdit,'String', cableData.freqStep);
        COM=str2num(get(handles.ComEdit,'String'));
end
cableData.freqs=cableData.freqStart:cableData.freqStep:cableData.freqStop;
cableLoss.KP=MEAS(cableData,COM,handles);

if get(handles.AnalyzerMenu,'Value')==3  %затычка для FSH6
    return;
end

cableLoss.freqs=cableData.freqs
textOUT('Измерение завершено!',1,handles);
textOUT('--------------------------------',1,handles);
axes(handles.axes2);
plot(cableLoss.freqs,cableLoss.KP);
xlabel('Частота, МГц', 'FontName', 'Arial','FontSize',7);
ylabel('КП, дБ',  'FontName', 'Arial','FontSize',7);
title('Коэффициент передачи кабеля', 'FontName', 'Arial','FontSize',7);
grid on;
set(handles.CableSaveButton,'Enable','on');

% --- Executes on button press in LoadCableButton.
function LoadCableButton_Callback(hObject, eventdata, handles)
global cableLoss
toLoad=uigetfile({'.mat'}, 'Загрузка коэффициента передачи кабеля');
if toLoad==0
    return;
end
load(toLoad, 'cableLoss');
axes(handles.axes2);
plot(cableLoss.freqs,cableLoss.KP);
xlabel('Частота, МГц', 'FontName', 'Arial','FontSize',7);
ylabel('КП, дБ',  'FontName', 'Arial','FontSize',7);
title('Коэффициент передачи кабеля', 'FontName', 'Arial','FontSize',7);
grid on;

% --- Executes on button press in LoadKPButton.
function LoadKPButton_Callback(hObject, eventdata, handles)
global measData
toLoad=uigetfile({'.mat'}, 'Загрузка коэффициента передачи');
if toLoad==0
    return
end
load(toLoad, 'measData');
axes(handles.axes1);
plot(measData.freqs, measData.KP);
xlabel('Частота, МГц', 'FontName', 'Arial','FontSize',7);
ylabel('КП, дБ',  'FontName', 'Arial','FontSize',7);
title('Коэффициент передачи', 'FontName', 'Arial','FontSize',7);
grid on;

% --- Executes on button press in HoldBox.
function HoldBox_Callback(hObject, eventdata, handles)

% --- Executes on selection change in AnalyzerMenu.
function AnalyzerMenu_Callback(hObject, eventdata, handles)
switch get(handles.AnalyzerMenu,'Value')
    case 1
        set(handles.FreqStepEdit,'Enable', 'on');
        set(handles.ComEdit,'Enable', 'off');
    case 2
        set(handles.FreqStepEdit,'Enable', 'on');
        set(handles.ComEdit,'Enable', 'off');
    case 3
        set(handles.FreqStepEdit,'Enable', 'off');
        set(handles.ComEdit,'Enable', 'on');
end

% --- Executes during object creation, after setting all properties.
function AnalyzerMenu_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

% --- Executes on selection change in AntTypeMenu.
function AntTypeMenu_Callback(hObject, eventdata, handles)
switch get(handles.AntTypeMenu,'Value')
    case 1
        set(handles.AntDiapText,'String','20-300 МГц');
    case 2
        set(handles.AntDiapText,'String','200-1300 МГц');
    case 3
        set(handles.AntDiapText,'String','200-1300 МГц');
    case 4
        set(handles.AntDiapText,'String','100-1000 МГц');
    case 5
        set(handles.AntDiapText,'String','1000-3000 МГц');
    case 6
        set(handles.AntDiapText,'String','1000-18000 МГц');
    case 7
        set(handles.AntDiapText,'String','30-8000 МГц');
end


% --- Executes during object creation, after setting all properties.
function AntTypeMenu_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function DistToAntEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function DistToAntEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function AngleEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function AngleEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function ComEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function ComEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function FreqStartEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function FreqStartEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function FreqStepEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function FreqStepEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function FreqStopEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function FreqStopEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

%подпрограмма вывода текста с замещением
function textOUT(textIN, type, handles)
text0=cellstr(get(handles.StatusText, 'String'));
if strcmp(text0(1,1),'')
    text0=textIN;
else
    switch type
        case 1 %следующей строкой
            if size(text0,1)>5 %допустимое число строк в окне StatusText
                for i=2:size(text0,1)
                    text0(i-1,1)=text0(i,1);
                end
                text0{size(text0,1),1}=textIN;
            else
                text0=[text0;textIN];
            end
        case 2 %поверх последней строчки
            text0{size(text0,1),1}=textIN;
    end
end
set(handles.StatusText, 'String', text0);

function gain = GetGain(targetFreq,antennaName)
switch antennaName
    case 'HK116'%биконус
        freqData=[20 22 24 26 28 30 32 34 36 38 40 42 44 46 48 50 52 54 56 58 60 62 64 66 68 70 72 74 76 78 80 82 84 86 88 90 92 94 96 98 100 105 110 115 120 125 130 135 140 145 150 155 160 165 170 175 180 185 190 195 200 205 210 215 220 225 230 235 240 245 250 255 260 265 270 275 280 285 290 295 300;];
        gainData=[-20.9400000000000 -19.1300000000000 -17.5600000000000 -16.1600000000000 -14.8200000000000 -13.6300000000000 -12.4900000000000 -11.3900000000000 -10.4000000000000 -9.48000000000000 -8.61000000000000 -7.76000000000000 -6.97000000000000 -6.28000000000000 -5.62000000000000 -4.99000000000000 -4.41000000000000 -3.91000000000000 -3.49000000000000 -3.10000000000000 -2.73000000000000 -2.38000000000000 -2.07000000000000 -1.78000000000000 -1.51000000000000 -1.24000000000000 -0.990000000000000 -0.760000000000000 -0.530000000000000 -0.330000000000000 -0.140000000000000 0.0400000000000000 0.180000000000000 0.290000000000000 0.380000000000000 0.470000000000000 0.570000000000000 0.610000000000000 0.670000000000000 0.730000000000000 0.780000000000000 0.850000000000000 0.910000000000000 0.950000000000000 1 1.09000000000000 1.21000000000000 1.35000000000000 1.49000000000000 1.64000000000000 1.81000000000000 2.02000000000000 2.19000000000000 2.30000000000000 2.42000000000000 2.42000000000000 2.38000000000000 2.32000000000000 2.28000000000000 2.26000000000000 2.29000000000000 2.34000000000000 2.32000000000000 1.95000000000000 2.57000000000000 2.68000000000000 2.87000000000000 2.98000000000000 3.01000000000000 3 3.05000000000000 2.89000000000000 2.93000000000000 2.80000000000000 2.64000000000000 2.29000000000000 2.07000000000000 1.66000000000000 1.37000000000000 1.09000000000000 0.650000000000000;];
    case 'HL223'%ЛПА вертикальная поляризация 3.5м
        freqData=[200 225 250 275 300 325 350 375 400 425 450 475 500 525 550 575 600 625 650 675 700 725 750 775 800 825 850 875 900 925 950 975 1000 1025 1050 1075 1100 1125 1150 1175 1200 1225 1250 1275 1300;];
        gainData=[6.20000000000000 6.80000000000000 6.80000000000000 6.90000000000000 6.90000000000000 6.90000000000000 6.90000000000000 6.90000000000000 6.70000000000000 6.20000000000000 6.70000000000000 6.80000000000000 6.80000000000000 7 7.10000000000000 6.60000000000000 6.90000000000000 6.40000000000000 6.70000000000000 6.20000000000000 6.20000000000000 6.60000000000000 6.90000000000000 7 7.10000000000000 7.20000000000000 6.60000000000000 6.40000000000000 6.80000000000000 7 7.40000000000000 7.20000000000000 7 7.40000000000000 7.10000000000000 6.40000000000000 6.30000000000000 6.80000000000000 7.20000000000000 7.30000000000000 6.90000000000000 6.40000000000000 6.60000000000000 6.80000000000000 6.80000000000000;];
    case 'HL223hor'%ЛПА горизонтальная поляризация 1.5м
        freqData=[200 225 250 275 300 325 350 375 400 425 450 475 500 525 550 575 600 625 650 675 700 725 750 775 800 825 850 875 900 925 950 975 1000 1025 1050 1075 1100 1125 1150 1175 1200 1225 1250 1275 1300;];
        gainData=[1.30000000000000 3.70000000000000 4.70000000000000 4.80000000000000 4.50000000000000 3.70000000000000 3.80000000000000 5.40000000000000 5.60000000000000 4.50000000000000 4.10000000000000 4.50000000000000 5.40000000000000 6.20000000000000 6.10000000000000 5.20000000000000 5 4.40000000000000 5.80000000000000 5.90000000000000 5.50000000000000 5 5.30000000000000 5.90000000000000 6.40000000000000 6.50000000000000 5.70000000000000 4.70000000000000 5.70000000000000 6.70000000000000 6.70000000000000 5.90000000000000 5.70000000000000 6.20000000000000 6.20000000000000 5.90000000000000 5.80000000000000 5.40000000000000 5.90000000000000 6.50000000000000 6.40000000000000 5.70000000000000 5.30000000000000 5.50000000000000 5.90000000000000;];
    case 'П6-040'
        freqData=[100 120 140 160 180 200 220 240 260 280 300 320 340 360 380 400 420 440 460 480 500 520 540 560 580 600 620 640 660 680 700 720 740 760 780 800 820 840 860 880 900 920 940 960 980 1000];
        gainData=[1.5 2 2.4 3 4.4 5.4 6 6.4 6.85 7.3 7.55 7.85 8.1 8.3 8.45 8.5 8.7 9 9.4 9.7 10.1 10.15 10.15 10.15 10.15 10.1 10 9.99 9.85 9.75 9.7 9.65 9.65 9.65 9.7 9.7 9.5 9.3 9 8.6 8.2 8 7.95 7.9 7.95 7.95];
    case 'П6-023'
        freqData=[1000 1200 1400 1600 1800 2000 2200 2400 2600 2800 3000];
        gainData=[10 11.2 12.2 13.5 14.4 15 15.8 16.2 16.8 17.5 18];
    case 'LINDGREN'
        freqData=[1000 1500 2000 2500 3000 3500 4000 4500 5000 5500 6000 6500 7000 7500 8000 8500 9000 9500 10000 10500 11000 11500 12000 12500 13000 13500 14000 14500 15000 15500 16000 16500 17000 17500 18000]
        gainData=[5.90000000000000 8.10000000000000 8.30000000000000 9.20000000000000 8.90000000000000 9 8.90000000000000 10.2000000000000 10 10.2000000000000 10.6000000000000 11 10.6000000000000 10 10.6000000000000 10.7000000000000 11 11.4000000000000 11.5000000000000 11.9000000000000 11.9000000000000 12.1000000000000 12.4000000000000 12.8000000000000 11.9000000000000 11.5000000000000 10.9000000000000 11.8000000000000 13.7000000000000 15.4000000000000 14.7000000000000 13.7000000000000 12 9.60000000000000 7.80000000000000]
    case 'VULB'
        freqData=[30;32;34;36;38;40;42;44;46;48;50;55;60;65;70;75;80;85;90;95;100;105;110;115;120;125;130;135;140;145;150;155;160;165;170;175;180;185;190;195;200;210;220;230;240;250;260;270;280;290;300;320;340;360;380;400;420;440;460;480;500;520;540;560;580;600;620;640;660;680;700;720;740;760;780;800;820;840;860;880;900;920;940;960;980;1000;1050;1100;1150;1200;1250;1300;1350;1400;1450;1500;1550;1600;1650;1700;1750;1800;1850;1900;1950;2000;2050;2100;2150;2200;2250;2300;2350;2400;2450;2500;2550;2600;2650;2700;2750;2800;2850;2900;2950;3000;3050;3100;3150;3200;3250;3300;3350;3400;3450;3500;3550;3600;3650;3700;3750;3800;3850;3900;3950;4000;4050;4100;4150;4200;4250;4300;4350;4400;4450;4500;4550;4600;4650;4700;4750;4800;4850;4900;4950;5000;5050;5100;5150;5200;5250;5300;5350;5400;5450;5500;5550;5600;5650;5700;5750;5800;5850;5900;5950;6000;6050;6100;6150;6200;6250;6300;6350;6400;6450;6500;6550;6600;6650;6700;6750;6800;6850;6900;6950;7000;7050;7100;7150;7200;7250;7300;7350;7400;7450;7500;7550;7600;7650;7700;7750;7800;7850;7900;7950;8000]';
        gainData=[-10.5600000000000;-10.1000000000000;-9.74000000000000;-9.76000000000000;-9.90000000000000;-9.91000000000000;-9.86000000000000;-9.84000000000000;-9.70000000000000;-9.48000000000000;-9.10000000000000;-7.87000000000000;-6.35000000000000;-4.37000000000000;-1.79000000000000;0.520000000000000;1.30000000000000;0.860000000000000;0.0800000000000000;-0.610000000000000;-0.820000000000000;-0.540000000000000;0.260000000000000;1.37000000000000;2.88000000000000;3.76000000000000;4.60000000000000;5.11000000000000;5.62000000000000;5.91000000000000;6.09000000000000;6.16000000000000;6.19000000000000;6.24000000000000;6.27000000000000;6.18000000000000;6.02000000000000;5.75000000000000;5.52000000000000;5.38000000000000;5.42000000000000;5.69000000000000;5.84000000000000;5.75000000000000;5.74000000000000;5.80000000000000;5.93000000000000;6.07000000000000;6.23000000000000;6.37000000000000;6.44000000000000;6.47000000000000;6.37000000000000;6.54000000000000;6.72000000000000;6.72000000000000;6.87000000000000;7.04000000000000;7.10000000000000;7.08000000000000;7.12000000000000;7.18000000000000;7.22000000000000;7.10000000000000;7.03000000000000;7.02000000000000;7.09000000000000;7.17000000000000;7.11000000000000;7.03000000000000;6.95000000000000;6.93000000000000;6.95000000000000;6.95000000000000;6.95000000000000;6.97000000000000;6.86000000000000;6.80000000000000;6.74000000000000;6.75000000000000;6.80000000000000;6.89000000000000;6.95000000000000;6.97000000000000;6.98000000000000;6.89000000000000;6.70000000000000;6.80000000000000;7;6.93000000000000;6.71000000000000;6.73000000000000;6.87000000000000;7.02000000000000;6.92000000000000;6.81000000000000;6.81000000000000;6.90000000000000;7.03000000000000;7.01000000000000;6.89000000000000;6.80000000000000;6.80000000000000;6.85000000000000;6.93000000000000;7;6.93000000000000;6.87000000000000;6.85000000000000;6.92000000000000;6.99000000000000;7.06000000000000;7.07000000000000;7.02000000000000;6.89000000000000;6.82000000000000;6.74000000000000;6.73000000000000;6.74000000000000;6.78000000000000;6.79000000000000;6.78000000000000;6.78000000000000;6.68000000000000;6.63000000000000;6.53000000000000;6.53000000000000;6.54000000000000;6.54000000000000;6.53000000000000;6.51000000000000;6.47000000000000;6.41000000000000;6.30000000000000;6.07000000000000;5.95000000000000;5.82000000000000;5.76000000000000;5.76000000000000;5.79000000000000;5.83000000000000;5.88000000000000;5.95000000000000;6.01000000000000;6.02000000000000;6;5.93000000000000;5.90000000000000;5.76000000000000;5.62000000000000;5.57000000000000;5.41000000000000;5.27000000000000;5.23000000000000;5.15000000000000;5.12000000000000;5.14000000000000;5.15000000000000;5.16000000000000;5.16000000000000;5.18000000000000;5.18000000000000;5.20000000000000;5.17000000000000;5.18000000000000;5.13000000000000;5.10000000000000;4.97000000000000;4.92000000000000;4.86000000000000;4.71000000000000;4.63000000000000;4.57000000000000;4.35000000000000;4.24000000000000;4.15000000000000;3.89000000000000;3.80000000000000;3.71000000000000;3.63000000000000;3.43000000000000;3.37000000000000;3.33000000000000;3.30000000000000;3.25000000000000;3.26000000000000;3.32000000000000;3.38000000000000;3.47000000000000;3.56000000000000;3.70000000000000;3.78000000000000;3.84000000000000;3.92000000000000;4.01000000000000;4.05000000000000;4.08000000000000;4.12000000000000;4.08000000000000;4.05000000000000;3.95000000000000;3.84000000000000;3.68000000000000;3.50000000000000;3.30000000000000;3.07000000000000;2.80000000000000;2.56000000000000;2.36000000000000;2.12000000000000;1.88000000000000;1.69000000000000;1.53000000000000;1.41000000000000;1.30000000000000;1.27000000000000;1.30000000000000;1.30000000000000;1.33000000000000;1.41000000000000;1.44000000000000;1.53000000000000;1.61000000000000;1.75000000000000;1.94000000000000;2.09000000000000]';
        
end
gain=interp1(freqData,gainData,targetFreq);

% --- Executes on button press in CableSaveButton.
function CableSaveButton_Callback(hObject, eventdata, handles)
global cableLoss
toSave=uiputfile({'.mat'}, 'Сохранение КП кабеля');
if toSave==0
    return;
    textOUT('Не сохранено!',1,handles);
    textOUT('--------------------------------',1,handles);
else
    save(toSave, 'cableLoss');
    textOUT('Сохранено!',1,handles);
    textOUT('--------------------------------',1,handles);
    set(handles.CableSaveButton,'Enable','off');
end


% --- Executes on button press in SaveGraphButton.
function SaveGraphButton_Callback(hObject, eventdata, handles)
toSave=uiputfile({'.fig'}, 'Сохранение графика');
if toSave==0
    return;
else
    SaveFigure=figure(2);
    handleLines=get(handles.axes3, 'Children');
    for line=1:size(handleLines)
        plot(get(handleLines(line),'XData').', get(handleLines(line),'YData').', 'red');
        hold on;
    end
    grid on;
    xlabel('Частота, МГц', 'FontName', 'Arial');
    ylabel('КУ, дБ',  'FontName', 'Arial');
    title('Коэффициент усиления', 'FontName', 'Arial');
    saveas(SaveFigure, toSave);
    close;
end

function AverEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function AverEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end


% --- Executes on button press in OpenFigButton.
function OpenFigButton_Callback(hObject, eventdata, handles)
FIG=figure(1);
handleLines=get(handles.axes3, 'Children');
handleColor=get(get(handles.axes3, 'Children'),'Color');
for line=1:size(handleLines)
    if size(handleLines)==1
        plot(get(handleLines(line),'XData').', get(handleLines(line),'YData').', 'Color', handleColor);
    else
        plot(get(handleLines(line),'XData').', get(handleLines(line),'YData').', 'Color', handleColor{line});
    end
    hold on;
end
grid on;
xlabel('Частота, МГц', 'FontName', 'Arial');
ylabel('КУ, дБ',  'FontName', 'Arial');
title(['Коэффициент усиления'], 'FontName', 'Arial');

% --- Executes on button press in ColorBox.
function ColorBox_Callback(hObject, eventdata, handles)
if get(handles.ColorBox, 'Value')==0
    set(handles.ColorButton, 'Enable','on');
else
    set(handles.ColorButton, 'Enable','inactive');
end

% --- Executes on button press in ColorButton.
function ColorButton_Callback(hObject, eventdata, handles)
if get(handles.ColorButton, 'BackgroundColor')==[0 0 0]
    set(handles.ColorButton, 'BackgroundColor',[0 0 1]);
    return;
end
if get(handles.ColorButton, 'BackgroundColor')==[0 0 1]
    set(handles.ColorButton, 'BackgroundColor',[1 0 0]);
    return;
end
if get(handles.ColorButton, 'BackgroundColor')==[1 0 0]
    set(handles.ColorButton, 'BackgroundColor',[0 0.5 0]);
    return;
end
if get(handles.ColorButton, 'BackgroundColor')==[0 0.5 0]
    set(handles.ColorButton, 'BackgroundColor',[0 1 1]);
    return;
end
if get(handles.ColorButton, 'BackgroundColor')==[0 1 1]
    set(handles.ColorButton, 'BackgroundColor',[1 0 1]);
    return;
end
if get(handles.ColorButton, 'BackgroundColor')==[1 0 1]
    set(handles.ColorButton, 'BackgroundColor',[0.93 0.69 0.13]);
    return;
end
if get(handles.ColorButton, 'BackgroundColor')==[0.93 0.69 0.13]
    set(handles.ColorButton, 'BackgroundColor',[0 0 0]);
    return;
end

% --- Executes on button press in LoadMatButton.
function LoadMatButton_Callback(hObject, eventdata, handles)
toLoad=uigetfile({'.mat'}, 'Загрузка КУ');
if toLoad==0
    return
end
set(handles.SaveButton,'Enable', 'off');
load(toLoad, 'GainData');
figure(1);
plot(GainData.freqs,GainData.gain, 'Color', get(handles.ColorButton, 'BackgroundColor'));
if get(handles.ColorBox, 'Value')==1
    ColorButton_Callback(hObject, eventdata, handles);
end
grid on;
hold on;
xlabel('Частота, МГц', 'FontName', 'Arial');
ylabel('КУ, дБ',  'FontName', 'Arial');
title('Коэффициент усиления, дБ', 'FontName', 'Arial');

% --- Executes on button press in LoadConnectButton.
function LoadConnectButton_Callback(hObject, eventdata, handles)
toLoad=uigetfile({'.mat'}, 'Загрузка КУ','MultiSelect','on');
%если файлы не выбраны
if size(toLoad,2)==1
    return
end
q=0;
set(handles.SaveButton,'Enable', 'off');
%если выбран только один файл
TMP=cellstr(toLoad);
if size(TMP,2)==1
    load(toLoad,'GainData');
    q=1;
else
    %если выбрано несколько файлов
    if size(toLoad,2)>1
        %загрузка для предварительных вычислений
        fmax=reshape([],0,1);
        for i=1:size(toLoad,2)
            load(toLoad{i},'GainData');
            fmax=cat(1, fmax, GainData.freqs(size(GainData.freqs,2)));
        end
        
        %находим правильный порядок загрузки
        for i=1:size(fmax,1)
            N(size(fmax,1)-i+1)=find(fmax==max(fmax));
            fmax(N(size(fmax,1)-i+1))=0;
        end
        %находим пересекающиеся интервалы и заполняем их усредненными
        %значениями с применением интерполированными значениями с шагом по
        %частоте, наименьшим из двух графиков, накладывающихся друг на друга
        %в данном диапазоне
        load(toLoad{N(1)},'GainData');
        GainTMP.gain=GainData.gain;
        GainTMP.freqs=GainData.freqs;
        for i=2:size(toLoad,2)
            load(toLoad{N(i)},'GainData');
            gain2=GainData.gain;
            freqs2=GainData.freqs;
            if GainTMP.freqs(size(GainTMP.freqs,2))-GainTMP.freqs(size(GainTMP.freqs,2)-1)<=freqs2(size(freqs2,2))-freqs2(size(freqs2,2)-1);
                MID=(GainTMP.gain(find(GainTMP.freqs>=freqs2(1)))+interp1(freqs2,gain2,GainTMP.freqs(find(GainTMP.freqs>=freqs2(1)))))/2;
                GainTMP.gain=cat(2, GainTMP.gain(find(GainTMP.freqs<freqs2(1))),MID,gain2(find(freqs2>GainTMP.freqs(size(GainTMP.freqs,2)))));
                GainTMP.freqs=cat(2, GainTMP.freqs, freqs2(find(freqs2>GainTMP.freqs(size(GainTMP.freqs,2)))));
            else
                MID=(interp1(GainTMP.freqs,GainTMP.gain, freqs2(find(freqs2<=GainTMP.freqs(size(GainTMP.freqs,2)))))+gain2(find(freqs2<=GainTMP.freqs(size(GainTMP.freqs,2)))))/2;
                GainTMP.gain=cat(2, GainTMP.gain(find(GainTMP.freqs<freqs2(1))),MID,gain2(find(freqs2>GainTMP.freqs(size(GainTMP.freqs,2)))));
                GainTMP.freqs=cat(2, GainTMP.freqs(GainTMP.freqs<freqs2(1)), freqs2);
            end
        end
        GainData=GainTMP;
    end
end

%вывод общего графика
figure(1);
plot(GainData.freqs,GainData.gain, 'Color', get(handles.ColorButton, 'BackgroundColor'));
if get(handles.ColorBox, 'Value')==1
    ColorButton_Callback(hObject, eventdata, handles);
end
grid on;
hold on;
xlabel('Частота, МГц', 'FontName', 'Arial');
ylabel('КУ, дБ',  'FontName', 'Arial');
title('Коэффициент усиления, дБ', 'FontName', 'Arial');

%сохранение объединенного файла
if q==1
    helpdlg('Загружен только один файл','Внимание!');
    return;
end
toSave=uiputfile({'.mat'}, 'Сохранение объединенных данных');
if toSave==0
    return;
else
    save(toSave,'GainData');
    textOUT('Сохранено!',1,handles);
    textOUT('--------------------------------',1,handles);
end

% --- Executes on button press in LoadSettingsButton.
function LoadSettingsButton_Callback(hObject, eventdata, handles)
global but
if but==0
    if strcmp(ls('LastSettings.mat'),'LastSettings.mat')==1
        load('LastSettings.mat');
        set(handles.FreqStartEdit, 'String', freqStart);
        set(handles.FreqStepEdit, 'String', freqStep);
        set(handles.FreqStopEdit, 'String', freqStop);
        set(handles.AverEdit, 'String', averageFactor);
        set(handles.AntTypeMenu, 'Value', antType);
        set(handles.AnalyzerMenu,'Value',analyzerType);
        set(handles.DistToAntEdit,'String',distToAnt);
        set(handles.LoadSettingsButton,'String','Вернуть стандартные');
        but=1;
    else
        errordlg('Файл с настройками не обнаружен','Ошибка');
    end
else
    set(handles.FreqStartEdit, 'String', '30');
    set(handles.FreqStepEdit, 'String', '10');
    set(handles.FreqStopEdit, 'String', '300');
    set(handles.AverEdit, 'String', '30');
    set(handles.AntTypeMenu, 'Value', 1);
    set(handles.AnalyzerMenu,'Value',1);
    set(handles.DistToAntEdit,'String','3.5');
    set(handles.LoadSettingsButton,'String','Загрузить настройки');
    but=0;
end


% --- Executes on button press in AngleDownButton.
function AngleDownButton_Callback(hObject, eventdata, handles)
global opu
global opuFlag
if str2num(get(handles.AngleEdit, 'String'))>0
    if str2num(get(handles.AngleEdit,'String'))<=5
        set(handles.AngleEdit, 'String','0');
    else
        set(handles.AngleEdit, 'String',num2str(str2num(get(handles.AngleEdit,'String'))-5));
    end
    if opuFlag==0
        opuFlag==1;
        opu=InitOPU();
    end
    SetCorner(str2num(get(handles.AngleEdit,'String')), opu);
end


% --- Executes on button press in AngleUpButton.
function AngleUpButton_Callback(hObject, eventdata, handles)
global opu
global opuFlag
if str2num(get(handles.AngleEdit, 'String'))<360
    if str2num(get(handles.AngleEdit,'String'))>=355
        set(handles.AngleEdit, 'String','360');
    else
        set(handles.AngleEdit, 'String',num2str(str2num(get(handles.AngleEdit,'String'))+5));
    end
    if opuFlag==0
        opuFlag==1;
        opu=InitOPU();
    end
    SetCorner(str2num(get(handles.AngleEdit,'String')), opu);
end


% --- Executes on button press in AngleSetButton.
function AngleSetButton_Callback(hObject, eventdata, handles)
global opu
global opuFlag
if str2num(get(handles.AngleEdit,'String'))>360
    set(handles.AngleEdit,'String','360');
end
if str2num(get(handles.AngleEdit,'String'))<0
    set(handles.AngleEdit,'String','0');
end
if opuFlag==0
    opuFlag==1;
    opu=InitOPU();
end
SetCorner(str2num(get(handles.AngleEdit,'String')), opu);
pause(0.5)


function AngleReadBox_Callback(hObject, eventdata, handles)
global opu
global exitFlag
global opuFlag
if opuFlag==0
    opu=InitOPU();
    opuFlag=1;
end
if get(handles.AngleReadBox, 'Value')==0
    set(handles.CurrentAngleText, 'ForeGroundColor', 'red');
else
    set(handles.CurrentAngleText, 'ForeGroundColor', 'black');
end
while get(handles.AngleReadBox, 'Value')==1
    corner=ReadCorner(opu);
    pause(0.2);
    if exitFlag>0
        return;
    end
    set(handles.CurrentAngleText, 'String', corner);
end


function [opu] = InitOPU() %инициализация ОПУ
opu = tcpip('192.168.10.4',502);
set(opu,'Timeout',0.1);
fopen(opu);

function [instr] = InitINSTR(analyzerType, COM, handles) %инициализация измерительного прибора

switch analyzerType
    case 'ZVA 40'
        %         инициализация
        instr = visa('ni', 'TCPIP::192.168.10.3');
        set(instr,'InputBufferSize',60000);
        fopen(instr);
        textOUT('Подключен к ZVA 40',1,handles);
    case 'Anritsu MS2028C'
        %         инициализация
        instr = visa('ni', 'TCPIP::192.168.10.3');
        set(instr,'InputBufferSize',60000);
        fopen(instr);
        textOUT('Подключен к Anritsu MS2028C',1,handles);
    case 'FSH 6'
        %         инициализация
        textOUT('Подключен к FSH 6',1,handles);
end

function [KP]=MEAS(inputData,COM,handles)
switch get(handles.AnalyzerMenu,'Value')
    case 1
        textOUT('Подключение к ZVA 40..',1,handles);
        instr=InitINSTR(inputData.analyzerType, COM,handles);
        textOUT('Соединение установлено',1,handles)
        %измерение с помощью ZVA40
        fprintf(instr,'INIT:CONT OFF');
        fprintf(instr,['SWEep:POINts ' num2str(size(str2num(get(handles.FreqStartEdit,'String')):str2num(get(handles.FreqStepEdit,'String')):str2num(get(handles.FreqStopEdit,'String')),2))]);
        fprintf(instr,['FREQ:STARt ' num2str(str2num(get(handles.FreqStartEdit,'String'))*1e6)]);
        fprintf(instr,['FREQ:STOP ' num2str(str2num(get(handles.FreqStopEdit,'String'))*1e6)]);
        fprintf(instr,'AVERage ON');
        fprintf(instr,['AVERage:COUNt ' num2str(get(handles.AverEdit,'String'))]);
        fprintf(instr,['SWEep:COUNt ' num2str(get(handles.AverEdit,'String'))]);
        fprintf(instr,['AVERage:CLEar']);
        fprintf(instr,'INIT:CONT OFF; :INIT;');
        fprintf(instr,'if (CALC:DATA:NSW:COUN? > 99) CALC:DATA:NSW:FIRS?SDAT, 100');
        fprintf(instr,['if (CALC:DATA:NSW:COUN? > 99) INIT:CONT ON']);
        fprintf(instr,'INIT');
        fprintf(instr,'*OPC?');
        fprintf(instr,'CALCulate1:DATA:ALL? SDATa')
        instrData=str2num(fscanf(instr));
        KP=mag2dB(abs(instrData(1:2:end)+1i*instrData(2:2:end)));
        fprintf(instr,'INIT:CONT ON');
        fclose(instr);
    case 2
        textOUT('Подключение к Anritsu MS2028C..',1,handles);
        instr=InitINSTR(inputData.analyzerType, COM,handles);
        textOUT('Соединение установлено',1,handles);
        %измерение с помощью Anritsu
        fprintf(instr,['SWEep:POINts ' num2str(size(str2num(get(handles.FreqStartEdit,'String')):str2num(get(handles.FreqStepEdit,'String')):str2num(get(handles.FreqStopEdit,'String')),2))]);
        fprintf(instr,['FREQ:STARt ' num2str(str2num(get(handles.FreqStartEdit,'String'))*1e6)]);
        fprintf(instr,['FREQ:STOP ' num2str(str2num(get(handles.FreqStopEdit,'String'))*1e6)]);
        fprintf(instr,'AVERage ON');
        fprintf(instr,['AVERage:COUNt ' num2str(get(handles.AverEdit,'String'))]);
        fprintf(instr,['SWEep:COUNt ' num2str(get(handles.AverEdit,'String'))]);
        fprintf(instr,'AVERage:CLEar');
        fprintf(instr,'INIT');
        fprintf(instr,':STATus:OPERation?');
        ready=fscanf(instr);
        while(ready~='256')
            fprintf(instr,':STATus:OPERation?');
            ready=fscanf(instr);
        end
        fprintf(instr, 'FORMat:DATA ASCii');
        fprintf(instr,'TRAC?');
        andData=fscanf(instr);
        andData=andData(9:end);
        dataArray=str2num(andData);
        KP=mag2dB(abs(dataArray(1:2:end)+1i*dataArray(2:2:end)))
        fprintf(instr,'INIT:CONT ON');
        fclose(instr);
    case 3
        textOUT('Подключение к FSH 6..',1,handles);
        errordlg('Если Вы нашли кабель для  FSH6, пожалуйста принесите его в комнату 422, а я допишу для него программу','Ошибка!');
        KP=0;
end

function angleInt = SetCorner(Angle,t)
Angle=Angle-180;
NumRegisters=9;
i=0;
run=0;
errorReceiveFlag=0;
%ограничение углов поворота
if Angle>185
    Angle=185;
    warning('Wrong angle request');
elseif Angle<-185
    Angle= -185;
    warning('Wrong angle request');
end

angToSend=Angle*60;
%проверка знака
if angToSend<0
    angToSend=angToSend+intmax('uint32')+1;
end
%формирование байтов отвечающих за угол поворота
ang1= bitshift(angToSend,-24,8);
ang2= bitshift(angToSend,-16,8);
ang3= bitshift(angToSend,-8,8);
ang4= bitshift(angToSend,0,8);
%команда на отправку
str=[char(0) char(0) char(0) char(0) char(0)...
    char(11+2*NumRegisters) char(255) char(23) char(0)...
    char(4) char(0) char(NumRegisters) char(0)...
    char(4) char(0) char(NumRegisters) char(NumRegisters*2)...
    char(0) char(0) char(0) char(0)...
    char(33) char(6) char(1) char(104)... //19-20 скорость
    char(2) char(208) char(255) char(255)...//21-22 ускорение
    char(ang1) char(ang2) char(ang3) char(ang4)...//25-28 угол
    char(0) char(0)];

while(run<1 || (bitshift(uint8(readData(15)),-3,1)==0) || abs(angleInt-Angle)>=0.05)
    run=1;
    fwrite(t,str);
    %на получение 27 байтов дается 1000 циклов, иначе выдаст предупреждение
    while(get(t,'BytesAvailable')<27 && ~errorReceiveFlag)
        i=i+1;
        pause(0.02);%!!!
        if i>250
            errorReceiveFlag=1;
        end
    end
    i=0;
    %если все байты так и не были получены, вывод предупреждения и выход
    if errorReceiveFlag
        angleInt = NaN;
        errordlg({'Ошибка получения данных ОПУ.'; 'Устройство будет перезапущено'},'Ошибка');
        fclose(instrfind);
        pause(0.2);
        fopen(t);
        return
    end
    %из принятых байтов получаем информацию о текущем угле
    readData=fread(t, 27);
    angleReceived=readData(22:25);
    angleInt=double(bitshift(angleReceived(1),24)+bitshift(angleReceived(2),16)...
        +bitshift(angleReceived(3),8)+angleReceived(4));
    %проверка знака
    if angleInt>bitshift(intmax('uint32'),-1)-1;
        angleInt=angleInt-double(intmax('uint32'));
    end
    angleInt=angleInt/60;
end
angleInt=180+angleInt;

function angleInt = ReadCorner(t)
NumRegisters=9;
i=0;
errorReceiveFlag=0;

%команда на отправку
str=[char(0) char(0) char(0) char(0) char(0)...
    char(6) char(255) char(3) char(0)...
    char(4) char(0) char(NumRegisters)];

fwrite(t,str);
%на получение 27 байтов дается 1000 циклов, иначе выдаст предупреждение
while(get(t,'BytesAvailable')<27 && ~errorReceiveFlag)
    i=i+1;
    pause(0.02);%!!!
    if i>250
        errorReceiveFlag=1;
    end
end
i=0;
%если все байты так и не были получены, вывод предупреждения и выход
if errorReceiveFlag
    angleInt = NaN;
    errordlg({'Ошибка получения данных ОПУ.'; 'Устройство будет перезапущено'},'Ошибка');
    fclose(instrfind);
    pause(0.2);
    fopen(t);
    return
end
%из принятых байтов получаем информацию о текущем угле
readData=fread(t, 27);
angleReceived=readData(22:25);
angleInt=double(bitshift(angleReceived(1),24)+bitshift(angleReceived(2),16)...
    +bitshift(angleReceived(3),8)+angleReceived(4));
%проверка знака
if angleInt>bitshift(intmax('uint32'),-1)-1;
    angleInt=angleInt-double(intmax('uint32'));
end
angleInt=angleInt/60;
angleInt=180+angleInt;

% --- Executes when user attempts to close figure1.
function figure1_CloseRequestFcn(hObject, eventdata, handles)
global opu
global opuFlag
global exitFlag
if opuFlag==1
    fclose(opu);
end
exitFlag=1;
delete(hObject);
