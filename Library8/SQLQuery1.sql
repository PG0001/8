USE EnterpriseTaskManagement;
GO

/* =====================================================
   CLEAN PROJECT-RELATED DATA
===================================================== */

DELETE FROM ProjectChat;
DELETE FROM TaskTimeline;
DELETE FROM TaskComments;
DELETE FROM TaskItems;
DELETE FROM ProjectUsers;
DELETE FROM Projects;
GO


/* =====================================================
   VARIABLES TO STORE GENERATED IDS
===================================================== */

DECLARE @Project1Id INT;
DECLARE @Project2Id INT;

DECLARE @Task1Id INT;
DECLARE @Task2Id INT;
DECLARE @Task3Id INT;
DECLARE @Task4Id INT;


/* =====================================================
   INSERT PROJECTS (CAPTURE IDS)
===================================================== */

INSERT INTO Projects (Name, Description, StartDate, EndDate, Status, CreatedBy)
VALUES
('Enterprise Task Platform',
 'Main internal task management system',
 '2025-01-01', '2025-12-31', 'Active', 5);

SET @Project1Id = SCOPE_IDENTITY();

INSERT INTO Projects (Name, Description, StartDate, EndDate, Status, CreatedBy)
VALUES
('Mobile App Development',
 'Customer-facing mobile app',
 '2025-02-01', NULL, 'Active', 8);

SET @Project2Id = SCOPE_IDENTITY();


/* =====================================================
   PROJECT MEMBERS
===================================================== */

INSERT INTO ProjectUsers (ProjectId, UserId)
VALUES
(@Project1Id, 5),
(@Project1Id, 9),
(@Project1Id, 10),
(@Project1Id, 11),

(@Project2Id, 8),
(@Project2Id, 9),
(@Project2Id, 10);


/* =====================================================
   TASKS (CAPTURE IDS)
===================================================== */

INSERT INTO TaskItems
(ProjectId, Title, Description, AssignedTo, Priority, Status, DueDate)
VALUES
(@Project1Id, 'Setup backend',
 'Initialize ASP.NET Core backend',
 10, 'High', 'Todo', '2025-02-15');

SET @Task1Id = SCOPE_IDENTITY();

INSERT INTO TaskItems
(ProjectId, Title, Description, AssignedTo, Priority, Status, DueDate)
VALUES
(@Project1Id, 'Create DB schema',
 'Design SQL Server schema',
 9, 'High', 'InProgress', '2025-02-10');

SET @Task2Id = SCOPE_IDENTITY();

INSERT INTO TaskItems
(ProjectId, Title, Description, AssignedTo, Priority, Status, DueDate)
VALUES
(@Project1Id, 'Implement SignalR',
 'Real-time updates for tasks',
 11, 'Medium', 'Review', '2025-02-20');

SET @Task3Id = SCOPE_IDENTITY();

INSERT INTO TaskItems
(ProjectId, Title, Description, AssignedTo, Priority, Status, DueDate)
VALUES
(@Project2Id, 'UI Wireframes',
 'Design mobile UI',
 10, 'Low', 'Done', '2025-02-05');

SET @Task4Id = SCOPE_IDENTITY();


/* =====================================================
   TASK COMMENTS
===================================================== */

INSERT INTO TaskComments (TaskId, UserId, CommentText)
VALUES
(@Task1Id, 9, 'Backend structure looks good'),
(@Task1Id, 10, 'I will start today'),
(@Task2Id, 5, 'Make sure indexes are added'),
(@Task3Id, 11, 'SignalR integration pending review');


/* =====================================================
   TASK TIMELINE
===================================================== */

INSERT INTO TaskTimeline (TaskId, UserId, Action)
VALUES
(@Task1Id, 5, 'Task created'),
(@Task1Id, 10, 'Task assigned'),
(@Task2Id, 9, 'Status changed to InProgress'),
(@Task3Id, 11, 'Status changed to Review'),
(@Task4Id, 10, 'Task completed');


/* =====================================================
   PROJECT CHAT
===================================================== */

INSERT INTO ProjectChat (ProjectId, UserId, Message)
VALUES
(@Project1Id, 5, 'Welcome to the project'),
(@Project1Id, 9, 'Let’s start backend work'),
(@Project1Id, 10, 'Working on assigned tasks'),
(@Project2Id, 11, 'Mobile UI ready for review');


/* =====================================================
   FILES
===================================================== */

INSERT INTO Files
(FileName, FilePath, UploadedBy, RelatedEntityId, EntityType)
VALUES
('architecture.pdf', '/uploads/architecture.pdf', 5, @Project1Id, 'Project'),
('api-spec.docx', '/uploads/api-spec.docx', 9, @Project1Id, 'Project'),
('task-notes.txt', '/uploads/task-notes.txt', 10, @Task1Id, 'Task');


/* =====================================================
   NOTIFICATIONS
===================================================== */

INSERT INTO Notifications (UserId, Title, Message)
VALUES
(10, 'Task Assigned', 'You have been assigned "Setup backend"'),
(11, 'Task Review', 'Task "Implement SignalR" is in review'),
(5, 'Project Update', 'New task added to your project');


/* =====================================================
   VERIFY
===================================================== */

SELECT * FROM Projects;
SELECT * FROM ProjectUsers;
SELECT * FROM TaskItems;
SELECT * FROM TaskComments;
SELECT * FROM TaskTimeline;
SELECT * FROM ProjectChat;
SELECT * FROM Files;
SELECT * FROM Notifications;
SELECT * FROM vw_TaskSummary;
GO
