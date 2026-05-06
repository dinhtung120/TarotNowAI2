"use client";

import AuthSessionManager from "@/features/auth/session/components/AuthSessionManager";
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
