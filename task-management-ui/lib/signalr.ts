import * as signalR from "@microsoft/signalr";
import { getToken } from "./auth";

const API_URL = "https://localhost:7016";

export function createTaskHubConnection() {
  return new signalR.HubConnectionBuilder()
    .withUrl(`${API_URL}/hubs/tasks`, {
      accessTokenFactory: () => getToken() || "",
    })
    .withAutomaticReconnect()
    .build();
}

export function createNotificationHubConnection() {
  return new signalR.HubConnectionBuilder()
    .withUrl(`${API_URL}/hubs/notifications`, {
      accessTokenFactory: () => getToken() || "",
    })
    .withAutomaticReconnect()
    .build();
}

export function createChatHubConnection() {
  return new signalR.HubConnectionBuilder()
    .withUrl(`${API_URL}/hubs/chat`, {
      accessTokenFactory: () => getToken() || "",
    })
    .withAutomaticReconnect()
    .build();
}
