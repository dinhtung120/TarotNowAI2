"use client";

import AuthSessionManager from "@/shared/components/auth/AuthSessionManager";
import {
 logoutAction,
 refreshAccessTokenAction,
} from "@/features/auth/application/actions";

export default function AppAuthSessionManager() {
 return (
  <AuthSessionManager
   logout={logoutAction}
   refreshAccessToken={refreshAccessTokenAction}
  />
 );
}
