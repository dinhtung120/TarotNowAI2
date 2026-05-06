"use client";

import { useLoginPage } from "@/features/auth/login/useLoginPage";
import LoginForm from "@/features/auth/login/LoginForm";
import AuthLayout from "@/features/auth/shared/app-shell/layout/AuthLayout";

export default function LoginPage() {
 const vm = useLoginPage();

 return (
  <AuthLayout title={vm.t("login.title")} subtitle={vm.t("login.subtitle")}>
   <LoginForm {...vm} />
  </AuthLayout>
 );
}
