README v1.0 / 29 DECEMBER 2017
=

# Project name
Dellasanta

## Introduction
Dellasanta is an MVC web application to administer a school.
There a 3 roles: "Admin", "Student", "Teacher". Student register themselves on the application and choose a single CoursePath.
A CoursePath has many Courses. Courses are taught by a Teacher. Only an Admin can register a new Teacher.
After signing up a Student is logged in and see a menu option that only "Student" profile can see: Course Enrollment. Upon clicking
in Course Enrollment the student is shown a form where he can select a Course from a list of Courses belonging to the CoursePath that she choose
when signing up. The Course Enrollment form is posted via ajax and the student is given immediate feedback of success.


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

    <add key="PathLocalFolder" value="C:\Apparound\Instances\MR_3_\uploads\WSImport\"/>
    <add key="PathLocalFolderAttachments" value="C:\Apparound\Instances\MR_3_\uploads\WSAttachment\"/>
    <add key="USR_DET_Table" value="[dbo].[WS_UsersDetails]"/>
    <add key="USR_HIER_Table" value="[dbo].[WS_UsersHierarchy]"/>
    <add key="SUB_GUID_Table" value="[dbo].[WS_SubGuid]"/>
    <add key="VIEW_Utenti" value="UM_USER" />
    <add key="TAB_DatiUtenti" value="[RES-0G3-]" />
    <add key="TAB_LinkUtentiDatiUtenti" value="[SYS-UTE-2LINK_SYS-UTE-_RES-0G3-]" />

    Note: Values are from svil instance, needs to be adapted for the destination instance (PRE,PROD)

----------------------------------------------------------------------------------

Check if the following folders exist, if not they have to be created :
-  \{PathLocalFolder\}
-  \{PathLocalFolder\}/Archivio
-  \{PathLocalFolder\}/Archivio/Pdv

-  \{PathLocalFolderAttachments\}
 
----------------------------------------------------------------------------------
The project needs "User Data" TO linked with [SYS-UTE] to manage user extrainfo ,
TO has 3 string properties named:
- ws_usrRole
- ws_usrDealer
- ws_usrGuid

The latter should have a new guid as default value "eval(newid())"

----------------------------------------------------------------------------------

Put the file settings.xml on server instance folder so that the structure of the ExtraInfo is known to the UserManager class.
    userprofile section has to be edited with the values relative to "User Data" TO

## Test Configuration
To test this software use unit test in WindsurfImporter.Test in this solution.

## Contributing

## API Reference


## Links
