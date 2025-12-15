"use client";

import { useRouter, usePathname } from "next/navigation";
import NotificationBell from "@/components/NotificationBell";
import { clearToken, getUserRole } from "@/lib/auth";
import { useEffect, useState } from "react";

import {
  AppBar,
  Toolbar,
  Typography,
  Button,
  Box,
  IconButton,
  Menu,
  MenuItem,
  Avatar,
} from "@mui/material";

export default function Navbar() {
  const router = useRouter();
  const pathname = usePathname();

  const [role, setRole] = useState<string | null>(null);
  const [mounted, setMounted] = useState(false); // 🔑
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

  useEffect(() => {
    setRole(getUserRole());
    setMounted(true); // 🔑
  }, []);

  if (!mounted) {
    return null; // ✅ prevents hydration mismatch
  }

  const navItems = [
    { label: "Dashboard", path: "/dashboard", roles: ["Admin", "ProjectManager", "Employee"] },
    { label: "Projects", path: "/projects", roles: ["Admin", "ProjectManager"] },
    { label: "My Tasks", path: "/tasks/my", roles: ["Admin", "Employee"] },
  ];

  const isActive = (path: string) =>
    pathname === path || pathname.startsWith(path + "/");

  const logout = () => {
    clearToken();
    router.push("/login");
  };

  return (
    <AppBar position="static" color="default" elevation={1}>
      <Toolbar sx={{ justifyContent: "space-between" }}>
        {/* LEFT */}
        <Box display="flex" alignItems="center" gap={2}>
          <Typography
            variant="h6"
            sx={{ cursor: "pointer" }}
            onClick={() => router.push("/dashboard")}
          >
            TaskManager
          </Typography>

          {navItems
            .filter((item) => role && item.roles.includes(role))
            .map((item) => (
              <Button
                key={item.path}
                onClick={() => router.push(item.path)}
                sx={{
                  fontWeight: isActive(item.path) ? 600 : 400,
                }}
              >
                {item.label}
              </Button>
            ))}
        </Box>

        {/* RIGHT */}
        <Box display="flex" alignItems="center" gap={2}>
          <NotificationBell />

          <IconButton onClick={(e) => setAnchorEl(e.currentTarget)}
            aria-controls={anchorEl ? "menu" : undefined}
  aria-haspopup="true"
  aria-expanded={anchorEl ? "true" : undefined}>
            <Avatar>{role?.[0] ?? "U"}</Avatar>
          </IconButton>

          <Menu
            anchorEl={anchorEl}
            open={Boolean(anchorEl)}
            onClose={() => setAnchorEl(null)}
            MenuListProps={{
    autoFocusItem: false,
  }}
          >
            
            <MenuItem onClick={() => router.push("/profile")}>
              Profile
            </MenuItem>
            <MenuItem onClick={logout}>
              Logout
            </MenuItem>
          </Menu>
        </Box>
      </Toolbar>
    </AppBar>
  );
}
