"use client";

import { Mail, Send } from "lucide-react";
import { useForgotPasswordPage } from "@/features/auth/application/useForgotPasswordPage";
import ForgotPasswordBackLink from "@/features/auth/presentation/components/ForgotPasswordBackLink";
import { AuthErrorBanner } from "@/features/auth/presentation/components/AuthErrorBanner";
import { AuthSuccessCard } from "@/features/auth/presentation/components/AuthSuccessCard";
import AuthLayout from "@/shared/components/layout/AuthLayout";
import { Button, Input } from "@/shared/components/ui";

export default function ForgotPasswordPage() {
  const {
    t,
    errorMsg,
    success,
    register,
    handleSubmit,
    errors,
    isSubmitting,
    onSubmit,
  } = useForgotPasswordPage();

  if (success) {
    return (
      <AuthSuccessCard
        ctaHref="/reset-password"
        ctaLabel={t("forgot.success_cta")}
        description={t("forgot.success_desc")}
        glowClass="bg-[var(--info-bg)]"
        icon={<Mail className="h-10 w-10 text-[var(--info)]" />}
        iconWrapperClass="bg-[var(--info-bg)] shadow-[0_0_30px_var(--info)]"
        title={t("forgot.success_title")}
      />
    );
  }

  return (
    <AuthLayout subtitle={t("forgot.subtitle")} title={t("forgot.title")}>
      <ForgotPasswordBackLink label={t("forgot.back_to_login")} />
      <AuthErrorBanner message={errorMsg} />

      <form className="space-y-6" onSubmit={handleSubmit(onSubmit)}>
        <Input
          error={errors.email?.message}
          label={t("forgot.email_label")}
          leftIcon={<Mail className="h-5 w-5" />}
          placeholder={t("forgot.email_placeholder")}
          type="email"
          {...register("email")}
        />
        <Button
          fullWidth
          isLoading={isSubmitting}
          rightIcon={!isSubmitting && <Send className="ml-2 h-5 w-5" />}
          size="lg"
          type="submit"
          variant="brand"
        >
          {t("forgot.cta")}
        </Button>
      </form>
    </AuthLayout>
  );
}
