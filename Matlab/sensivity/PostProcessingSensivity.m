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
set(handles.figure1,'position',[40 40 82 48]);  %��������� ���� �� ������
set(handles.figure1,'Name', '������ ����������������');  %��������� ����

% --- Outputs from this function are returned to the command line.
function varargout = PostProcessingSensivity_OutputFcn(hObject, eventdata, handles) 
varargout{1} = handles.output;

% --- Executes on button press in AddExpDataButton.
function AddExpDataButton_Callback(hObject, eventdata, handles)
% set(handles.ResetExpDataButton, 'Enable', 'off')
global ExpDataFiles;
ExpDataFiles=uigetfile({'.mat'}, '�������� ����������� �� ��������� ��������');
if ExpDataFiles==0
    if strcmp(get(handles.AddedExpFilesText, 'String'), '������ �� ���������')==0
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
NoiseDataFile=uigetfile({'.mat'}, '�������� ������� �������');
if NoiseDataFile==0
    if strcmp(get(handles.AddedNoiseFilesText, 'String'), '������ �� ���������')==0
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
    errordlg('�� ������ ���� � ���������� ��','������');
    return;
end 
if NoiseDataFile==0
    errordlg('�� ������ ���� � ������� ��������','������') ;  
    return;
end
load(NoiseDataFile);
load(ExpDataFiles);
SensivityData.freqsExp = max(min(NoiseData.freqs), min(GainData.freqs)):1:min(max(NoiseData.freqs), max(GainData.freqs));
SensivityData.gainExp = interp1(GainData.freqs, GainData.gain, SensivityData.freqsExp,'linear');
SensivityData.noiseExp = interp1(NoiseData.freqs, NoiseData.noise, SensivityData.freqsExp,'linear');
%������
SensivityData.sensivityExp=((10^6)*SensivityData.freqsExp.*sqrt(1e3*str2num(get(handles.BandEdit, 'String')))*(9.73*sqrt(2)/600).*10.^((SensivityData.noiseExp-SensivityData.gainExp)/20))/sqrt(str2num(get(handles.InputBandEdit,'String')))
axes(handles.axes1);
plot(SensivityData.freqsExp,SensivityData.sensivityExp, 'Color', get(handles.ColorButton, 'BackgroundColor'));
xlabel('�������, ���', 'FontName', 'Arial');
ylabel('����������������, ���/�',  'FontName', 'Arial');
title(['���������������� � ������ ' get(handles.BandEdit, 'String') ' ���'], 'FontName', 'Arial');
grid on;
hold on;


function ResetExpDataButton_Callback(hObject, eventdata, handles)
global ExpDataFiles;
ExpDataFiles=0;
global NoiseDataFile;
NoiseDataFile=0;
set(handles.AddedExpFilesText, 'String', '������ �� ���������');
set(handles.AddedNoiseFilesText, 'String', '������ �� ���������');
set(handles.ResetExpDataButton, 'Enable', 'off');

% --- Executes on button press in AddPDFDataButton.
function AddPDFDataButton_Callback(hObject, eventdata, handles)
global PDFDataFile
PDFDataFile=uigetfile({'.mat'}, '�������� ������ ���������');
if PDFDataFile==0
    if strcmp(get(handles.AddedPDFFilesText, 'String'), '������ �� ���������')==0
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
THDataFile=uigetfile({'.mat'}, '�������� ������ ������������� ��������� ��������');
if  THDataFile==0
if strcmp(get(handles.AddedTHFilesText, 'String'), '������ �� ���������')==0
        THDataFile=get(handles.AddedTHFilesText, 'String');
    end
    return;
else
set(handles.AddedTHFilesText, 'String', THDataFile);
set(handles.ResetTHDataButton, 'Enable', 'on');
end


% --- Executes on button press in CountTHButton.
function CountTHButton_Callback(hObject, eventdata, handles)
%%   ������
global THDataFile;
global PDFDataFile;
global SensivityData;

if PDFDataFile==0;
    errordlg('�� ������ ���� � ���������������� ���������','������');
    return;
end 
if THDataFile==0;
    errordlg('�� ������ ���� � ���������� ������� �������','������') ;  
    return;
end

