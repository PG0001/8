"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import api from "@/lib/api";

import {
  Box,
  Button,
  Container,
  TextField,
  Typography,
  Paper,
  Alert,
  CircularProgress,
  MenuItem,
} from "@mui/material";

export default function RegisterPage() {
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState("Employee");

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const router = useRouter();

  const register = async () => {
    setError("");
    setLoading(true);

    try {
      await api.post("/auth/register", {
        fullName,
        email,
        password,
        role,
      });

      router.push("/login");
    } catch {
      setError("Registration failed. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container
      maxWidth="sm"
      sx={{
        minHeight: "100vh",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
      }}
    >
      <Paper elevation={3} sx={{ p: 4, width: "100%" }}>
        <Typography variant="h5" align="center" gutterBottom>
          Create Account
        </Typography>

        <Typography
          variant="body2"
          align="center"
          color="text.secondary"
          mb={3}
        >
          Register to start managing tasks
        </Typography>

        <Box display="flex" flexDirection="column" gap={2}>
          <TextField
            label="Full Name"
            fullWidth
            value={fullName}
            onChange={(e) => setFullName(e.target.value)}
          />

          <TextField
            label="Email"
            type="email"
            fullWidth
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />

          <TextField
            label="Password"
            type="password"
            fullWidth
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

          <TextField
            select
            label="Role"
            value={role}
            onChange={(e) => setRole(e.target.value)}
            fullWidth
          >
            <MenuItem value="Employee">Employee</MenuItem>
            <MenuItem value="ProjectManager">
              Project Manager
            </MenuItem>
            <MenuItem value="Admin">Admin</MenuItem>
          </TextField>

          {error && <Alert severity="error">{error}</Alert>}

          <Button
            variant="contained"
            fullWidth
            onClick={register}
            disabled={loading}
            sx={{ height: 44 }}
          >
            {loading ? (
              <CircularProgress size={22} color="inherit" />
            ) : (
              "Register"
            )}
          </Button>

          <Typography
            variant="body2"
            align="center"
            color="text.secondary"
          >
            Already have an account?{" "}
            <span
              style={{
                cursor: "pointer",
                textDecoration: "underline",
              }}
              onClick={() => router.push("/login")}
            >
              Login
            </span>
          </Typography>
        </Box>
      </Paper>
    </Container>
  );
}
