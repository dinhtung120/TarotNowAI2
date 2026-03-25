import { describe, expect, it } from 'vitest';
import {
 createForgotPasswordSchema,
 createLoginSchema,
 createRegisterSchema,
 createResetPasswordSchema,
 createVerifyEmailSchema,
} from '@/features/auth/domain/schemas';

const t = (key: string) => key;

describe('auth schemas', () => {
 it('validates login schema', () => {
  const schema = createLoginSchema(t);
  expect(schema.safeParse({ emailOrUsername: 'demo', password: 'secret' }).success).toBe(true);
  expect(schema.safeParse({ emailOrUsername: '', password: 'secret' }).success).toBe(false);
 });

 it('validates register password complexity and confirmation', () => {
  const schema = createRegisterSchema(t);

  const valid = schema.safeParse({
   email: 'demo@example.com',
   username: 'reader_demo',
   password: 'Strong@123',
   confirmPassword: 'Strong@123',
   displayName: 'Reader Demo',
   dateOfBirth: '1998-01-01',
   hasConsented: true,
  });
  expect(valid.success).toBe(true);

  const invalid = schema.safeParse({
   email: 'demo@example.com',
   username: 'reader_demo',
   password: 'weakpass',
   confirmPassword: 'weakpass',
   displayName: 'Reader Demo',
   dateOfBirth: '1998-01-01',
   hasConsented: true,
  });
  expect(invalid.success).toBe(false);
 });

 it('validates OTP formats for verify/reset password', () => {
  const verifySchema = createVerifyEmailSchema(t);
  const resetSchema = createResetPasswordSchema(t);

  expect(verifySchema.safeParse({ email: 'x@y.com', otpCode: '123456' }).success).toBe(true);
  expect(verifySchema.safeParse({ email: 'x@y.com', otpCode: '12ab56' }).success).toBe(false);

  expect(
   resetSchema.safeParse({
    email: 'x@y.com',
    otpCode: '654321',
    newPassword: 'Valid@123',
   }).success
  ).toBe(true);
  expect(
   resetSchema.safeParse({
    email: 'x@y.com',
    otpCode: '654321',
    newPassword: 'invalid',
   }).success
  ).toBe(false);
 });

 it('validates forgot password email', () => {
  const schema = createForgotPasswordSchema(t);
  expect(schema.safeParse({ email: 'demo@example.com' }).success).toBe(true);
  expect(schema.safeParse({ email: 'bad-email' }).success).toBe(false);
 });
});
