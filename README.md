# Emails service

This service send emails seted in database and deleted it when sended or reached numbers of trials to expired 

To run: 

1. Create file appsettings.json in project main folder like (NrOfTrialsToExpired is for deleted not delivered mail after x number of try):

  {
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      },
      "EventLog": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft": "Warning"
        }
      }
    },
    "EmailSettings": {
      "UserName": "",
      "Password": "",
      "Host": "smtp.gmail.com",
      "Port": "465",
      "NrOfTrialsToExpired":  3
    },
    "ConnectionStrings": {
      "DefaultConnection": "Data Source=RAKIETA\\SQLEXPRESS;Initial Catalog=EmailsServiceDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
    }
  }

2. Change ConnectionStrings to yours database (Database will be created at first startup if you don't created it first). If you want to create db first use script from SQL scripts folder. 
    If you use SQL server, need to add system permision(in user mapping) to masterdb and make them server role to sysadmin

3. Add to windows services like sc.exe create "emailservice" binpath="G:\Programowanie\Email service\Service\EmailService.exe"


To build:
create file appsettings.json like above


To run tests:
create file appsettings.Development.json in project main folder like:

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "EmailProviderSettings": {
    "UserName": "",
    "Password": "",
    "Host": "smtp.gmail.com",
    "Port": "465"
  }
}
