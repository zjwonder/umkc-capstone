# Corner Club Capstone Project

## Welcome!

We've been waiting for you! Here, have a cup of tea.

<img src="https://media0.giphy.com/media/uS9epqlhBNQOI/giphy.gif" width="300px">


In this repository, you will find the application that our group, the Corner Club, is building for our capstone class.

Who is the Corner Club, you ask? We are a team of computer science students, working together to create a better tomorrow! ... or at least a nice web app for Commerce Bank. Members include:

- Wes McNall
- Zach Halderwood
- Ryan Williams
- Colin McNish
- Ian Chacey
- Jayson Nguyen

## Resources

[Classroom Github Repository](https://github.com/UMKC-CS451R/cs415r_f21_groupproject-corner-club)
- Contains file submissions for each iteration of the project

[Project Trello Board](https://trello.com/b/0DNIyTPA/corner-club)
- Where we break down and track the tasks involved in building this project

## Developer Tips

Setup:
1) Open the solution in Visual Studio.
2) Go to Tools>Nuget Package Manager>Package Manager Console
3) In the console at the bottom of the screen type--> `update-database`
4) In the command prompt enter--> `pip install pyodbc`
5) Run insertData.py
6) Click IIS Express in Visual Studio to run the site

To redo the migration:
1) In Visual Studio: View > SQL Server Object Explorer
2) Open up Databases and right click the DB and choose delete.
3) Tools > Nuget Package Manager > Package Manager Console
4) In the console enter `update-database`
5) Run insertData.py

Notes:
The registration form will not create an account for you unless the customerID and email matches what is in the database. Check the data when registering a new user.
To see the database in SQL Server, connect to (localdb)\MSSqlLocalDb. If you want to undo a migration, delete the database in SQL Server, then in the Package Manager Console type remove-migration.

Addendum:
You are enough.
