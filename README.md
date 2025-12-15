# 🧩 Enterprise Task Management & Collaboration Platform

> 🚀 A scalable, enterprise-grade task management and collaboration system with real-time updates, role-based access control, and modular architecture.

---

## 📖 Overview

The **Enterprise Task Management & Collaboration Platform** is a full-stack application designed to manage projects, tasks, teams, and communication in a single unified system.

It supports:
- Role-based access control
- Real-time collaboration using SignalR
- Notifications & background jobs
- File uploads
- Analytics dashboards
- Clean Architecture principles

---

## 🛠 Tech Stack

### Backend
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- SQL Server
- SignalR (Real-time communication)
- JWT Authentication
- IMemoryCache
- Background Services (`IHostedService`)

### Frontend
- React / Next.js
- Material UI (MUI)
- Axios
- SignalR Client

---

## 🏗 System Architecture
Client (React)
↓
ASP.NET Core Web API
↓
Application Services
↓
Repositories (EF Core)
↓
SQL Server


- Clean Architecture
- Repository Pattern
- Separation of Concerns
- Real-time communication via SignalR

---

## 👥 User Roles

| Role | Capabilities |
|----|----|
| **Admin** | Full access, dashboard & analytics |
| **Project Manager** | Create projects, assign users & tasks |
| **Employee** | Work on assigned tasks, chat & comments |

---

## ✨ Core Features

### 🔐 Authentication & Authorization
- JWT-based authentication
- Role-based authorization
- Profile update
- Password update

---

### 📂 Project Management
- Create projects (Admin / PM only)
- Add / remove users from projects
- View all projects / my projects
- Search & filter projects
- Project-level chat & files

---

### ✅ Task Management
- Create tasks under projects
- Assign & reassign tasks
- Task priority:
  - Low / Medium / High
- Task status:
  - Todo → InProgress → Review → Done
- Task comments
- Task timeline (audit history)

---

## ⚡ Real-Time Collaboration (SignalR)

| Feature | Description |
|------|------------|
| Task updates | Live task status updates for all members |
| Project chat | Group chat per project |
| Task comments | Real-time comment updates |
| Notifications | Instant notification alerts |

---

## 🔔 Notifications & Background Jobs

### Notification Types
- Task assigned
- Task status changed
- New project member added
- New chat message
- Task due reminders

### Background Worker
- Runs every **1 hour**
- Sends reminders for tasks due in next **24 hours**
- Implemented using `IHostedService`

---

## 📎 File Upload Module

- Upload task attachments
- Upload project-level documents
- Metadata stored in database
- Files stored in `/Uploads` directory

---

## 📊 Dashboard & Analytics (Admin Only)

- Total projects
- Total tasks
- Tasks by status
- Tasks by priority
- User workload
- Overdue tasks
- Upcoming deadlines

---

## 🚀 Performance & Caching

- Uses `IMemoryCache`
- Cached data:
  - Project list
  - Tasks per project
  - Dashboard analytics
- Cache duration: **10 minutes**
- Cache invalidation on data updates

---

## 📁 Project Structure
Project-8/
│
├── TaskManagementAPI/ # Backend API
├── Library8/ # Data & Repository layer
├── task-management-ui/ # Frontend (React)
├── Project-8.sln
├── .gitignore
└── README.md


---

## 📘 API Documentation

- Swagger enabled
- JWT secured endpoints
- REST + SignalR hubs
- Clean & versioned APIs

---

## 🧪 Demo Features

- Create project
- Assign users
- Create tasks
- Real-time task updates
- Project chat
- Notifications
- Dashboard analytics

---

## 🔮 Future Enhancements

- Email notifications
- SMS notifications
- Azure Blob Storage for files
- Advanced reporting
- CI/CD pipeline

---



