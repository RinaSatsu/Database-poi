# Database Management Application (C# WPF)

Welcome to the Database Management Application! This project is designed to help you manage a database with multiple tables using two different versions: Plain and with the MVVM (Model-View-ViewModel) pattern. It's a simple tool that allows you to interact with the database, view and modify data in the _Departments_ and _Employees_ tables.

## Motivation
The motivation behind this project was to create a user-friendly and efficient way to manage data stored in a database. Whether you're a developer looking to explore different architectural patterns or a database administrator needing a convenient tool, this application provides two distinct versions to suit your needs.

## Technologies Used
This project is built using C# and Windows Presentation Foundation (WPF) and leverages the following technologies:
* __C#:__ The primary programming language for developing the application's logic.
* __WPF (Windows Presentation Foundation):__ Used for creating the graphical user interface (GUI) of the application.
* __SQL Server:__ The database management system for storing and retrieving data.
* __SQL:__ SQL scripts are used to create and populate the database tables.

## Getting Started
__Prerequisites__

Before using the application, ensure you have the following:
* Microsoft Visual Studio: You will need Visual Studio installed to build and run the application.
* SQL Server: Make sure you have SQL Server installed. The default connection string is (localdb)\MSSQLLocalDB, database name-Company.

__Running the Application__
1. Clone or download the repository.
2. Open the solution in Visual Studio.

There are two projects available:

_Plain:_ A version of the application without MVVM pattern.

_MVVM:_ A version of the application using the MVVM architectural pattern.

3. Open the project you want to use and build the solution.
4. Run the application, and you will be able to interact with the database tables.

## Features
* __Database Interaction:__ View, add, edit, and delete records in the `dbo.Departments` and `dbo.Employees` tables.
* __Two Versions:__ Choose between the plain version and the version that follows the MVVM pattern, depending on your architectural preferences.
* __Data Management:__ Easily manage data stored in the database with a user-friendly interface.

## Configuration
The connection string to the database is stored in the App.config file of each sub-folder project. The default connection string is set to (localdb)\MSSQLLocalDB with the database name-Company. You can modify the connection string in the App.config file to connect to your desired database.

## Feedback and Issues
If you encounter any issues or have suggestions for improvements, please open an issue on the GitHub repository. Your feedback is valuable and will help make this application even better.

### Credits
This project was developed by Maryna Snihurska (aka RinaSatsu).
