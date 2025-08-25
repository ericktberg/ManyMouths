import React from "react";
import { Redirect } from "expo-router";

// Place this code in your main app entry file (e.g., App.tsx) or a dedicated API setup file.
import { OpenAPI } from '@/src/api-client/core/OpenAPI';

// Set the base URL based on your environment.
// For example, using an environment variable or a constant.
OpenAPI.BASE = 'https://localhost:7173'; // Or process.env.REACT_APP_API_URL;

export default function Index() {
  return <Redirect href="./(tabs)/recipes"/>;
}