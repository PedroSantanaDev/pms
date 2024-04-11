# Product Management System

This is a basic Product  Management System app that includes the following modules:

1. Item Module (w/ CRUD)
2. Customer Module (w/ CRUD)
3. Customer Item Module (w/ CRUD)
4. Category Module
4. Report Module

The basic idea behind the application is that an user can create/update/delete
customers, Items, categories and assign items customers. Also add the ability to run reports listing
items, customer, and items assigned to customers.

The applications is a Blazor web app written in .Net 8 using Entity Framework core and SQLlite.

# App Setup Guide

This guide will walk you through the steps to set up and run the Blazor web app using the provided PowerShell script (`setup.ps1`).

## Prerequisites

Before running the setup script, ensure you have the following prerequisites installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8) the setup will try to install .NET if it is not install but if it fails please install manually
- PowerShell (version 5.1 or later recommended)

## Setup Instructions

### Manual Download

You can manually download the project as a ZIP file by following these steps:

1. Navigate to the repository on GitHub: https://github.com/PedroSantanaDev/pms.git
2. Click on the green "Code" button located near the top-right corner of the page.
3. Select "Download ZIP" from the dropdown menu.
4. Save the ZIP file to your desired location on your local machine.
5. Extract the contents of the ZIP file to a folder.

###  Using Git

You can clone the repository to your local machine using Git.

To clone the repository using Git, open your terminal or command prompt and run the following command:

- **Clone the Repository:** Clone this repository to your local machine using Git:

   ```bash
   git clone https://github.com/PedroSantanaDev/pms.git


### Running the app

Once you have cloned or downloaded the project, you'll need to navigate to the project directory in order to run the setup script.

#### Using Windows File Explorer

If you prefer to use the Windows interface, you can navigate to the project directory using File Explorer:

1. Open File Explorer.
2. Navigate to the location where you cloned or extracted the project.
3. Navigate to pms/pms.app to directory.
4. Once you are inside the directory pms/pms.app right-click on the `setup.ps1` file.
5. Select "Run with PowerShell" from the context menu.
6. **Follow the On-Screen Instructions:** The setup script will perform the following tasks:
   - Check if the .NET SDK is installed and install it if necessary.
   - Restore NuGet packages.
   - Apply Entity Framework Core migrations.
   - Start the Blazor app in the default web browser.


#### Using Command Line

If you're comfortable with the command line, follow these steps:

1. Open your terminal or command prompt.

2. Navigate to the project directory if you haven't already.
    ```bash
   cd pms/pms.app
   
3. Run the following command:
   ```powershell
   .\setup.ps1
4. **Follow the On-Screen Instructions:** The setup script will perform the following tasks:
   - Check if the .NET SDK is installed and install it if necessary.
   - Restore NuGet packages.
   - Apply Entity Framework Core migrations.
   - Start the Blazor app in the default web browser.
   
**This script will:**
- Check if the .NET SDK is installed and install it if necessary.
- Restore NuGet packages.
- Apply Entity Framework Core migrations.
- Start the Blazor app.

## Access the App

Once the setup process is complete, the Blazor app will automatically open in your default web browser. If it doesn't open automatically, you can access the app by navigating to the URL displayed in your terminal or command prompt after running the setup script.

### Sign In

The app requires authentication, please use the credentials below to sign in

1. Enter the following credentials (username and password) in the provided fields.
   - Email: admin@pms.com
   - Password: Megatron4584$$
5. Click on the "Log In" button to authenticate.
6. Once authenticated, you should have access to the all app's features.

If you encounter any issues during the sign-in process, please refer to the app's documentation or contact the app's administrator for assistance.
