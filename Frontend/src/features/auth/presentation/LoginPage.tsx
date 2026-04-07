"use client";

import { Lock, Mail } from "lucide-react";
import { Link } from "@/i18n/routing";
import { useLoginPage } from "@/features/auth/application/useLoginPage";
import { AuthErrorBanner } from "@/features/auth/presentation/components/AuthErrorBanner";
import { LoginRememberField } from "@/features/auth/presentation/components/LoginRememberField";
import { cn } from "@/lib/utils";
import AuthLayout from "@/shared/components/layout/AuthLayout";
import { Button, Input } from "@/shared/components/ui";

export default function LoginPage() {
 const vm = useLoginPage();

 return (
  <AuthLayout title={vm.t("login.title")} subtitle={vm.t("login.subtitle")}>
   <AuthErrorBanner message={vm.errorMsg} />
   <form onSubmit={vm.handleSubmit(vm.onSubmit)} method="post" className={cn("space-y-5")}>
    <Input
     label={vm.t("login.email_or_username_label")}
     leftIcon={<Mail className={cn("h-5", "w-5")} />}
     placeholder={vm.t("login.email_or_username_placeholder")}
     error={vm.errors.emailOrUsername?.message}
     {...vm.register("emailOrUsername")}
    />
    <div className={cn("space-y-1")}>
     <Input
      label={vm.t("login.password_label")}
      type="password"
      leftIcon={<Lock className={cn("h-5", "w-5")} />}
      placeholder={vm.t("login.password_placeholder")}
      error={vm.errors.password?.message}
      {...vm.register("password")}
     />
     <div className={cn("flex", "justify-end", "pt-1")}>
      <Link
       href="/forgot-password"
       className={cn(
        "inline-flex",
        "min-h-11",
        "items-center",
        "px-1",
        "text-xs",
        "font-bold",
        "uppercase",
        "tracking-widest",
        "text-violet-400",
        "transition-colors",
       )}
      >
       {vm.t("login.forgot_password")}
      </Link>
     </div>
    </div>
    <LoginRememberField label={vm.t("login.remember_me")} register={vm.register("rememberMe")} />
    <Button type="submit" variant="brand" size="lg" fullWidth isLoading={vm.isSubmitting || vm.isRedirecting} className={cn("mt-2")}>
     {vm.t("login.cta")}
    </Button>
   </form>
   <p className={cn("mt-8", "text-center", "text-sm", "font-medium", "tn-text-muted")}>
    {vm.t("login.footer_prompt")}{" "}
    <Link
     href="/register"
     className={cn(
      "inline-flex",
      "min-h-11",
      "items-center",
      "px-1",
      "font-bold",
      "text-violet-400",
      "transition-colors",
     )}
    >
     {vm.t("login.footer_link")}
    </Link>
   </p>
  </AuthLayout>
 );
}