load(THDataFile);
load(PDFDataFile);
% ������� ��� �������������� �������
SensivityData.freqsTH=max(min(PDFData.freqs),min(THData.freqs)):1:min(max(PDFData.freqs),max(THData.freqs));
% ��������� ��������� �� 
SensivityData.gainTH=interp1(THData.freqs,THData.gain,SensivityData.freqsTH,'linear'); 
% ��������� ��������� �� ��������
SensivityData.noisePDF=interp1(PDFData.freqs,PDFData.noise,SensivityData.freqsTH,'linear');
% ������ ���������������� [���/�]
SensivityData.sensivityTH=0.00145*SensivityData.freqsTH.*sqrt(str2num(get(handles.BandEdit, 'String'))).*10.^((SensivityData.noisePDF-SensivityData.gainTH)/20);
% ����� �������
axes(handles.axes1);
plot(SensivityData.freqsTH,SensivityData.sensivityTH, 'Color', get(handles.ColorButton, 'BackgroundColor'),'LineStyle', '--');
xlabel('�������, ���', 'FontName', 'Arial');
ylabel('����������������, ���/�',  'FontName', 'Arial');
title(['���������������� � ������ ' get(handles.BandEdit, 'String') ' ���'], 'FontName', 'Arial');
grid on;
hold on;



% --- Executes on button press in ResetTHDataButton.
function ResetTHDataButton_Callback(hObject, eventdata, handles)
global THDataFiles;
global PDFDataFiles;
THDataFiles=0;
PDFDataFiles=0;
set(handles.AddedTHFilesText, 'String', '������ �� ���������');
set(handles.AddedPDFFilesText, 'String', '������ �� ���������');
set(handles.ResetTHDataButton, 'Enable', 'off');


function BandEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function BandEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'));
    set(hObject,'BackgroundColor','white');
end

% --- Executes on button press in FAQButton.
function FAQButton_Callback(hObject, eventdata, handles)
helpdlg({'��������� ����������� ������: ��������� ������������';...
    '���������������� �������� ���������� ������ � ������� ������������.';...
    '������� ������ ������ � ����������� ������.'; ...
    '��� ��������� ����������� � �������������� �������,';...
    '�������� �� ��������� � ��������� �� ��������� ��������,';...
    '������� ������ ������, ����������� ������ � ��������� ���������';...
    '������ ������ ��� �����:';
    '����������� �������� (����������): GainData.gain, GainData.freqs';
    '������� �������: NoiseData.freqs,NoiseData.noise';
    '������ ���������: PDFData.gain, PDFData.noise, PDFData.freqs';
    '����������� �������� (�������������): THData.gain, THData.freqs';
    },'�������'); 

% --- Executes on button press in SaveDataButton.
function SaveDataButton_Callback(hObject, eventdata, handles)
global SensivityData
toSave=uiputfile({'.mat'}, '���������� ������');
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
set(handles.AddedTHFilesText, 'String', '������ �� ���������');
set(handles.AddedPDFFilesText, 'String', '������ �� ���������');
set(handles.ResetTHDataButton, 'Enable', 'off');
set(handles.AddedExpFilesText, 'String', '������ �� ���������');
set(handles.AddedNoiseFilesText, 'String', '������ �� ���������');
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
    xlabel('�������, ���', 'FontName', 'Arial');
    ylabel('����������������, ���/�',  'FontName', 'Arial');
    title(['���������������� � ������ ' get(handles.BandEdit, 'String') ' ���'], 'FontName', 'Arial');

    
% --- Executes on button press in SaveGraphButton.
function SaveGraphButton_Callback(hObject, eventdata, handles)
% axes(handles.axes1)
toSave=uiputfile({'.fig'}, '���������� �������');
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
    xlabel('�������, ���', 'FontName', 'Arial');
    ylabel('����������������, ���/�',  'FontName', 'Arial');
    title(['���������������� � ������ ' get(handles.BandEdit, 'String') ' ���'], 'FontName', 'Arial');
    saveas(SaveFigure, toSave);
    close;

end
    
    
% --- Executes on button press in LoadMatButton.
function LoadMatButton_Callback(hObject, eventdata, handles)
global SensivityData
toLoad=uigetfile({'.mat'}, '�������� ����������������')
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
    xlabel('�������, ���', 'FontName', 'Arial');
    ylabel('����������������, ���/�',  'FontName', 'Arial');
    title(['���������������� � ������ ' get(handles.BandEdit, 'String') ' ���'], 'FontName', 'Arial');
end
if isfield(SensivityData,'gainTH')
    figure(1)
    plot(SensivityData.freqsTH,SensivityData.sensivityTH, 'Color', get(handles.ColorButton, 'BackgroundColor'))
    grid on
    hold on
    xlabel('�������, ���', 'FontName', 'Arial');
    ylabel('����������������, ���/�',  'FontName', 'Arial');
    title(['���������������� � ������ ' get(handles.BandEdit, 'String') ' ���'], 'FontName', 'Arial');
end
clear SensivityData;
