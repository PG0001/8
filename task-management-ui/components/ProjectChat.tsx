"use client";

import { useEffect, useRef, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { createChatHubConnection } from "@/lib/signalr";

import {
  Box,
  Paper,
  Typography,
  TextField,
  IconButton,
  Stack,
} from "@mui/material";
import SendIcon from "@mui/icons-material/Send";

type ChatMessage = {
  user: string;
  message: string;
  sentOn: string;
};

export default function ProjectChat({
  projectId,
}: {
  projectId: number;
}) {
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [text, setText] = useState("");
  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const bottomRef = useRef<HTMLDivElement>(null);

  // ---------------- SIGNALR SETUP ----------------
  useEffect(() => {
    const connection = createChatHubConnection();

    connection.on("ReceiveMessage", (msg: ChatMessage) => {
      setMessages((prev) => [...prev, msg]);
    });

    connection
      .start()
      .then(() => connection.invoke("JoinProjectChat", projectId))
      .catch(console.error);

    connectionRef.current = connection;

    return () => {
      connection.off("ReceiveMessage");
      connection.stop();
    };
  }, [projectId]);

  // ---------------- AUTO SCROLL ----------------
  useEffect(() => {
    bottomRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  // ---------------- SEND MESSAGE ----------------
  const send = async () => {
    if (!text.trim() || !connectionRef.current) return;

    await connectionRef.current.invoke(
      "SendMessage",
      projectId,
      text
    );

    setText("");
  };

  return (
    <Paper
      sx={{
        height: 520,
        display: "flex",
        flexDirection: "column",
      }}
    >
      {/* HEADER */}
      <Box p={2} borderBottom="1px solid #e0e0e0">
        <Typography fontWeight={600}>
          Project Chat
        </Typography>
      </Box>

      {/* MESSAGES */}
      <Box
        flex={1}
        p={2}
        sx={{ overflowY: "auto", bgcolor: "#fafafa" }}
      >
        <Stack spacing={1}>
          {messages.map((m, i) => (
            <Box key={i} maxWidth="70%">
              <Paper sx={{ p: 1.5 }}>
                <Typography variant="caption" fontWeight={600}>
                  {m.user}
                </Typography>

                <Typography variant="body2">
                  {m.message}
                </Typography>

                <Typography
                  variant="caption"
                  display="block"
                  textAlign="right"
                  color="text.secondary"
                >
                  {new Date(m.sentOn).toLocaleTimeString()}
                </Typography>
              </Paper>
            </Box>
          ))}
        </Stack>
        <div ref={bottomRef} />
      </Box>

      {/* INPUT */}
      <Box
        p={2}
        borderTop="1px solid #e0e0e0"
        display="flex"
        gap={1}
      >
        <TextField
          fullWidth
          size="small"
          placeholder="Type a message..."
          value={text}
          onChange={(e) => setText(e.target.value)}
          onKeyDown={(e) => e.key === "Enter" && send()}
        />
        <IconButton color="primary" onClick={send}>
          <SendIcon />
        </IconButton>
      </Box>
    </Paper>
  );
}
