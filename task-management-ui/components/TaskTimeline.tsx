"use client";

import { useEffect, useState } from "react";
import api from "@/lib/api";

import {
  Box,
  Paper,
  Typography,
  Stack,
  CircularProgress,
  Divider,
} from "@mui/material";

type TimelineItem = {
  action: string;
  createdOn: string;
  userId: number;
};

export default function TaskTimeline({
  taskId,
}: {
  taskId: number;
}) {
  const [items, setItems] = useState<TimelineItem[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api
      .get(`/tasks/${taskId}/timeline`)
      .then((res) => setItems(res.data))
      .finally(() => setLoading(false));
  }, [taskId]);

  return (
    <Paper sx={{ p: 2 }}>
      <Typography variant="h6" mb={2}>
        Timeline
      </Typography>

      {loading && (
        <Stack alignItems="center" p={2}>
          <CircularProgress size={24} />
        </Stack>
      )}

      {!loading && items.length === 0 && (
        <Typography
          variant="body2"
          color="text.secondary"
        >
          No activity recorded yet.
        </Typography>
      )}

      {!loading && items.length > 0 && (
        <Stack spacing={2}>
          {items.map((t, i) => (
            <Box key={i}>
              <Typography variant="body2">
                {t.action}
              </Typography>

              <Typography
                variant="caption"
                color="text.secondary"
              >
                User {t.userId} ·{" "}
                {new Date(t.createdOn).toLocaleString()}
              </Typography>

              {i !== items.length - 1 && (
                <Divider sx={{ mt: 1 }} />
              )}
            </Box>
          ))}
        </Stack>
      )}
    </Paper>
  );
}
