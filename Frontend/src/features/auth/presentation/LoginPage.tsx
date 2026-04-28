"use client";

import { Mail } from "lucide-react";
import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import { useLoginPage } from "@/features/auth/application/useLoginPage";
import { AuthErrorBanner } from "@/features/auth/presentation/components/AuthErrorBanner";
import { LoginPasswordField } from "@/features/auth/presentation/components/LoginPasswordField";
import { LoginRememberField } from "@/features/auth/presentation/components/LoginRememberField";
import { cn } from "@/lib/utils";
import AuthLayout from "@/shared/components/layout/AuthLayout";
import { Button, Input } from "@/shared/components/ui";

/*
 * Component LoginPage
 * 
 * Mục đích: Quản lý giao diện đăng nhập.
 * Chức năng: Cho phép người dùng nhập thông tin và đăng nhập, hỗ trợ ẩn/hiện mật khẩu.
 * 
 * Tại sao chọn cách này: Đưa trực tiếp logic ẩn/hiện vào page giúp mã nguồn tập trung, 
 * dễ theo dõi cho các chức năng đơn giản như thay đổi kiểu hiển thị input.
 */
export default function LoginPage() {
  const vm = useLoginPage();

  return (
    <AuthLayout title={vm.t("login.title")} subtitle={vm.t("login.subtitle")}>
      <AuthErrorBanner message={vm.errorMsg} />
      <form onSubmit={vm.handleSubmit(vm.onSubmit)} method="post" className={cn("space-y-1")}>
        <Input
          label={vm.t("login.email_or_username_label")}
          leftIcon={<Mail className={cn("h-5", "w-5")} />}
          placeholder={vm.t("login.email_or_username_placeholder")}
          error={vm.errors.emailOrUsername?.message}
          {...vm.register("emailOrUsername")}
        />
        <div className={cn("space-y-1")}>
          <LoginPasswordField
            label={vm.t("login.password_label")}
            placeholder={vm.t("login.password_placeholder")}
            error={vm.errors.password?.message}
            registerField={vm.register("password")}
          />
          <div className={cn("flex", "items-center", "justify-between")}>
            <LoginRememberField label={vm.t("login.remember_me")} register={vm.register("rememberMe")} />
            <Link
              href="/forgot-password"
              className={cn(
                "inline-flex",
                "items-center",
                "px-1",
                "tn-text-11",
                "font-bold",
                "uppercase",
                "tracking-widest",
                "text-violet-400",
                "transition-colors"
              )}
            >
              {vm.t("login.forgot_password")}
            </Link>
          </div>
        </div>

        <Button type="submit" variant="brand" size="lg" fullWidth isLoading={vm.isSubmitting || vm.isRedirecting} className={cn("mt-1")}>
          {vm.t("login.cta")}
        </Button>
      </form>
      <p className={cn("mt-4", "text-center", "text-sm", "font-medium", "tn-text-muted")}>
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
            "transition-colors"
          )}
        >
          {vm.t("login.footer_link")}
        </Link>
      </p>
    </AuthLayout>
  );
}
