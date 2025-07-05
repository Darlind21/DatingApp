# Dating App

## Project Overview

This app replicates the complete functionality expected of a modern dating app. It includes user registration, secure authentication, profile management, photo uploads, real-time messaging, live presence, filtering, pagination, user-matching, role-management and more.

## Live Demo

**Hosted on Azure:** https://datingapp-demo.azurewebsites.net/

## Tech Stack

### Backend – ASP.NET Core 8 Web API
- Clean architecture with layered structure
- Entity Framework Core with Code-First approach
- Identity with JWT Authentication & Role-based Authorization
- Repository + Unit of Work patterns
- LINQ-based querying and pagination
- Cloud storage integration for media uploads (Cloudinary)
- Global error handling

### Frontend – Angular 17
- Standalone components and modern Angular architecture
- Angular Routing & Guards
- Interceptors for token handling and global error notifications
- Form validation (template-driven and reactive)
- Bootstrap and custom styles for responsive UI

### Real-time Communication
- SignalR for real-time messaging and presence detection

### Cloud & Deployment
- Deployed to Microsoft Azure App Service
- Azure SQL Database (Serverless)

## Key Features

### Authentication
- Secure login/register with JWT
- Role-based access control

### User Profiles
- Edit profile functionality
- Upload photo capability
- View user details

### Matchmaking
- Like/Dislike system
- Match suggestions
- Compatibility logic

### Messaging
- Real-time chat with SignalR
- Message thread history

### Notifications
- Online status indicators
- Presence tracking

### Admin Panel
- Role management
- User moderation

### Persistence
- Paginated and filtered lists
- User preferences stored
