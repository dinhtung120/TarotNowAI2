"use client";

import { useState } from "react";
import { Eye, EyeOff, Lock, Mail } from "lucide-react";
import { Link } from "@/i18n/routing";
import { useLoginPage } from "@/features/auth/application/useLoginPage";
import { AuthErrorBanner } from "@/features/auth/presentation/components/AuthErrorBanner";
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
  // Trạng thái cục bộ để kiểm soát việc hiển thị mật khẩu (password vs text)
  const [showPassword, setShowPassword] = useState(false);

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
          {/* 
            Trường nhập mật khẩu tích hợp nút ẩn/hiện. 
            - type: thay đổi dựa trên showPassword.
            - rightElement: chứa nút bấm chuyển đổi biểu tượng con mắt.
          */}
          <Input
            label={vm.t("login.password_label")}
            type={showPassword ? "text" : "password"}
            leftIcon={<Lock className={cn("h-5", "w-5")} />}
            placeholder={vm.t("login.password_placeholder")}
            error={vm.errors.password?.message}
            rightElement={
              <button
                type="button"
                onClick={() => setShowPassword(!showPassword)}
                className={cn("tn-text-secondary hover:tn-text-primary transition-colors focus:outline-none")}
                aria-label={showPassword ? "Hide password" : "Show password"}
              >
                {showPassword ? <EyeOff className={cn("h-5", "w-5")} /> : <Eye className={cn("h-5", "w-5")} />}
              </button>
            }
            {...vm.register("password")}
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
