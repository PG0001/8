"use client";

import { use, useEffect, useState } from "react";
import api from "@/lib/api";
import { createTaskHubConnection } from "@/lib/signalr";
import CreateTaskForm from "@/components/CreateTaskForm"

import {
  Box,
  Paper,
  Typography,
  Stack,
  Chip,
  CircularProgress,
  Alert,
  Button,
} from "@mui/material";

import {
  DragDropContext,
  Droppable,
  Draggable,
  DropResult,
} from "@hello-pangea/dnd";
import { create } from "domain";

type TaskStatus = "Todo" | "InProgress" | "Review" | "Done";
type TaskPriority = "Low" | "Medium" | "High";
console.log(CreateTaskForm)
type Task = {
  id: number;
  title: string;
  status: TaskStatus;
  priority: TaskPriority;
};
 
const STATUSES: TaskStatus[] = [
  "Todo",
  "InProgress",
  "Review",
  "Done",
];

export default function TaskBoard({
  projectId,
}: {
  projectId: number;
}) {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [hide, setHide] = useState(false);


  const loadTasks = async () => {
    try {
      setLoading(true);
      const res = await api.get(
        `/tasks/project/${projectId}`
      );
      setTasks(res.data);
    } catch {
      setError("Failed to load tasks");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadTasks();

    const connection = createTaskHubConnection();

    connection
      .start()
      .then(() => connection.invoke("JoinProject", projectId))
      .catch(console.error);

    const statusHandler = (data: {
      TaskId: number;
      Status: TaskStatus;
    }) => {
      setTasks((prev) =>
        prev.map((t) =>
          t.id === data.TaskId
            ? { ...t, status: data.Status }
            : t
        )
      );
    };

    connection.on("TaskStatusChanged", statusHandler);

    return () => {
      connection.off("TaskStatusChanged", statusHandler);
      connection.stop();
    };
  }, [projectId]);

  const onDragEnd = async (result: DropResult) => {
    const { destination, source, draggableId } = result;

    if (!destination) return;
    if (
      destination.droppableId === source.droppableId &&
      destination.index === source.index
    )
      return;

    const taskId = Number(draggableId);
    const newStatus = destination.droppableId as TaskStatus;

    // optimistic update
    const prev = [...tasks];
    setTasks((prev) =>
      prev.map((t) =>
        t.id === taskId ? { ...t, status: newStatus } : t
      )
    );

    try {
      await api.patch(`/tasks/${taskId}/status`, {
        status: newStatus,
      });
    } catch {
      setTasks(prev); // rollback
      setError("Failed to update task status");
    }
  };

  if (loading) {
    return (
      <Stack alignItems="center" p={4}>
        <CircularProgress />
      </Stack>
    );
  }

  return (
    <Box>
      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}
      <Button onClick={() => setHide((hide) => !hide)}>Create Task</Button>
      {hide && <CreateTaskForm projectId={projectId} onCreated={loadTasks} />}


      <DragDropContext onDragEnd={onDragEnd}>
        <Stack direction="row" spacing={2}>
          {STATUSES.map((status) => (
            <Droppable droppableId={status} key={status}>
              {(provided) => (
                <Paper
                  ref={provided.innerRef}
                  {...provided.droppableProps}
                  sx={{
                    p: 2,
                    width: 260,
                    minHeight: 420,
                    bgcolor: "#f5f5f5",
                  }}
                >
                  <Typography
                    variant="subtitle1"
                    fontWeight={600}
                    mb={2}
                  >
                    {status}
                  </Typography>

                  {tasks
                    .filter((t) => t.status === status)
                    .map((task, index) => (
                      <Draggable
                        key={task.id}
                        draggableId={task.id.toString()}
                        index={index}
                      >
                        {(provided) => (
                          <Paper
                            ref={provided.innerRef}
                            {...provided.draggableProps}
                            {...provided.dragHandleProps}
                            sx={{
                              p: 2,
                              mb: 2,
                              cursor: "grab",
                            }}
                          >
                            <Typography
                              variant="body2"
                              mb={1}
                            >
                              {task.title}
                            </Typography>

                            <Chip
                              size="small"
                              label={task.priority}
                              color={
                                task.priority === "High"
                                  ? "error"
                                  : task.priority === "Medium"
                                  ? "warning"
                                  : "success"
                              }
                            />
                          </Paper>
                        )}
                      </Draggable>
                    ))}

                  {provided.placeholder}
                </Paper>
              )}
            </Droppable>
          ))}
        </Stack>
      </DragDropContext>
    </Box>
  );
}
