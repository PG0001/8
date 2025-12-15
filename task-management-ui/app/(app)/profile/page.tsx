"use client";

import { useEffect, useState } from "react";
import api from "@/lib/api";
import ProtectedRoute from "@/components/ProtectedRoute";

import {
  Box,
  Paper,
  Typography,
  TextField,
  Button,
  Alert,
  Stack,
  CircularProgress,
} from "@mui/material";

type Profile = {
  fullName: string;
  email: string;
};

export default function ProfilePage() {
  const [profile, setProfile] = useState<Profile>({
    fullName: "",
    email: "",
  });

  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(true);

  const loadProfile = async () => {
    try {
      const res = await api.get("/users/me");
      setProfile(res.data);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadProfile();
  }, []);

  const updateProfile = async () => {
    setMessage("");
    setError("");
    try {
      await api.put("/users/profile", profile);
      setMessage("Profile updated successfully");
    } catch {
      setError("Failed to update profile");
    }
  };

  const updatePassword = async () => {
    setMessage("");
    setError("");
    const oldPassword = localStorage.getItem("password");
    try {
      await api.put("/auth/change-password", {
        oldPassword: oldPassword,
        newPassword: password,
      });
      setPassword("");
      setMessage("Password updated successfully");
    } catch {
      setError("Failed to update password");
    }
  };

  return (
    <ProtectedRoute>
      <Box maxWidth={700} mx="auto">
        <Typography variant="h5" mb={3}>
          Profile
        </Typography>

        {loading ? (
          <CircularProgress />
        ) : (
          <Stack spacing={3}>
            {/* PROFILE INFO */}
            <Paper sx={{ p: 3 }}>
              <Typography variant="h6" mb={2}>
                Personal Information
              </Typography>

              <Stack spacing={2}>
                <TextField
                  label="Full Name"
                  fullWidth
                  value={profile.fullName}
                  onChange={(e) =>
                    setProfile({
                      ...profile,
                      fullName: e.target.value,
                    })
                  }
                />

                <TextField
                  label="Email"
                  fullWidth
                  disabled
                  value={profile.email}
                />

                <Button
                  variant="contained"
                  onClick={updateProfile}
                  sx={{ alignSelf: "flex-start" }}
                >
                  Save Changes
                </Button>
              </Stack>
            </Paper>

            {/* PASSWORD */}
            <Paper sx={{ p: 3 }}>
              <Typography variant="h6" mb={2}>
                Change Password
              </Typography>

              <Stack spacing={2}>
                <TextField
                  label="New Password"
                  type="password"
                  fullWidth
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />

                <Button
                  variant="contained"
                  color="warning"
                  onClick={updatePassword}
                  sx={{ alignSelf: "flex-start" }}
                >
                  Update Password
                </Button>
              </Stack>
            </Paper>

            {message && (
              <Alert severity="success">{message}</Alert>
            )}
            {error && <Alert severity="error">{error}</Alert>}
          </Stack>
        )}
      </Box>
    </ProtectedRoute>
  );
}
