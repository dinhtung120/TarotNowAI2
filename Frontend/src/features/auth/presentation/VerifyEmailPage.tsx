"use client";

import { CheckCircle2, KeyRound, Mail } from "lucide-react";
import { useVerifyEmailPage } from "@/features/auth/application/useVerifyEmailPage";
import { AuthErrorBanner } from "@/features/auth/presentation/components/AuthErrorBanner";
import { AuthSuccessCard } from "@/features/auth/presentation/components/AuthSuccessCard";
import { VerifyEmailResendButton } from "@/features/auth/presentation/components/VerifyEmailResendButton";
import AuthLayout from "@/shared/components/layout/AuthLayout";
import { Button, Input } from "@/shared/components/ui";

export default function VerifyEmailPage() {
 const vm = useVerifyEmailPage();

 if (vm.success) {
  return <AuthSuccessCard icon={<CheckCircle2 className="w-10 h-10 text-[var(--success)]" />} title={vm.t("verify.success_title")} description={vm.t("verify.success_desc")} ctaHref="/login" ctaLabel={vm.t("verify.success_cta")} glowClass="bg-[var(--success-bg)]" iconWrapperClass="bg-[var(--success-bg)] shadow-[0_0_30px_var(--success)]" />;
 }

 return (
  <AuthLayout title={vm.t("verify.title")} subtitle={vm.t("verify.subtitle")}>
   <AuthErrorBanner message={vm.errorMsg} />
   <form onSubmit={vm.handleSubmit(vm.onSubmit)} className="space-y-6">
    <Input label={vm.t("verify.email_label")} type="email" leftIcon={<Mail className="w-5 h-5" />} placeholder={vm.t("verify.email_placeholder")} error={vm.errors.email?.message} {...vm.register("email")} />
    <Input label={vm.t("verify.otp_label")} type="text" leftIcon={<KeyRound className="w-5 h-5" />} placeholder={vm.t("verify.otp_placeholder")} maxLength={6} error={vm.errors.otpCode?.message} {...vm.register("otpCode")} className="text-center font-bold tracking-widest text-xl" />
    <div className="pt-2"><Button type="submit" variant="brand" size="lg" fullWidth isLoading={vm.isSubmitting} rightIcon={!vm.isSubmitting ? <CheckCircle2 className="w-5 h-5 ml-2" /> : undefined}>{vm.t("verify.cta")}</Button></div>
   </form>
   <div className="text-center mt-6"><VerifyEmailResendButton isResending={vm.isResending} resendTimer={vm.resendTimer} resendLabel={vm.t("verify.resend")} resendWithTimerLabel={(seconds) => vm.t("verify.resend_with_timer", { seconds })} onResend={vm.handleResendOtp} /></div>
  </AuthLayout>
 );
}
