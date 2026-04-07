"use client";

import { CheckCircle2 } from "lucide-react";
import { useResetPasswordPage } from "@/features/auth/application/useResetPasswordPage";
import { AuthErrorBanner } from "@/features/auth/presentation/components/AuthErrorBanner";
import { AuthSuccessCard } from "@/features/auth/presentation/components/AuthSuccessCard";
import ResetPasswordForm from "@/features/auth/presentation/components/ResetPasswordForm";
import AuthLayout from "@/shared/components/layout/AuthLayout";

export default function ResetPasswordPage() {
  const {
    t,
    errorMsg,
    success,
    register,
    handleSubmit,
    errors,
    isSubmitting,
    onSubmit,
  } = useResetPasswordPage();

  if (success) {
    return (
      <AuthSuccessCard
        ctaHref="/login"
        ctaLabel={t("reset.success_cta")}
        description={t("reset.success_desc")}
        glowClass="bg-[var(--success-bg)]"
        icon={<CheckCircle2 className="h-10 w-10 text-[var(--success)]" />}
        iconWrapperClass="bg-[var(--success-bg)] shadow-[0_0_30px_var(--success)]"
        title={t("reset.success_title")}
      />
    );
  }

  return (
    <AuthLayout subtitle={t("reset.subtitle")} title={t("reset.title")}>
      <AuthErrorBanner message={errorMsg} />
      <ResetPasswordForm
        errors={errors}
        handleSubmit={handleSubmit}
        isSubmitting={isSubmitting}
        labels={{
          emailLabel: t("reset.email_label"),
          emailPlaceholder: t("reset.email_placeholder"),
          otpLabel: t("reset.otp_label"),
          otpPlaceholder: t("reset.otp_placeholder"),
          passwordLabel: t("reset.password_label"),
          passwordPlaceholder: t("reset.password_placeholder"),
          cta: t("reset.cta"),
        }}
        register={register}
        onSubmit={onSubmit}
      />
    </AuthLayout>
  );
}
