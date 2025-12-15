"use client";

import { useParams } from "next/navigation";
import { useState } from "react";

import ProtectedRoute from "@/components/ProtectedRoute";
import TaskBoard from "@/components/TaskBoard";
import ProjectChat from "@/components/ProjectChat";
import ProjectFiles from "@/components/ProjectFiles";
import ProjectMembers from "@/components/ProjectMembers";

import {
  Box,
  Typography,
  Tabs,
  Tab,
  Paper,
} from "@mui/material";

export default function ProjectDetailsPage() {
  const params = useParams();

  const projectId = Number(
    Array.isArray(params.id) ? params.id[0] : params.id
  );

  const [tab, setTab] = useState(0);

  if (!projectId) {
    return <Typography>Invalid project</Typography>;
  }

  return (
    <ProtectedRoute>
      <Box maxWidth={1200} mx="auto">
        {/* HEADER */}
        <Typography variant="h5" mb={2}>
          Project #{projectId}
        </Typography>

        {/* TABS */}
        <Paper sx={{ mb: 3 }}>
          <Tabs
            value={tab}
            onChange={(_, v) => setTab(v)}
            variant="scrollable"
          >
            <Tab label="Tasks" />
            <Tab label="Members" />
            <Tab label="Chat" />
            <Tab label="Files" />
          </Tabs>
        </Paper>

        {/* TAB CONTENT */}
        {tab === 0 && (
          <Paper sx={{ p: 2 }}>
            <TaskBoard projectId={projectId} />
          </Paper>
        )}

        {tab === 1 && (
          <Paper sx={{ p: 2 }}>
            <ProjectMembers projectId={projectId} />
          </Paper>
        )}

        {tab === 2 && (
          <Paper sx={{ p: 2 }}>
            <ProjectChat projectId={projectId} />
          </Paper>
        )}

        {tab === 3 && (
          <Paper sx={{ p: 2 }}>
            <ProjectFiles projectId={projectId} />
          </Paper>
        )}
      </Box>
    </ProtectedRoute>
  );
}
