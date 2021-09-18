function varargout = MeasureNoise(varargin)
% MEASURENOISE MATLAB code for MeasureNoise.fig
% Begin initialization code - DO NOT EDIT
gui_Singleton = 1;
gui_State = struct('gui_Name',       mfilename, ...
                   'gui_Singleton',  gui_Singleton, ...
                   'gui_OpeningFcn', @MeasureNoise_OpeningFcn, ...
                   'gui_OutputFcn',  @MeasureNoise_OutputFcn, ...
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

% --- Executes just before MeasureNoise is made visible.
function MeasureNoise_OpeningFcn(hObject, eventdata, handles, varargin)
handles.output = hObject;
guidata(hObject, handles);
set(handles.figure1,'position',[40 40 72 43]);  %��������� ���� �� ������
set(handles.figure1,'Name', '��������� ����');  %��������� ����

% --- Outputs from this function are returned to the command line.
function varargout = MeasureNoise_OutputFcn(hObject, eventdata, handles) 
varargout{1} = handles.output;

function Reset(instr, span, points, averageSamples, RBW, VBW, preAmp, att)   %��������� ����������
fprintf(instr,'*RST');
fprintf(instr,'INIT:CONT OFF');
fprintf(instr, span); %span
fprintf(instr, points); %����� �����
fprintf(instr,'DISP:TRAC1:MODE AVER'); 
fprintf(instr, averageSamples); %����������
fprintf(instr, 'INP:ATT:AUTO OFF');
fprintf(instr, ['INP:ATT ' att 'dB']); %����������
fprintf(instr, 'DISP:TRAC1:Y:RLEV -50 dBm'); %���� ���� ����� �� ������
fprintf(instr,['INP:GAIN:STAT ' preAmp]); %���/���� �������������
fprintf(instr,'BAND:AUTO OFF');
fprintf(instr, RBW); %RBW
fprintf(instr,'BAND:VID:AUTO OFF');
fprintf(instr, VBW); %VBW
fprintf(instr,'CALC1:MARK1 ON');
fprintf(instr,'CALC1:MARK1:FUNC:NOIS ON');  %��������� ��������� ����
fprintf(instr,'CALC1:MARK1:MIN:AUTO ON'); %����� ��������

function noise = GetNoise(instr,freqs) %���������� ������ ���������
fprintf(instr,['FREQ:CENT ' num2str(freqs*1e6)]);
fprintf(instr,'INIT;*WAI');
fprintf(instr,'CALC1:MARK1:FUNC:NOIS:RES?');
noise=str2double(fscanf(instr));

%������������ ������ ������ � ����������
function textOUT(textIN, type, handles)
text0=get(handles.StatusText, 'String');
switch type
    case 1 %��������� �������
        if size(text0,1)>3 %���������� ����� ����� � ���� StatusText
            for i=2:size(text0,1)
                text0(i-1,1)=text0(i,1);
            end
            text0{size(text0,1),1}=textIN;
        else
            text0=[text0;textIN];
        end
    case 2 %������ ��������� �������
        text0{size(text0,1),1}=textIN;
end
set(handles.StatusText, 'String', text0);

% --- Executes on button press in StartButton.
function StartButton_Callback(hObject, eventdata, handles)   %������ ������ ���������
global stopMeas;
stopMeas=false;
set(handles.StartButton, 'Enable', 'off');
set(handles.StopButton, 'Visible', 'on');
set(handles.ExitButton, 'Visible', 'off');
freqStart=str2num(get(handles.FreqStartEdit,'String'));
freqStep=str2num(get(handles.FreqStepEdit,'String'));
freqStop=str2num(get(handles.FreqStopEdit,'String'));
noiseFreqs=freqStart:freqStep:freqStop;
noiseDate=date;
span=['FREQ:SPAN ' get(handles.SpanEdit,'String') 'MHz'];
points=['SENS:SWE:POIN ' get(handles.NumberOfPointsEdit, 'String')];
averageSamples=['AVERage:COUN ' get(handles.AverageSamplesEdit,'String')];
RBW=['BAND ' get(handles.RBWEdit,'String') ' KHZ'];
VBW=['BAND:VID ' get(handles.RBWEdit,'String') ' KHZ'] ;   %���������� ��������
if get(handles.PreAmpBox,'Value')==1
    preAmp='ON';
else
    preAmp='OFF';
end
att=get(handles.AttEdit,'String')

textOUT('����������� � �����������', 1, handles);
instr = visa('ni', 'TCPIP::192.168.10.77','Timeout',20);  
set(instr,'InputBufferSize',60000);
fopen(instr); %����������� � �����������
textOUT('����������� � �����������...���������� �����������', 2, handles);

textOUT('�������� ��������', 1, handles);
Reset(instr, span, points, averageSamples, RBW, VBW, preAmp, att); %������������ ������������ ����������� � ��������� ���������� ���������
textOUT('�������� ��������...��������� �����������', 2, handles);

textOUT('���� ���������. ��������� 0%',1,handles);
for i=1:size(noiseFreqs,2)
    drawnow;  %��������� �������� GUI
    if stopMeas == true; %������
        stopMeas = false;
        textOUT('��������',1,handles);
        fclose(instr);
        return;
    end
    noise(i) = GetNoise(instr,noiseFreqs(i)); %���������� ������ ���������
    axes(handles.NoiseAxes);
    plot(noiseFreqs(1:i),noise(1:i))  %���������� �������
    xlim([freqStart freqStop]);
    xlabel('�������,���')
    ylabel('���, ���/��')
    grid on;
    textOUT(['���� ���������. ��������� ', num2str(fix(i/size(noiseFreqs,2)*100)),'%'],2,handles);
end
fclose(instr); %���������� �� �����������
textOUT('��������� ���������!', 1, handles);  
set(handles.StartButton, 'Enable', 'on');
set(handles.ExitButton, 'Visible', 'on');
set(handles.StopButton, 'Visible', 'off');
toSave = uiputfile;
NoiseData.Date=noiseDate;
NoiseData.freqs=noiseFreqs;
NoiseData.noise=noise;
if toSave ~= 0
    save (toSave,'NoiseData');
end

% --- Executes on button press in StopButton.
function StopButton_Callback(hObject, eventdata, handles)    %������ ������
Ack = questdlg('���������� ���������?', '�������������', '��','���','���');
if strcmp(Ack,'��')==1
global stopMeas 
stopMeas = true;
set(handles.StartButton, 'Enable', 'on');
set(handles.ExitButton, 'Visible', 'on');
set(handles.StopButton, 'Visible', 'off');
set(handles.SensMeasButton, 'Enable', 'off');
end

% --- Executes on button press in ExitButton.
function ExitButton_Callback(hObject, eventdata, handles)    % ������ �����
global stopMeas;
stopMeas=true;
Ack = questdlg('��� �� ������ �������?', '�������������', '������ ����������������','�����','�����');
if strcmp(Ack,'������ ����������������')==1
    close;
    PostProcessingSensivity;
else
    close;
end

function RBWEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function RBWEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function VBWEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function VBWEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function RefLevelEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function RefLevelEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function NumberOfPointsEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function NumberOfPointsEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function AverageSamplesEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function AverageSamplesEdit_CreateFcn(hObject, eventdata, handles)
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

function SpanEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function SpanEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

% --- Executes on button press in PreAmpBox.
function PreAmpBox_Callback(hObject, eventdata, handles)


function AttEdit_Callback(hObject, eventdata, handles)

% --- Executes during object creation, after setting all properties.
function AttEdit_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end
