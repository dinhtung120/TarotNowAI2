import * as z from 'zod';
import type {
 ForgotPasswordFormValues,
 LoginFormValues,
 RegisterFormValues,
 ResetPasswordFormValues,
 VerifyEmailFormValues,
} from '@/features/auth/shared/schemas';

type AuthTranslator = (
 key: string,
 values?: Record<string, string | number | Date>
) => string;

const PASSWORD_COMPLEXITY_REGEX =
 /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
const OTP_NUMERIC_REGEX = /^\d+$/;
const USERNAME_REGEX = /^[a-zA-Z0-9_]+$/;

export type {
 ForgotPasswordFormValues,
 LoginFormValues,
 RegisterFormValues,
 ResetPasswordFormValues,
 VerifyEmailFormValues,
};

export function createLoginSchema(t: AuthTranslator) {
 return z.object({
  emailOrUsername: z.string().min(1, t('validation.email_or_username_required')),
  password: z.string().min(1, t('validation.password_required')),
  rememberMe: z.boolean().optional(),
 });
}

export function createRegisterSchema(t: AuthTranslator, minimumAge: number) {
 return z
  .object({
   email: z.string().email(t('validation.email_invalid')).max(100),
   username: z
    .string()
    .min(3, t('validation.username_min'))
    .max(32)
    .regex(USERNAME_REGEX, t('validation.username_pattern')),
   password: z
    .string()
    .min(8, t('validation.password_min'))
    .max(100)
    .regex(PASSWORD_COMPLEXITY_REGEX, t('validation.password_complexity')),
   confirmPassword: z.string().max(100),
   displayName: z.string().min(1, t('validation.display_name_required')).max(50),
   dateOfBirth: z.string().refine(
    (date) => {
     if (!date) return false;
     const d = new Date(date);
     if (Number.isNaN(d.getTime())) return false;

     const parts = date.split('-');
     if (parts.length !== 3) return false;
     const y = Number.parseInt(parts[0], 10);
     const m = Number.parseInt(parts[1], 10);
     const day = Number.parseInt(parts[2], 10);

     if (d.getUTCFullYear() !== y || (d.getUTCMonth() + 1) !== m || d.getUTCDate() !== day) {
      return false;
     }

     const age = (Date.now() - d.getTime()) / (365.25 * 24 * 60 * 60 * 1000);
     return age >= minimumAge;
    },
    t('validation.age_minimum', { age: minimumAge }),
   ),
   hasConsented: z.boolean().refine((value) => value === true, {
    message: t('validation.must_accept_terms'),
   }),
  } satisfies z.ZodRawShape)
  .refine((data) => data.password === data.confirmPassword, {
   message: t('validation.passwords_no_match'),
   path: ['confirmPassword'],
  });
}

export function createVerifyEmailSchema(t: AuthTranslator) {
 return z.object({
  email: z.string().email(t('validation.email_invalid')),
  otpCode: z
   .string()
   .length(6, t('validation.otp_length'))
   .regex(OTP_NUMERIC_REGEX, t('validation.otp_numeric')),
 });
}

export function createForgotPasswordSchema(t: AuthTranslator) {
 return z.object({
  email: z.string().email(t('validation.email_invalid')),
 });
}

export function createResetPasswordSchema(t: AuthTranslator) {
 return z.object({
  email: z.string().email(t('validation.email_invalid')),
  otpCode: z
   .string()
   .length(6, t('validation.otp_length'))
   .regex(OTP_NUMERIC_REGEX, t('validation.otp_numeric')),
  newPassword: z
   .string()
   .min(8, t('validation.password_min'))
   .regex(PASSWORD_COMPLEXITY_REGEX, t('validation.password_complexity')),
 });
}
