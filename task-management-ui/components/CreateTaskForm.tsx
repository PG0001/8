"use client";

import { useEffect, useState } from "react";
import api from "@/lib/api";
import {
  Box,
  TextField,
  Button,
  MenuItem,
  Stack,
  Typography,
Radio,
RadioGroup,
FormControlLabel
} from "@mui/material";


type Priority = "Low" | "Medium" | "High";

const priorityColor = {
  High: "#e53935",
  Medium: "#fb8c00",
  Low: "#43a047",
};

function PriorityDot({ color }: { color: string }) {
  return (
    <Box
      sx={{
        width: 14,
        height: 14,
        borderRadius: "50%",
        bgcolor: color,
        display: "inline-block",
        mr: 1,
      }}
    />
  );
}

type User = {
  id: number;
  fullName: string;
};

export default function CreateTaskForm({
  projectId,
  onCreated,
}: {
  projectId: number;
  onCreated: () => void;
}) {
  const [title, setTitle] = useState("");
  const [assignedTo, setAssignedTo] = useState<number | "">("");
  const [users, setUsers] = useState<User[]>([]);
  const [dueDate, setDueDate] = useState<string>("");
    const [Description, setDescription] = useState<string>("");
    const[priority, setPriority] = useState<"Low" | "Medium" | "High">("Medium");

  // 🔹 load project members
  useEffect(() => {
    api.get(`/projects/${projectId}/members`)
      .then(res => setUsers(res.data));
  }, [projectId]);

  const createTask = async () => {
    if (!title || !assignedTo) return;

    await api.post("/tasks", {
      projectId,
      title,
      Description,
      assignedTo,
      status: "Todo",
      priority,
      dueDate 
    });

    setTitle("");
    setAssignedTo("");
    alert("Task created successfully");
    onCreated(); // reload board
  };

  return (
    <Box>
      <Stack spacing={2}>
        <TextField
          label="Task Title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />
        <TextField
            label="Description"
            value={Description}
            onChange={(e) => setDescription(e.target.value)}
        />  
        <TextField
          select
          label="Assign To"
          value={assignedTo}
          onChange={(e) => setAssignedTo(Number(e.target.value))}
        >
          {users.map((u) => (
            <MenuItem key={u.id} value={u.id}>
              {u.fullName}
            </MenuItem>
          ))}
        </TextField>
      <Typography variant="subtitle2" mb={2}>
  Priority
</Typography>

<RadioGroup
  row
  value={priority}
  onChange={(e) =>
    setPriority(e.target.value as Priority)
  }
>
  {(["Low", "Medium", "High"] as Priority[]).map(
    (p) => (
      <FormControlLabel
        key={p}
        value={p}
        control={<Radio />}
        label={
          <Box display="flex" alignItems="center">
            <PriorityDot color={priorityColor[p]} />
            {p}
          </Box>
        }
      />
    )
  )}
</RadioGroup>

        <TextField
          label="Due Date"
          type="date"
            InputLabelProps={{ shrink: true }}
            value={dueDate}
            onChange={(e) => setDueDate(e.target.value)}
        />

        <Button variant="contained" onClick={createTask}>
          Create Task
        </Button>
      </Stack>
    </Box>
  );
}
