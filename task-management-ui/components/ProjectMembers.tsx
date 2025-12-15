"use client";

import { useEffect, useState } from "react";
import api from "@/lib/api";

import {
  Box,
  Paper,
  Typography,
  Button,
  Stack,
  TextField,
  IconButton,
  List,
  ListItem,
  ListItemText,
  CircularProgress,
  Alert,
} from "@mui/material";

import DeleteIcon from "@mui/icons-material/Delete";

type Member = {
  id: number;
  fullName: string;
  email: string;
  role: string;
};

export default function ProjectMembers({
  projectId,
}: {
  projectId: number;
}) {
  const [members, setMembers] = useState<Member[]>([]);
  const [userId, setUserId] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
const role = localStorage.getItem("role");
const canManageMembers = role === "Admin" || role === "ProjectManager";

  const loadMembers = async () => {
    try {
      setLoading(true);
      const res = await api.get(
        `/projects/${projectId}/members`
      );
      console.log(res.data);
      setMembers(res.data);
    } catch {
      setError("Failed to load project members");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadMembers();
  }, [projectId]);

  const addMember = async () => {
    if (!userId || isNaN(Number(userId))) return;

    setError("");

    try {
      await api.post(`/projects/${projectId}/users/${userId}`);
      setUserId("");
      loadMembers();
    } catch {
      setError("Failed to add member");
    }
  };

  const removeMember = async (id: number) => {
    const confirm = window.confirm(
      "Remove this member from project?"
    );
    if (!confirm) return;

    try {
      await api.delete(
        `/projects/${projectId}/users/${id}`
      );
      loadMembers();
    } catch {
      setError("Failed to remove member");
    }
  };

  return (
    <Box>
      <Typography variant="h6" mb={2}>
        Project Members
      </Typography>

      {/* ADD MEMBER */}
     {canManageMembers && <Paper sx={{ p: 2, mb: 3 }}>
        <Stack direction="row" spacing={2}>
          <TextField
            label="User ID"
            size="small"
            value={userId}
            onChange={(e) => setUserId(e.target.value)}
          />

          <Button
            variant="contained"
            onClick={addMember}
            disabled={!userId || isNaN(Number(userId))}
          >
            Add Member
          </Button>
        </Stack>
      </Paper>
}
      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      {/* MEMBERS LIST */}
      <Paper>
        {loading && (
          <Stack alignItems="center" p={3}>
            <CircularProgress />
          </Stack>
        )}

        {!loading && (
          <List>
            {members.length === 0 && (
              <Typography
                variant="body2"
                color="text.secondary"
                sx={{ p: 2 }}
              >
                No members added yet.
              </Typography>
            )}

            {members.map((m) => (
              <ListItem
                key={m.id}
                divider
                secondaryAction={ canManageMembers &&
                  <IconButton
                    edge="end"
                    onClick={() =>
                      removeMember(m.id)
                    }
                  >
                    <DeleteIcon />
                  </IconButton>
                }
              >
                <ListItemText
                  primary={m.fullName}
                  secondary={m.email + " - " + m.role}
                />
              </ListItem>
            ))}
          </List>
        )}
      </Paper>
    </Box>
  );
}
