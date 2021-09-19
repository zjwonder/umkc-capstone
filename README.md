# Welcome to the Corner Club

Good vibes are implicit. Good code is essential.

### How to configure the project for local testing:
1) Open the solution in Visual Studio.
2) Go to Tools>Nuget Package Manager>Package Manager Console
3) In the console at the bottom of the screen type--> update-database
4) In the command prompt enter--> pip install pyodbc
5) Run insertData.py
6) Click IIS Express in Visual Studio to run the site

### How to configure MySQL Workbench with Azure:

Microsoft has [already made a guide for this](https://docs.microsoft.com/en-us/azure/mysql/concepts-migrate-import-export). Below are the credentials you'll need to connect to the cloud server.

- Server: corner-club-db.mysql.database.azure.com
- Username/password: [visit Corner Club on Discord](https://discord.com/channels/879727306254463006/879727306254463009/889228310838648872)

*Note:
The registration form will not create an account for you unless the customerID and email matches what is in the database. Check the data when registering a new user.
To see the database in SQL Server, connect to (localdb)\MSSqlLocalDb. If you want to undo a migration, delete the database in SQL Server, then in the Package Manager Console type remove-migration.*


**You are enough.**
