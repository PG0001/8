"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import ProtectedRoute from "@/components/ProtectedRoute";
import api from "@/lib/api";

import {
  Box,
  Typography,
  Paper,
  Stack,
  Chip,
} from "@mui/material";

type Task = {
  id: number;
  title: string;
  status: "Todo" | "InProgress" | "Review" | "Done";
  priority: "Low" | "Medium" | "High";
  projectName: string;
};

export default function MyTasksPage() {
  const router = useRouter();
  const [tasks, setTasks] = useState<Task[]>([]);

  useEffect(() => {
    api.get("/tasks/my").then((res) => setTasks(res.data));
  }, []);

  return (
    <ProtectedRoute>
      <Box maxWidth={1000} mx="auto">
        <Typography variant="h5" mb={3}>
          My Tasks
        </Typography>

        {tasks.length === 0 && (
          <Typography color="text.secondary">
            No tasks assigned.
          </Typography>
        )}

        <Stack spacing={2}>
          {tasks.map((t) => (
            <Paper
              key={t.id}
              sx={{
                p: 2,
                cursor: "pointer",
                "&:hover": { boxShadow: 4 },
              }}
              onClick={() => router.push(`/tasks/${t.id}`)}
            >
              <Stack
                direction="row"
                justifyContent="space-between"
                alignItems="center"
              >
                <Box>
                  <Typography fontWeight={600}>
                    {t.title}
                  </Typography>
                  <Typography
                    variant="body2"
                    color="text.secondary"
                  >
                    {t.projectName}
                  </Typography>
                </Box>

                <Stack direction="row" spacing={1}>
                  <Chip
                    label={t.status}
                    color={statusColor(t.status)}
                    size="small"
                  />
                  <Chip
                    label={t.priority}
                    color={priorityColor(t.priority)}
                    size="small"
                  />
                </Stack>
              </Stack>
            </Paper>
          ))}
        </Stack>
      </Box>
    </ProtectedRoute>
  );
}

/* ---------- HELPERS ---------- */

function statusColor(
  status: Task["status"]
): "default" | "info" | "warning" | "success" {
  switch (status) {
    case "Todo":
      return "default";
    case "InProgress":
      return "info";
    case "Review":
      return "warning";
    case "Done":
      return "success";
  }
}

function priorityColor(
  priority: Task["priority"]
): "success" | "warning" | "error" {
  switch (priority) {
    case "Low":
      return "success";
    case "Medium":
      return "warning";
    case "High":
      return "error";
  }
}
