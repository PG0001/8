"use client";

import { useEffect, useState } from "react";
import api from "@/lib/api";

import {
  Box,
  Typography,
  Paper,
  Stack,
  Button,
  Chip,
  CircularProgress,
} from "@mui/material";

type Notification = {
  id: number;
  title: string;
  message: string;
  isRead: boolean;
  createdOn: string;
};

export default function NotificationsPage() {
  const [items, setItems] = useState<Notification[]>([]);
  const [loading, setLoading] = useState(true);

  const load = async () => {
    const res = await api.get("/notifications");
    setItems(res.data);
    setLoading(false);
  };

  useEffect(() => {
    load();
  }, []);

  const markRead = async (id: number) => {
    await api.patch(`/notifications/${id}/read`);
    setItems((prev) =>
      prev.map((n) =>
        n.id === id ? { ...n, isRead: true } : n
      )
    );
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" mt={6}>
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box maxWidth={900} mx="auto">
      <Typography variant="h5" mb={3}>
        Notifications
      </Typography>

      {items.length === 0 && (
        <Typography color="text.secondary">
          No notifications yet.
        </Typography>
      )}

      <Stack spacing={2}>
        {items.map((n) => (
          <Paper
            key={n.id}
            sx={{
              p: 2,
              borderLeft: n.isRead
                ? "4px solid transparent"
                : "4px solid #1976d2",
              backgroundColor: n.isRead
                ? "white"
                : "rgba(25, 118, 210, 0.05)",
            }}
          >
            <Stack
              direction="row"
              justifyContent="space-between"
              mb={1}
            >
              <Typography fontWeight={600}>
                {n.title}
              </Typography>

              {!n.isRead && (
                <Chip
                  label="New"
                  color="primary"
                  size="small"
                />
              )}
            </Stack>

            <Typography
              variant="body2"
              color="text.secondary"
            >
              {n.message}
            </Typography>

            <Stack
              direction="row"
              justifyContent="space-between"
              alignItems="center"
              mt={2}
            >
              <Typography
                variant="caption"
                color="text.secondary"
              >
                {new Date(n.createdOn).toLocaleString()}
              </Typography>

              {!n.isRead && (
                <Button
                  size="small"
                  onClick={() => markRead(n.id)}
                >
                  Mark as read
                </Button>
              )}
            </Stack>
          </Paper>
        ))}
      </Stack>
    </Box>
  );
}
