# DataGate Report Processor

.Net Core console application running on Ubuntu 20.04 SFTP Linux Server. </br>
This tool was created to manage the reporting part of the [DataGate Web Platform](https://github.com/PhilShishov/DataGate) </br>
The main function is automatic data feed of internal database from Fund Admin (Asset under management, Subscription, Redeption, Net asset value)
 1. Receive input from client reports - .csv, .xlsx
 2. Manipulate data through SFTP Server
 3. Send processed data to DataGate DB to be displayed in web platform 

## Technologies
* C# .NET Core 3.1
* SQL Server, Ubuntu 20.04
* XML, NLog
* CsvHelper, ToDataTable
