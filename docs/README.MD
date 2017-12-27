README v1.0 / 29 DECEMBER 2017
=

# Project name
Dellasanta

## Introduction
Dellasanta is an MVC web application to administer a school.
There a 3 roles: "Admin", "Student", "Teacher". A Student register themselves on the application and choose a single CoursePath along with an email and password to log in.
A CoursePath has many Courses. Courses are taught by a Teacher. Only an Admin can register a new Teacher.
After signing up a Student is logged in and see a menu option that only "Student" profile can see: Course Enrollment. Upon clicking
in Course Enrollment the student is shown a form where he can select a Course from a list of Courses belonging to the CoursePath that she choose
when signing up. The Course Enrollment form is posted via ajax and the student is given immediate feedback of outcome.


## Usage


### Requirements and dependencies
IMPORTANT:
**************************************
The additional packages have been removed to send a slimmer zip over the wire, as are the bin and obj folder. 
If you have the "Enable restore Nuget packages" option enabled on VS 2017 upon building the solution the packages will be automatically downloaded from Nuget.org 
(please check the Package Manager console for the actual Package source and select Nget.org). 
If you don't have the option enabled you will have to enable it:
https://docs.microsoft.com/en-us/nuget/consume-packages/package-restore
https://docs.microsoft.com/en-us/nuget/consume-packages/package-restore#enabling-and-disabling-package-restore
**************************************




### Installation

IMPORTANT:
**************************************
The project use Entity Framework Migrations, to generate the database and seed it with seminal data:
-Go to the Web.config of the Dellasanta.Web project and modify the connection string.
-Go to Package Manager Console and select Default project = "Libraries\DellaSanta.DataLayer"
-Execute the following command from the shell "update-database -verbose"

**************************************


### Configuration
----------------------------------------------------------------------------------

The software needs the following configuration keys to be set in Web.config file:

<connectionStrings>
    <add name="DefaultConnection" connectionString="Data Source=localhost\SQLEXPRESS01;Initial Catalog=DellaSantaSchool;Integrated Security=SSPI;MultipleActiveResultSets=True;" providerName="System.Data.SqlClient" />
</connectionStrings>

----------------------------------------------------------------------------------


## Test Configuration
To test this software use unit test and integration tests in Tests folder in this solution.

## Contributing

## API Reference


## Links
