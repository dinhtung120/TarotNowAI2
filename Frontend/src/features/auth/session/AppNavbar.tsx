"use client";

import Navbar from "@/features/app-shell/navigation/navbar/Navbar";
import { logoutAction } from "@/features/auth/shared";

export default function AppNavbar() {
 return <Navbar onLogout={logoutAction} />;
}
