"use client";

import { useEffect, useRef, useState } from "react";
import api from "@/lib/api";
import { createTaskHubConnection } from "@/lib/signalr";

import {
  Box,
  Paper,
  Typography,
  Stack,
  TextField,
  IconButton,
  Divider,
} from "@mui/material";
import SendIcon from "@mui/icons-material/Send";

type Comment = {
  userId: number;
  comment: string;
  createdOn: string;
};

export default function TaskComments({
  taskId,
}: {
  taskId: number;
}) {
  const [comments, setComments] = useState<Comment[]>([]);
  const [text, setText] = useState("");

  const bottomRef = useRef<HTMLDivElement>(null);

  // -------- LOAD COMMENTS --------
  const loadComments = async () => {
    const res = await api.get(
      `/tasks/${taskId}/comments`
    );
    setComments(res.data);
  };

  // -------- SIGNALR --------
  useEffect(() => {
    loadComments();

    const connection = createTaskHubConnection();

    connection
      .start()
      .catch(console.error);

    const handler = (data: {
      TaskId: number;
      UserId: number;
      Comment: string;
    }) => {
      if (data.TaskId === taskId) {
        setComments((prev) => [
          ...prev,
          {
            userId: data.UserId,
            comment: data.Comment,
            createdOn: new Date().toISOString(),
          },
        ]);
      }
    };

    connection.on("TaskCommentAdded", handler);

    return () => {
      connection.off("TaskCommentAdded", handler);
      connection.stop();
    };
  }, [taskId]);

  // -------- AUTO SCROLL --------
  useEffect(() => {
    bottomRef.current?.scrollIntoView({
      behavior: "smooth",
    });
  }, [comments]);

  // -------- ADD COMMENT --------
  const addComment = async () => {
    if (!text.trim()) return;

    await api.post(`/tasks/${taskId}/comments`, {
      comment: text,
    });

    setText("");
  };

  return (
    <Paper sx={{ p: 2 }}>
      <Typography variant="h6" mb={2}>
        Comments
      </Typography>

      {/* COMMENT LIST */}
      <Stack spacing={1} mb={2}>
        {comments.length === 0 && (
          <Typography
            variant="body2"
            color="text.secondary"
          >
            No comments yet.
          </Typography>
        )}

        {comments.map((c, i) => (
          <Box key={i}>
            <Typography
              variant="caption"
              fontWeight={600}
            >
              User {c.userId}
            </Typography>
            <Typography variant="body2">
              {c.comment}
            </Typography>
            <Typography
              variant="caption"
              color="text.secondary"
            >
              {new Date(c.createdOn).toLocaleString()}
            </Typography>
            <Divider sx={{ mt: 1 }} />
          </Box>
        ))}
        <div ref={bottomRef} />
      </Stack>

      {/* INPUT */}
      <Box display="flex" gap={1}>
        <TextField
          fullWidth
          size="small"
          placeholder="Add a comment..."
          value={text}
          onChange={(e) => setText(e.target.value)}
          onKeyDown={(e) =>
            e.key === "Enter" && addComment()
          }
        />
        <IconButton
          color="primary"
          onClick={addComment}
        >
          <SendIcon />
        </IconButton>
      </Box>
    </Paper>
  );
}
