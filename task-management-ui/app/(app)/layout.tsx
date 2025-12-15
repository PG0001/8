"use client";

import Navbar from "@/components/Navbar";
import { ThemeProvider, CssBaseline } from "@mui/material";
import { createTheme } from "@mui/material/styles";

const theme = createTheme();

export default function AppLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Navbar />
      <main style={{ padding: 24 }}>
        {children}
      </main>
    </ThemeProvider>
  );
}
