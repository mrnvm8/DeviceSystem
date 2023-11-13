 Device System

This project was created using VSCode and dotnet 6.0

It is a Device System for Managing Devices. It uses the MySql database to store the information. This project can be separated two times.

1. Device System
2. Ticket System

**Requirements for the project**

- MySql Server
- ASP.NET dotnet 6.0
- VSCode or Any Editor you Prefer
- 
**Uses**
The works like this you
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
-- Can Upate the Ticket as Admin.

Admin
--Can Acknowledge/Aechive ticket
--Can Do CRUD Operation to all the tablets
