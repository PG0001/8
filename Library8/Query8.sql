-- ============================================
-- DATABASE: EnterpriseTaskManagement
-- ============================================
CREATE DATABASE EnterpriseTaskManagement;
GO
USE EnterpriseTaskManagement;
GO

-- ============================================
-- USERS & ROLES
-- ============================================

CREATE TABLE Roles (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Users (
    Id INT IDENTITY PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    RoleId INT NOT NULL,
    CreatedOn DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);

-- Insert default roles
INSERT INTO Roles (Name) VALUES ('Admin'), ('ProjectManager'), ('Employee');


-- ============================================
-- PROJECTS
-- ============================================

CREATE TABLE Projects (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    StartDate DATE NOT NULL,
    EndDate DATE,
    Status NVARCHAR(20) NOT NULL,  -- Active / Completed
    CreatedBy INT NOT NULL,
    CreatedOn DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
);

-- Many-to-Many: Project Members
CREATE TABLE ProjectUsers (
    Id INT IDENTITY PRIMARY KEY,
    ProjectId INT NOT NULL,
    UserId INT NOT NULL,
    AddedOn DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Performance Index
CREATE INDEX IX_ProjectUsers_ProjectId ON ProjectUsers(ProjectId);
CREATE INDEX IX_ProjectUsers_UserId ON ProjectUsers(UserId);


-- ============================================
-- TASKS
-- ============================================

CREATE TABLE TaskItems (
    Id INT IDENTITY PRIMARY KEY,
    ProjectId INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    AssignedTo INT,
    Priority NVARCHAR(20) NOT NULL,   -- Low / Medium / High
    Status NVARCHAR(20) NOT NULL,     -- Todo / InProgress / Review / Done
    DueDate DATE,
    CreatedOn DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id),
    FOREIGN KEY (AssignedTo) REFERENCES Users(Id)
);

CREATE INDEX IX_TaskItems_ProjectId ON TaskItems(ProjectId);
CREATE INDEX IX_TaskItems_AssignedTo ON TaskItems(AssignedTo);


-- ============================================
-- TASK COMMENTS
-- ============================================

CREATE TABLE TaskComments (
    Id INT IDENTITY PRIMARY KEY,
    TaskId INT NOT NULL,
    UserId INT NOT NULL,
    CommentText NVARCHAR(MAX) NOT NULL,
    CreatedOn DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (TaskId) REFERENCES TaskItems(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE INDEX IX_TaskComments_TaskId ON TaskComments(TaskId);


-- ============================================
-- TASK TIMELINE (Activity Log)
-- ============================================

CREATE TABLE TaskTimeline (
    Id INT IDENTITY PRIMARY KEY,
    TaskId INT NOT NULL,
    UserId INT NOT NULL,
    Action NVARCHAR(200) NOT NULL,  -- e.g., "Status changed to InProgress"
    CreatedOn DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (TaskId) REFERENCES TaskItems(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);


-- ============================================
-- SIGNALR CHAT (Real-Time Project Chat)
-- ============================================

CREATE TABLE ProjectChat (
    Id INT IDENTITY PRIMARY KEY,
    ProjectId INT NOT NULL,
    UserId INT NOT NULL,
    Message NVARCHAR(MAX),
    FileUrl NVARCHAR(500),
    SentOn DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);


-- ============================================
-- FILE UPLOADS (TASK & PROJECT FILES)
-- ============================================

CREATE TABLE Files (
    Id INT IDENTITY PRIMARY KEY,
    FileName NVARCHAR(300),
    FilePath NVARCHAR(500),
    UploadedBy INT NOT NULL,
    UploadedOn DATETIME2 DEFAULT GETDATE(),
    RelatedEntityId INT NOT NULL,       -- ProjectId or TaskId
    EntityType NVARCHAR(20) NOT NULL,   -- "Project" or "Task"
    FOREIGN KEY (UploadedBy) REFERENCES Users(Id)
);

CREATE INDEX IX_Files_RelatedEntityId ON Files(RelatedEntityId);


-- ============================================
-- NOTIFICATIONS
-- ============================================

CREATE TABLE Notifications (
    Id INT IDENTITY PRIMARY KEY,
    UserId INT NOT NULL,
    Title NVARCHAR(200),
    Message NVARCHAR(MAX),
    IsRead BIT DEFAULT 0,
    CreatedOn DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE INDEX IX_Notifications_UserId ON Notifications(UserId);


-- ============================================
-- BACKGROUND JOB LOG (Optional)
-- ============================================
CREATE TABLE BackgroundJobLogs (
    Id INT IDENTITY PRIMARY KEY,
    JobName NVARCHAR(200),
    RunTime DATETIME2 DEFAULT GETDATE(),
    RecordsProcessed INT
);

go
-- ============================================
-- DASHBOARD AGGREGATION VIEW (Optional)
-- ============================================
CREATE VIEW vw_TaskSummary AS
SELECT
    COUNT(*) AS TotalTasks,
    SUM(CASE WHEN Status = 'Todo' THEN 1 ELSE 0 END) AS TodoCount,
    SUM(CASE WHEN Status = 'InProgress' THEN 1 ELSE 0 END) AS InProgressCount,
    SUM(CASE WHEN Status = 'Review' THEN 1 ELSE 0 END) AS ReviewCount,
    SUM(CASE WHEN Status = 'Done' THEN 1 ELSE 0 END) AS DoneCount
FROM TaskItems;
GO


Select * from sys.tables;

Select * from sys.views;

select * from sys.indexes;

Select * from sys.foreign_keys;

select * from sys.schemas;

go 

select * from ProjectUsers;
select * from vw_TaskSummary;
select * from Roles;
select * from Users;
select * from Projects;
select * from TaskItems;
select * from TaskComments;
select * from TaskTimeline;
select * from ProjectChat;
select * from Files;
select * from Notifications;
select * from BackgroundJobLogs;


;


