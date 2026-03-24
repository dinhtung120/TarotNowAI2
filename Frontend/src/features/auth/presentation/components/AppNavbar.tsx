"use client";

import Navbar from "@/shared/components/common/Navbar";
import { logoutAction } from "@/features/auth/application/actions";

export default function AppNavbar() {
 return <Navbar onLogout={logoutAction} />;
}
