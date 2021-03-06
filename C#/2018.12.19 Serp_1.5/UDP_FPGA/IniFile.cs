using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

namespace UDP_FPGA
{
    class IniFile
    {
        string Path; //Имя файла.

        [DllImport("kernel32")] // Подключаем kernel32.dll и описываем его функцию WritePrivateProfilesString
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")] // Еще раз подключаем kernel32.dll, а теперь описываем функцию GetPrivateProfileString
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        // С помощью конструктора записываем пусть до файла и его имя.
        public IniFile(string IniPath)
        {
            Path = new FileInfo(IniPath).FullName.ToString();
        }

        //Читаем ini-файл и возвращаем значение указного ключа из заданной секции.
        public string ReadINI(string Section, string Key)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        //Записываем в ini-файл. Запись происходит в выбранную секцию в выбранный ключ.
        public void WriteINI(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, Path);
        }

        //Удаляем ключ из выбранной секции.
        public void DeleteKeyINI(string Section, string Key)
        {
            WriteINI(Section, Key, null);
        }

        //Удаляем выбранную секцию
        public void DeleteSectionINI(string Section = null)
        {
            WriteINI(Section, null, null);
        }

        //Проверяем, есть ли такой ключ, в этой секции
        public bool KeyExistsINI(string Section, string Key)
        {
            return ReadINI(Section, Key).Length > 0;
        }
    }


   

}
