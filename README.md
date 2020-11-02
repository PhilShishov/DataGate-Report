# DataGate Report Processor

.NET Core console application running on Ubuntu 20.04 SFTP Linux Server. </br>
This tool was created to manage the reporting part of the [DataGate Web Platform](https://github.com/PhilShishov/DataGate) </br>
The main function is automatic feeding of data into an internal database by the Fund Admin (Asset under management, Subscription, Redeption, Net asset value)
 1. Receive input from client reports - .csv, .xlsx 
 ![InputFiles](Documentation/SFTP/InputFiles.png)
 
 2. Manipulate data through SFTP Server - bash script, cron job
 ![BashScript](Documentation/SFTP/BashScript.png)
 ![CronJob](Documentation/SFTP/CronJob.png)
 
 3. Send processed data to DataGate DB to be displayed in web platform
![LogResult](Documentation/SFTP/LogResult.png)

## Technologies
* C# .NET Core 3.1
* SQL Server, Ubuntu 20.04
* XML, NLog
* CsvHelper, ToDataTable
