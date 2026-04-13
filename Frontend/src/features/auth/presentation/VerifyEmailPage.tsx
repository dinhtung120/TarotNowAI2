"use client";

import { CheckCircle2, KeyRound, Mail } from "lucide-react";
import { useVerifyEmailPage } from "@/features/auth/application/useVerifyEmailPage";
import { AuthErrorBanner } from "@/features/auth/presentation/components/AuthErrorBanner";
import { VerifyEmailResendButton } from "@/features/auth/presentation/components/VerifyEmailResendButton";
import { cn } from "@/lib/utils";
import AuthLayout from "@/shared/components/layout/AuthLayout";
import { Button, Input } from "@/shared/components/ui";

export default function VerifyEmailPage() {
 const vm = useVerifyEmailPage();

 return (
  <AuthLayout title={vm.t("verify.title")} subtitle={vm.t("verify.subtitle")}>
   <AuthErrorBanner message={vm.errorMsg} />
   <form onSubmit={vm.handleSubmit(vm.onSubmit)} className={cn("space-y-6")}>
    <Input
     label={vm.t("verify.email_label")}
     type="email"
     leftIcon={<Mail className={cn("h-5", "w-5")} />}
     placeholder={vm.t("verify.email_placeholder")}
     error={vm.errors.email?.message}
     readOnly={vm.isEmailReadonly}
     {...vm.register("email")}
    />
    <Input
     label={vm.t("verify.otp_label")}
     type="text"
     leftIcon={<KeyRound className={cn("h-5", "w-5")} />}
     placeholder={vm.t("verify.otp_placeholder")}
     maxLength={6}
     error={vm.errors.otpCode?.message}
     {...vm.register("otpCode")}
     className={cn("text-center", "text-xl", "font-bold", "tracking-widest")}
    />
    <div className={cn("pt-2")}>
     <Button
      type="submit"
      variant="brand"
      size="lg"
      fullWidth
      isLoading={vm.isSubmitting}
      rightIcon={!vm.isSubmitting ? <CheckCircle2 className={cn("ml-2", "h-5", "w-5")} /> : undefined}
     >
      {vm.t("verify.cta")}
     </Button>
    </div>
   </form>
   <div className={cn("mt-6", "text-center")}>
    <VerifyEmailResendButton
     isResending={vm.isResending}
     resendTimer={vm.resendTimer}
     resendLabel={vm.t("verify.resend")}
     resendWithTimerLabel={(seconds) => vm.t("verify.resend_with_timer", { seconds })}
     onResend={vm.handleResendOtp}
    />
   </div>
  </AuthLayout>
 );
}
