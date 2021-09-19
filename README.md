Setup:
1) Open the solution in Visual Studio.
2) Go to Tools>Nuget Package Manager>Package Manager Console
3) In the console at the bottom of the screen type--> update-database
4) In the command prompt enter--> pip install pyodbc
5) Run insertData.py
6) Click IIS Express in Visual Studio to run the site

Notes:
The registration form will not create an account for you unless the customerID and email matches what is in the database. Check the data when registering a new user.
To see the database in SQL Server, connect to (localdb)\MSSqlLocalDb. If you want to undo a migration, delete the database in SQL Server, then in the Package Manager Console type remove-migration.

Addendum:
You are enough.