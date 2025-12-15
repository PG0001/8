"use client";

import { useParams } from "next/navigation";
import ProtectedRoute from "@/components/ProtectedRoute";
import ProjectChat from "@/components/ProjectChat";

import { Box, Typography } from "@mui/material";

export default function ProjectChatPage() {
  const params = useParams();

  const projectId = Number(
    Array.isArray(params.id) ? params.id[0] : params.id
  );

  if (!projectId) {
    return <Typography>Invalid project</Typography>;
  }

  return (
    <ProtectedRoute>
      <Box maxWidth={1200} mx="auto">
        <Typography variant="h5" mb={2}>
          Project Chat
        </Typography>

        <ProjectChat projectId={projectId} />
      </Box>
    </ProtectedRoute>
  );
}
