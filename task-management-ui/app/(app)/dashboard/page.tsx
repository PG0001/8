"use client";

import { useEffect, useState } from "react";
import api from "@/lib/api";

import {
  Box,
  Paper,
  Typography,
  Stack,
  Divider,
} from "@mui/material";

/* ---------- TYPES MATCH BACKEND ---------- */

type DashboardData = {
  totalProjects: number;
  totalTasks: number;
  overdueTasks: number;
  upcomingDeadlines: number;

  tasksByStatus: Record<string, number>;
  tasksByPriority: Record<string, number>;
  userWorkload: Record<string, number>;
};

export default function DashboardPage() {
  const [data, setData] = useState<DashboardData | null>(null);

  useEffect(() => {
    api.get("/dashboard/summary").then((res) => {
      setData(res.data);
    });
  }, []);

  if (!data) {
    return <Typography>Loading dashboard...</Typography>;
  }

  return (
    <Box maxWidth={1200} mx="auto">
      <Typography variant="h5" mb={3}>
        Dashboard
      </Typography>

      {/* METRICS */}
      <Stack direction="row" spacing={2} mb={3}>
        <Metric title="Projects" value={data.totalProjects} />
        <Metric title="Tasks" value={data.totalTasks} />
        <Metric title="Overdue" value={data.overdueTasks} />
        <Metric
          title="Upcoming"
          value={data.upcomingDeadlines}
        />
      </Stack>

      {/* STATUS + PRIORITY */}
      <Stack direction="row" spacing={2} mb={3}>
        <StatsCard
          title="Tasks by Status"
          items={Object.entries(data.tasksByStatus)}
        />

        <StatsCard
          title="Tasks by Priority"
          items={Object.entries(data.tasksByPriority)}
        />
      </Stack>

      {/* USER WORKLOAD */}
      <Paper sx={{ p: 3 }}>
        <Typography variant="h6" mb={2}>
          User Workload
        </Typography>

        {Object.keys(data.userWorkload).length === 0 && (
          <Typography color="text.secondary">
            No workload data available
          </Typography>
        )}

        <Stack spacing={1}>
          {Object.entries(data.userWorkload).map(
            ([userName, count]) => (
              <Box
                key={userName}
                display="flex"
                justifyContent="space-between"
              >
                <Typography>{userName}</Typography>
                <Typography fontWeight={600}>
                  {count}
                </Typography>
              </Box>
            )
          )}
        </Stack>
      </Paper>
    </Box>
  );
}

/* ---------- SUB COMPONENTS ---------- */

function Metric({
  title,
  value,
}: {
  title: string;
  value: number;
}) {
  return (
    <Paper sx={{ p: 2, flex: 1 }}>
      <Typography
        variant="body2"
        color="text.secondary"
      >
        {title}
      </Typography>
      <Typography variant="h4">{value}</Typography>
    </Paper>
  );
}

function StatsCard({
  title,
  items,
}: {
  title: string;
  items: [string, number][];
}) {
  return (
    <Paper sx={{ p: 3, flex: 1 }}>
      <Typography variant="h6" mb={2}>
        {title}
      </Typography>

      <Stack spacing={1}>
        {items.map(([label, value]) => (
          <Box key={label}>
            <Box
              display="flex"
              justifyContent="space-between"
            >
              <Typography>{label}</Typography>
              <Typography fontWeight={600}>
                {value}
              </Typography>
            </Box>
            <Divider />
          </Box>
        ))}
      </Stack>
    </Paper>
  );
}
