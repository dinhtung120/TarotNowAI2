/*
 * ===================================================================
 * FILE: register/page.tsx (Đăng Ký Tài Khoản)
 * BỐI CẢNH (CONTEXT):
 *   Trang đăng ký tài khoản mới cho User.
 * 
 * VALIDATION:
 *   Sử dụng Zod + React Hook Form xác thực gắt gao mật khẩu bảo mật cao
 *   và ràng buộc độ tuổi tối thiểu 16 tuổi.
 * ===================================================================
 */
'use client';

import { useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Mail, Lock, User, AtSign, Calendar, Sparkles } from 'lucide-react';
import { registerAction, resendVerificationEmailAction } from '@/actions/authActions';
import { Link } from '@/i18n/routing';
import { useTranslations } from 'next-intl';

import AuthLayout from '@/components/layout/AuthLayout';
import { Input, Button, GlassCard } from '@/components/ui';

type RegisterFormValues = {
 email: string;
 username: string;
 password: string;
 confirmPassword: string;
 displayName: string;
 dateOfBirth: string;
 hasConsented: boolean;
};

export default function RegisterPage() {
 const [errorMsg, setErrorMsg] = useState('');
 const [success, setSuccess] = useState(false);
 const t = useTranslations('Auth');

 const registerSchema = useMemo(
 () =>
 z.object({
 email: z.string().email(t('validation.email_invalid')),
 username: z.string()
 .min(3, t('validation.username_min'))
 .regex(/^[a-zA-Z0-9_]+$/, t('validation.username_pattern')),
 password: z.string()
 .min(8, t('validation.password_min'))
 .regex(
 /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/,
 t('validation.password_complexity'),
 ),
 confirmPassword: z.string(),
 displayName: z.string().min(1, t('validation.display_name_required')),
 dateOfBirth: z.string().refine((date) => {
 const age = (new Date().getTime() - new Date(date).getTime()) / (365.25 * 24 * 60 * 60 * 1000);
 return age >= 16;
 }, t('validation.age_minimum')),
 hasConsented: z.boolean().refine((val) => val === true, {
 message: t('validation.must_accept_terms'),
 }),
 }).refine((data) => data.password === data.confirmPassword, {
 message: t('validation.passwords_no_match'),
 path: ['confirmPassword'],
 }),
 [t],
 );

 const {
 register,
 handleSubmit,
 formState: { errors, isSubmitting },
 } = useForm<RegisterFormValues>({
 resolver: zodResolver(registerSchema),
 });

 const onSubmit = async (data: RegisterFormValues) => {
 setErrorMsg('');
 try {
 const submitData = {
 email: data.email,
 username: data.username,
 password: data.password,
 displayName: data.displayName,
 dateOfBirth: data.dateOfBirth,
 hasConsented: data.hasConsented
 };
 const result = await registerAction(submitData);

 if (result.error) {
 setErrorMsg(result.error);
 return;
 }
 if (result.success) {
 setSuccess(true);
 // GỌI OTP: Tự động gửi OTP sau khi user đăng ký xong
 resendVerificationEmailAction(data.email).catch(console.error);
 }
 } catch {
 setErrorMsg(t('register.error_unexpected'));
 }
 };

 if (success) {
 return (
 <div className="min-h-dvh flex items-center justify-center bg-[var(--bg-void)] relative overflow-hidden font-sans px-4 py-10">
 {/* Decorative */}
 <div className="absolute top-[20%] right-[30%] w-96 h-96 bg-[var(--success-bg)] rounded-full filter blur-[120px] opacity-40 animate-pulse" />
 <GlassCard className="relative z-10 w-full max-w-md p-10 text-center animate-in zoom-in-95 duration-700">
	 <div className="w-20 h-20 bg-[var(--success-bg)] rounded-2xl flex items-center justify-center mx-auto mb-6 shadow-[0_0_30px_var(--success)] animate-pulse">
	 <Mail className="w-10 h-10 text-[var(--success)]" />
	 </div>
	 <h2 className="text-3xl font-black italic tracking-tighter tn-text-primary mb-4 uppercase">{t('register.success_title')}</h2>
	 <p className="tn-text-secondary font-medium mb-8 leading-relaxed">
	 {t('register.success_desc')}
	 </p>
	 <Link href="/verify-email" tabIndex={-1}>
	 <Button variant="brand" size="lg" fullWidth>
	 {t('register.success_cta')}
	 </Button>
	 </Link>
 </GlassCard>
 </div>
 );
 }

 return (
	 <AuthLayout title={t('register.title')} subtitle={t('register.subtitle')}
	 >
 {errorMsg && (
 <div className="mb-6 p-4 rounded-xl bg-[var(--danger)]/10 border border-[var(--danger)]/20 text-[var(--danger)] text-sm animate-in fade-in slide-in-from-top-2 flex items-center gap-3">
 <div className="w-1.5 h-1.5 rounded-full bg-[var(--danger)] animate-pulse" />
 {errorMsg}
 </div>
 )}

	 <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
	 <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
	 <Input label={t('register.email_label')} type="email" leftIcon={<Mail className="w-4 h-4" />}
	 placeholder={t('register.email_placeholder')} error={errors.email?.message} {...register('email')} />
	 <Input label={t('register.username_label')} type="text" leftIcon={<AtSign className="w-4 h-4" />}
	 placeholder={t('register.username_placeholder')} error={errors.username?.message} {...register('username')} />
	 </div>

	 <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
	 <Input label={t('register.display_name_label')} type="text" leftIcon={<User className="w-4 h-4" />}
	 placeholder={t('register.display_name_placeholder')} error={errors.displayName?.message} {...register('displayName')} />
	 <Input
	 label={t('register.dob_label')}
	 type="date"
	 leftIcon={<Calendar className="w-4 h-4 z-10" />}
	 error={errors.dateOfBirth?.message}
	 {...register('dateOfBirth')}
	 />
	 </div>

	 <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
	 <Input label={t('register.password_label')} type="password" leftIcon={<Lock className="w-4 h-4" />}
	 placeholder={t('register.password_placeholder')} error={errors.password?.message} {...register('password')} />
	 <Input label={t('register.confirm_password_label')} type="password" leftIcon={<Lock className="w-4 h-4" />}
	 placeholder={t('register.confirm_password_placeholder')} error={errors.confirmPassword?.message} {...register('confirmPassword')} />
	 </div>

 <div className="pt-2">
 <label className="flex items-center gap-3 cursor-pointer group p-3 rounded-xl hover:tn-surface border border-transparent hover:tn-border-soft transition-all">
<div className="relative flex items-center justify-center mt-0.5">
<input
type="checkbox"
{...register('hasConsented')}
className="peer appearance-none w-5 h-5 border border-[var(--purple-accent)]/50 rounded-md tn-overlay checked:bg-[var(--purple-accent)] transition-all cursor-pointer"
 />
 <svg className="absolute w-3 h-3 tn-text-ink pointer-events-none opacity-0 peer-checked:opacity-100 transition-opacity" viewBox="0 0 14 10" fill="none" xmlns="http://www.w3.org/2000/svg">
 <path d="M1 5L5 9L13 1" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round" />
 </svg>
 </div>
	 <span className="text-[11px] font-medium tn-text-secondary group-hover:tn-text-secondary transition-colors leading-none whitespace-nowrap">
	 {t('register.consent_prefix')}{' '}
	 <Link href="/legal/tos" className="text-[var(--purple-accent)] hover:underline border-b border-[var(--purple-accent)]/30 pb-0.5 align-middle">{t('register.consent_terms')}</Link>{' '}
	 {t('register.consent_and')}{' '}
	 <Link href="/legal/privacy" className="text-[var(--purple-accent)] hover:underline border-b border-[var(--purple-accent)]/30 pb-0.5 align-middle">{t('register.consent_privacy')}</Link>
	 </span>
</label>
 {errors.hasConsented && <p className="text-[11px] text-[var(--danger)] font-medium mt-1 ml-4">{errors.hasConsented.message}</p>}
 </div>

 <div className="pt-2">
	 <Button
 type="submit"
 variant="brand"
 size="lg"
 fullWidth
 isLoading={isSubmitting}
	 rightIcon={!isSubmitting && <Sparkles className="w-5 h-5 ml-2" />}
	 >
	 {t('register.cta')}
	 </Button>
 </div>
 </form>

	 <p className="mt-8 text-center text-sm tn-text-muted font-medium">
	 {t('register.footer_prompt')}{' '}
	 <Link href="/login" className="inline-flex items-center min-h-11 px-1 text-[var(--purple-accent)] font-bold hover:tn-text-primary transition-colors">
	 {t('register.footer_link')}
	 </Link>
	 </p>
 </AuthLayout>
 );
}
