 Device System

This project was created using VSCode and dotnet 6.0

It is a Device System for Managing Devices. It uses the MySql database to store the information. This project can be separated two times.

1. Device System
2. Ticket System

**Requirements for the project**

- MySql Server
- ASP.NET dotnet 6.0
	-dotnet ef
- VSCode or Any Editor you Prefer

**Before Anything**

After downloading the project run the following command inside the project 

dotnet ef database update --this will create the database

on this Github the is a project to create a User Login just by running dotnet run command but first

Download this DeviceSystemInitializer project.
--The are a few changes you need to make.
1. ApplicationDbContext.cs from Data Folder
	- change this to match your Database settings
	**server=localhost;user=user1;password=xxxxxx;database=SystemDB**
	
2. Users.cs from Model folder 
	-change the string password to what you like
	** CreatePasswordHash("xxxxxxxx", out byte[] passwordHash, out byte[] passwordSalt)**
Now good to go => dotnet run.

**Uses**
Notice this project was saving user secrets using Microsoft Secret Manager.
so you will have to create your own secret. for the database and email address
Email address secrets are required when creating a ticket for a device, otherwise, the other operation will work fine

**Database secret structure**


**Now back to this Project The project works like this you**
1. Add as Person --- People Table
	1.1 Add Office -- Offices Table
	1.2 Add Department --Department Tables

2. Add/Enrolled as Employee --Employees Table
3. Create a login => Users -- Table 

After having Login, 
-- Can do CRUD operation.
-- Can add Device
-- Assign/Unassign device to employees
-- Can create Ticket for device -- it's will send email
	uses (https://github.com/jstedfast/MailKit)
-- Can Update the Ticket as Admin.

Admin
--Can Acknowledge/Achieve ticket
--Can Do CRUD Operation on all the tablets
