"use client";

import { useEffect, useState } from "react";
import api from "@/lib/api";

import {
  Box,
  Paper,
  Typography,
  Button,
  Stack,
  List,
  ListItem,
  ListItemText,
  IconButton,
  CircularProgress,
  Alert,
} from "@mui/material";

import UploadFileIcon from "@mui/icons-material/UploadFile";
import DownloadIcon from "@mui/icons-material/Download";

type FileItem = {
  id: number;
  fileName: string;
  filePath: string;
  uploadedOn: string;
};

export default function ProjectFiles({
  projectId,
}: {
  projectId: number;
}) {
  const [files, setFiles] = useState<FileItem[]>([]);
  const [file, setFile] = useState<File | null>(null);
  const [uploading, setUploading] = useState(false);
  const [error, setError] = useState("");

  const loadFiles = async () => {
    const res = await api.get(`/files/project/${projectId}`);
    setFiles(res.data);
  };

  useEffect(() => {
    loadFiles();
  }, [projectId]);

  const uploadFile = async () => {
    if (!file) return;

    setError("");
    setUploading(true);

    try {
      const formData = new FormData();
      formData.append("file", file);

      await api.post(`/files/project/${projectId}`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });

      setFile(null);
      loadFiles();
    } catch {
      setError("File upload failed. Please try again.");
    } finally {
      setUploading(false);
    }
  };

  return (
    <Box>
      <Typography variant="h6" mb={2}>
        Project Files
      </Typography>

      {/* UPLOAD */}
      <Paper sx={{ p: 2, mb: 3 }}>
        <Stack direction="row" spacing={2} alignItems="center">
          <Button
            variant="outlined"
            component="label"
            startIcon={<UploadFileIcon />}
          >
            Select File
            <input
              hidden
              type="file"
              onChange={(e) =>
                setFile(e.target.files?.[0] || null)
              }
            />
          </Button>

          <Typography variant="body2">
            {file?.name || "No file selected"}
          </Typography>

          <Button
            variant="contained"
            disabled={!file || uploading}
            onClick={uploadFile}
          >
            {uploading ? (
              <CircularProgress size={20} />
            ) : (
              "Upload"
            )}
          </Button>
        </Stack>

        {error && (
          <Alert severity="error" sx={{ mt: 2 }}>
            {error}
          </Alert>
        )}
      </Paper>

      {/* FILE LIST */}
      <Paper>
        <List>
          {files.length === 0 && (
            <Typography
              variant="body2"
              color="text.secondary"
              sx={{ p: 2 }}
            >
              No files uploaded yet.
            </Typography>
          )}

          {files.map((f) => (
            <ListItem
              key={f.id}
              divider
              secondaryAction={
                <IconButton
                  edge="end"
                  onClick={() =>
                    window.open(
                      f.filePath,
                      "_blank",
                      "noopener,noreferrer"
                    )
                  }
                >
                  <DownloadIcon />
                </IconButton>
              }
            >
              <ListItemText
                primary={f.fileName}
                secondary={`Uploaded on ${new Date(
                  f.uploadedOn
                ).toLocaleDateString()}`}
              />
            </ListItem>
          ))}
        </List>
      </Paper>
    </Box>
  );
}
