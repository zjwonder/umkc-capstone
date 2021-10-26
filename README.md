# Corner Club Capstone Project

See the product in action by visiting our website: [www.corner-club.com](https://www.corner-club.com)

## Welcome!

We've been waiting for you! Here, have a cup of tea.

<img src="https://media0.giphy.com/media/uS9epqlhBNQOI/giphy.gif" width="300px">


In this repository, you will find the application that our group, the Corner Club, is building for our capstone class.

Who is the Corner Club, you ask? We are a team of computer science students, working together to create a better tomorrow! ... or at least a nice web app for Commerce Bank. Members include:

- Wes McNall, Project Manager
- Zach Halderwood, Architect / PO
- Ryan Williams, Senior Engineer
- Colin McNish, Full-Stack Engineer
- Ian Chacey, Full-Stack Engineer
- Jayson Nguyen, Full-Stack Engineer

## About the Project

This project is a fully-functional, professional-quality web app which simulates an online banking tool. It is being created with a bit of guidance from some wonderful people at Commerce Bank and the academic big wigs in the School of Computing & Engineering at UMKC.

### Team Meetings

Our full team meets once/week over Discord and Zoom, where we discuss various class assignments, have kanban-style standups, review the timeline for our project, and collaborate on student-y things in general. Wes, Zach, and Ryan chat regularly to maintain a cohesive plan for prioritizing and planning work for the team and make sure project documentation is up-to-spec and submitted to the professor when she needs it. Various members of the team get together to pair-program on an as-needed basis so that our tasks always are completed on-time and with excellent quality.

### Tech Stack

We followed the sponsoring company's recommendations for database and API frameworks. We did stray a bit from the beaten path on some of our tools, though.

- Database:         Microsoft SQL
- API:              ASP.NET Core 3.1
- Front end:        Razor Pages (CSHTML)

### Additional Tools

In this modern era, it would be silly of us to not consume some additional tools in order to ease the development and feature-building of our product.

- Version control:  Github Pro
- Cloud deployment: MS Azure
- Authentication:   MS Identity Framework
- Notifications:    Sendgrid API

### CI/CD

Our team has implemented Continuous Integration and Continuous Deployment. By configuring the Github repo to automatically deploy changes on the main branch to the Azure-hosted app service, we can easily and quickly patch bugs and add features with minimal risk. We protect the main branch by requiring approved pull requests in order to merge changes to main. The only users with access to pull, commit, and approve changes are members of this team and our professor.

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
