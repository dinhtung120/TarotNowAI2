"use client";

import { useLoginPage } from "@/features/auth/application/useLoginPage";
import LoginForm from "@/features/auth/presentation/components/LoginForm";
import AuthLayout from "@/shared/components/layout/AuthLayout";

export default function LoginPage() {
 const vm = useLoginPage();

 return (
  <AuthLayout title={vm.t("login.title")} subtitle={vm.t("login.subtitle")}>
   <LoginForm {...vm} />
  </AuthLayout>
 );
}
