"use client";

import { useEffect, useMemo, useState } from "react";
import { useRouter } from "next/navigation";
import api from "@/lib/api";

import {
  Box,
  Typography,
  TextField,
  Button,
  Paper,
  Stack,
  Chip,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from "@mui/material";

type Project = {
  id: number;
  name: string;
  description: string;
  status: "Active" | "Completed";
  startDate?: string;
  endDate?: string;
};

export default function ProjectsPage() {
  const router = useRouter();

  const [projects, setProjects] = useState<Project[]>([]);
  const [query, setQuery] = useState("");
  const [open, setOpen] = useState(false);

  const [form, setForm] = useState({
    name: "",
    description: "",
    startDate: "",
    endDate: "",
  });

  const loadProjects = async () => {
    const res = await api.get("/projects");
    setProjects(res.data);
  };

  useEffect(() => {
    loadProjects();
  }, []);

  const filtered = useMemo(() => {
    const q = query.toLowerCase();
    return projects.filter(
      (p) =>
        p.name.toLowerCase().includes(q) ||
        p.description?.toLowerCase().includes(q)
    );
  }, [projects, query]);

  const createProject = async () => {
    await api.post("/projects", form);
    setOpen(false);
    setForm({
      name: "",
      description: "",
      startDate: "",
      endDate: "",
    });
    loadProjects();
  };

  return (
    <Box maxWidth={1200} mx="auto">
      {/* HEADER */}
      <Stack
        direction="row"
        justifyContent="space-between"
        alignItems="center"
        mb={3}
      >
        <Typography variant="h5">Projects</Typography>

        <Button variant="contained" onClick={() => setOpen(true)}>
          Create Project
        </Button>
      </Stack>

      {/* SEARCH */}
      <TextField
        fullWidth
        placeholder="Search projects..."
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        sx={{ mb: 3 }}
      />

      {/* PROJECT LIST */}
      <Stack spacing={2}>
        {filtered.map((p) => (
          <Paper
            key={p.id}
            sx={{
              p: 2,
              cursor: "pointer",
              "&:hover": { boxShadow: 4 },
            }}
            onClick={() => router.push(`/projects/${p.id}`)}
          >
            <Stack
              direction="row"
              justifyContent="space-between"
              mb={1}
            >
              <Typography variant="h6">{p.name}</Typography>

              <Chip
                label={p.status}
                color={p.status === "Active" ? "success" : "default"}
                size="small"
              />
            </Stack>

            <Typography variant="body2" color="text.secondary">
              {p.description || "No description"}
            </Typography>

            <Typography
              variant="caption"
              color="text.secondary"
              display="block"
              mt={1}
            >
              {p.startDate &&
                `Start: ${p.startDate.split("T")[0]}`}
              {p.endDate &&
                ` • End: ${p.endDate.split("T")[0]}`}
            </Typography>
          </Paper>
        ))}

        {filtered.length === 0 && (
          <Typography color="text.secondary">
            No projects found.
          </Typography>
        )}
      </Stack>

      {/* CREATE PROJECT DIALOG */}
      <Dialog open={open} onClose={() => setOpen(false)} fullWidth>
        <DialogTitle>Create Project</DialogTitle>

        <DialogContent>
          <TextField
            label="Project Name"
            fullWidth
            margin="dense"
            value={form.name}
            onChange={(e) =>
              setForm({ ...form, name: e.target.value })
            }
          />

          <TextField
            label="Description"
            fullWidth
            margin="dense"
            multiline
            rows={3}
            value={form.description}
            onChange={(e) =>
              setForm({
                ...form,
                description: e.target.value,
              })
            }
          />

          <TextField
            type="date"
            label="Start Date"
            fullWidth
            margin="dense"
            InputLabelProps={{ shrink: true }}
            value={form.startDate}
            onChange={(e) =>
              setForm({
                ...form,
                startDate: e.target.value,
              })
            }
          />

          <TextField
            type="date"
            label="End Date"
            fullWidth
            margin="dense"
            InputLabelProps={{ shrink: true }}
            value={form.endDate}
            onChange={(e) =>
              setForm({
                ...form,
                endDate: e.target.value,
              })
            }
          />
        </DialogContent>

        <DialogActions>
          <Button onClick={() => setOpen(false)}>Cancel</Button>
          <Button variant="contained" onClick={createProject}>
            Create
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}
