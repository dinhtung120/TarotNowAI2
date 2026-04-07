import { KeyRound, Lock, Mail } from "lucide-react";
import { cn } from "@/lib/utils";
import { Button, Input } from "@/shared/components/ui";
import type { ResetPasswordFormProps } from "@/features/auth/presentation/components/ResetPasswordForm.types";

export default function ResetPasswordForm({
  labels,
  errors,
  isSubmitting,
  register,
  handleSubmit,
  onSubmit,
}: ResetPasswordFormProps) {
  return (
    <form className={cn("space-y-5")} onSubmit={handleSubmit(onSubmit)}>
      <Input
        error={errors.email?.message}
        label={labels.emailLabel}
        leftIcon={<Mail className={cn("h-5", "w-5")} />}
        placeholder={labels.emailPlaceholder}
        type="email"
        {...register("email")}
      />
      <Input
        className={cn("text-center", "text-lg", "font-bold", "tracking-widest")}
        error={errors.otpCode?.message}
        label={labels.otpLabel}
        leftIcon={<KeyRound className={cn("h-5", "w-5")} />}
        maxLength={6}
        placeholder={labels.otpPlaceholder}
        type="text"
        {...register("otpCode")}
      />
      <Input
        error={errors.newPassword?.message}
        label={labels.passwordLabel}
        leftIcon={<Lock className={cn("h-5", "w-5")} />}
        placeholder={labels.passwordPlaceholder}
        type="password"
        {...register("newPassword")}
      />
      <div className={cn("pt-2")}>
        <Button
          fullWidth
          isLoading={isSubmitting}
          rightIcon={!isSubmitting && <Lock className={cn("ml-2", "h-4", "w-4")} />}
          size="lg"
          type="submit"
          variant="brand"
        >
          {labels.cta}
        </Button>
      </div>
    </form>
  );
}
