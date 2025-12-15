"use client";

import { useParams } from "next/navigation";
import ProtectedRoute from "@/components/ProtectedRoute";
import TaskComments from "@/components/TaskComments";
import TaskTimeline from "@/components/TaskTimeline";
import TaskFiles from "@/components/TaskFiles";

import {
  Box,
  Typography,
  Stack,
  Paper,
} from "@mui/material";

export default function TaskDetailsPage() {
  const params = useParams();

  const taskId = Number(
    Array.isArray(params.id) ? params.id[0] : params.id
  );

  if (!taskId) {
    return <Typography>Invalid task</Typography>;
  }

  return (
    <ProtectedRoute>
      <Box maxWidth={900} mx="auto">
        <Typography variant="h5" mb={3}>
          Task #{taskId}
        </Typography>

        <Stack spacing={3}>
          <Paper sx={{ p: 2 }}>
            <TaskComments taskId={taskId} />
          </Paper>

          <Paper sx={{ p: 2 }}>
            <TaskFiles taskId={taskId} />
          </Paper>

          <Paper sx={{ p: 2 }}>
            <TaskTimeline taskId={taskId} />
          </Paper>
        </Stack>
      </Box>
    </ProtectedRoute>
  );
}
