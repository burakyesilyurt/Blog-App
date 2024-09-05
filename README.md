# Blog App

This is a comprehensive **Blog Application** built using **ASP.NET Core** and follows a layered architecture with a focus on Identity and Authorization. The application allows users to create, manage, and interact with articles, including features such as commenting, liking, and role-based access control for Admin, Author, and User roles.

## Features

- **User Management:**
  - Role-based authentication (Admin, Author, User)
  - Registration with role selection (excluding Admin role)
  - Login and logout functionalities
  - JWT Token generation for API access
  - Profile management

- **Article Management:**
  - Create, edit, delete articles
  - Article categorization with tags
  - Image upload for articles
  - Like and view count functionality
  - Commenting system for articles
  - Article approval by Admin

- **Admin Panel:**
  - Manage users and roles
  - Article approval and moderation
  - Site-wide management tools

## Technologies Used

- **Backend:**
  - ASP.NET Core API
  - Entity Framework Core
  - Identity for authentication and authorization
  - JWT Token for API security

- **Frontend:**
  - ASP.NET Core MVC for the web client
  - Razor views with Bootstrap for styling
  - AJAX and jQuery for dynamic content updates

## Project Structure

- **API Project:** 
  - Contains all the API endpoints for the blog.
  - Implements Identity and authorization.
  - Article, User, and Comment management with CRUD operations.

- **Client Project:**
  - Consumes the API via `HttpClient`.
  - Displays articles, allows interaction (like, comment), and handles user authentication.
