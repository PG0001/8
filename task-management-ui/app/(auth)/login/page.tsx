"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import api from "@/lib/api";
import { setToken } from "@/lib/auth";
import {
  Box,
  Button,
  Container,
  TextField,
  Typography,
  Paper,
  Alert,
  CircularProgress,
} from "@mui/material";

export default function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const router = useRouter();

  const login = async () => {
    setError("");
    setLoading(true);

    try {
      const res = await api.post("/auth/login", { email, password });
      setToken(res.data.token);
      localStorage.setItem("password", password)
      localStorage.setItem("role", res.data.role)
      router.push("/projects");
    } catch {
      setError("Invalid email or password");
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
          Task Management
        </Typography>

        <Typography
          variant="body2"
          align="center"
          color="text.secondary"
          mb={3}
        >
          Sign in to continue
        </Typography>

        <Box display="flex" flexDirection="column" gap={2}>
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

          {error && <Alert severity="error">{error}</Alert>}

          <Button
            variant="contained"
            fullWidth
            onClick={login}
            disabled={loading}
            sx={{ height: 44 }}
          >
            {loading ? (
              <CircularProgress size={22} color="inherit" />
            ) : (
              "Login"
            )}
          </Button>
        </Box>
      </Paper>
    </Container>
  );
}
