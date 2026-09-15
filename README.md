# Product Inventory

A simple product inventory management application built with **ASP.NET Core MVC**.

The application demonstrates basic CRUD operations for managing products, including:

* Product name and description
* Price and quantity
* Inventory status
* Creating, editing, viewing, and deleting products

## Technologies

* C#
* ASP.NET Core MVC
* Razor Views
* Entity Framework Core
* SQL Server
* HTML/CSS

## CI

This project uses GitHub Actions to automatically:

- Restore NuGet packages
- Build the application
- Run unit tests

The workflow runs whenever changes are pushed to `main`.

## Purpose

This project was created as a small example of building a traditional MVC web application with a clean separation between controllers, models, views, and data access.
