import type {
  FieldErrors,
  UseFormHandleSubmit,
  UseFormRegister,
} from "react-hook-form";
import type { ResetPasswordFormValues } from "@/features/auth/shared/schemas";

export interface ResetPasswordFormLabels {
  emailLabel: string;
  emailPlaceholder: string;
  otpLabel: string;
  otpPlaceholder: string;
  passwordLabel: string;
  passwordPlaceholder: string;
  cta: string;
}

export interface ResetPasswordFormProps {
  labels: ResetPasswordFormLabels;
  errors: FieldErrors<ResetPasswordFormValues>;
  isSubmitting: boolean;
  register: UseFormRegister<ResetPasswordFormValues>;
  handleSubmit: UseFormHandleSubmit<ResetPasswordFormValues>;
  onSubmit: (data: ResetPasswordFormValues) => Promise<void>;
}
