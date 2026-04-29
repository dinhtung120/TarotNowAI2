"use client";

import AuthSessionManager from "@/shared/components/auth/AuthSessionManager";
import { useAuth } from "@/shared/hooks/useAuth";

export default function AppAuthSessionManager() {
 const { logout, refresh } = useAuth();

 return (
  <AuthSessionManager
   logout={logout}
   refreshAccessToken={refresh}
  />
 );
}
