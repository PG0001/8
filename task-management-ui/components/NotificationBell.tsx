"use client";

import { useEffect, useState, useRef} from "react";
import api from "@/lib/api";
import { createNotificationHubConnection } from "@/lib/signalr";
import { getToken } from "@/lib/auth";
import {
  Box,
  Popover,
  Typography,
  List,
  ListItem,
  ListItemText,
  ListItemButton,
  Divider,
} from "@mui/material";

type Notification = {
  id: number;
  title: string;
  message: string;
  isRead: boolean;
  createdOn: string;
};

export default function NotificationBell() {
  const [count, setCount] = useState(0);
  const [items, setItems] = useState<Notification[]>([]);
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
 const connectionRef = useRef<any>(null);
const startedRef = useRef(false);

useEffect(() => {
  const token = getToken();
  if (!token) return;
  if (startedRef.current) return;

  const connection = createNotificationHubConnection();
  connectionRef.current = connection;

  // initial unread count
  api.get("/notifications").then((res) => {
    const unread = res.data.filter((n: any) => !n.isRead);
    setCount(unread.length);
  });

  const onNotification = () => {
    setCount((c) => c + 1);
  };

  connection.on("NotificationReceived", onNotification);

  connection
    .start()
    .then(() => {
      console.log("🔔 SignalR connected");
      startedRef.current = true; // ✅ IMPORTANT
    })
    .catch((err) => {
      console.error("SignalR start error:", err);
    });

  return () => {
    if (startedRef.current && connection.state === "Connected") {
      connection.off("NotificationReceived", onNotification);
      connection.stop();
      startedRef.current = false;
    }
  };
}, []);

  // ---------------- OPEN DROPDOWN ----------------
  const openDropdown = async (
    e: React.MouseEvent<HTMLElement>
  ) => {
    setAnchorEl(e.currentTarget);

    // load notifications
    const res = await api.get("/notifications");
    setItems(res.data);

  };

  const closeDropdown = () => {
    setAnchorEl(null);
  };

  return (
    <>
      {/* 🔔 Bell */}
      <Box
        onClick={openDropdown}
        sx={{
          position: "relative",
          cursor: "pointer",
          fontSize: 18,
        }}
        title="Notifications"
      >
        🔔
        {count > 0 && (
          <Box
            sx={{
              position: "absolute",
              top: -6,
              right: -6,
              background: "red",
              color: "white",
              borderRadius: "50%",
              px: "6px",
              fontSize: 12,
              minWidth: 18,
              textAlign: "center",
            }}
          >
            {count}
          </Box>
        )}
      </Box>

      {/* 📬 Dropdown */}
      <Popover
        open={Boolean(anchorEl)}
        anchorEl={anchorEl}
        onClose={closeDropdown}
        anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
        transformOrigin={{ vertical: "top", horizontal: "right" }}
        disableAutoFocus
        disableEnforceFocus
      >
        <Box sx={{ width: 320, maxHeight: 360, overflowY: "auto" }}>
          <Typography fontWeight={600} p={2}>
            Notifications
          </Typography>

          <Divider />

          {items.length === 0 && (
            <Typography
              variant="body2"
              color="text.secondary"
              p={2}
            >
              No notifications
            </Typography>
          )}

          <List>
         {items.map((n) => (
         <ListItem key={n.id} disablePadding>
  <ListItemButton
    sx={{
      bgcolor: n.isRead ? "transparent" : "#f5faff",
    }}
    onClick={async () => {
      if (!n.isRead) {
        await api.patch(`/notifications/${n.id}/read`);

        setItems((prev) =>
          prev.map((x) =>
            x.id === n.id ? { ...x, isRead: true } : x
          )
        );

        setCount((c) => (c > 0 ? c - 1 : 0));
      }
    }}
  >
    <ListItemText
      primary={n.title}
      secondary={
        <>
          <Typography component="span" variant="body2" color="text.secondary">
            {n.message}
          </Typography>
          <Typography component="span" variant="caption" color="text.secondary">
            {new Date(n.createdOn).toLocaleString()}
          </Typography>
        </>
      }
    />
  </ListItemButton>
</ListItem>

  ))}
</List>

        </Box>
      </Popover>
    </>
  );
}
