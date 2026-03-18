'use client';

import { useMemo, useEffect, useState } from 'react';
import { useForm, useWatch } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Mail, CheckCircle2, KeyRound, RefreshCcw } from 'lucide-react';
import { verifyEmailAction, resendVerificationEmailAction } from '@/actions/authActions';
import { Link } from '@/i18n/routing';
import toast from 'react-hot-toast';
import { useTranslations } from 'next-intl';

import AuthLayout from '@/components/layout/AuthLayout';
import { Input, Button, GlassCard } from '@/components/ui';

type VerifyFormValues = {
 email: string;
 otpCode: string;
};

export default function VerifyEmailPage() {
 const [errorMsg, setErrorMsg] = useState('');
 const [success, setSuccess] = useState(false);
 const [resendTimer, setResendTimer] = useState(0);
 const [isResending, setIsResending] = useState(false);
 const t = useTranslations('Auth');

 const verifySchema = useMemo(
 () =>
 z.object({
 email: z.string().email(t('validation.email_invalid')),
 otpCode: z.string()
 .length(6, t('validation.otp_length'))
 .regex(/^\d+$/, t('validation.otp_numeric')),
 }),
 [t],
 );

 const {
 register,
 handleSubmit,
 control,
 formState: { errors, isSubmitting },
 } = useForm<VerifyFormValues>({
 resolver: zodResolver(verifySchema),
 });

 const emailWatch = useWatch({ control, name: 'email' });

 useEffect(() => {
 let interval: NodeJS.Timeout;
 if (resendTimer > 0) {
 interval = setInterval(() => {
 setResendTimer((prev) => prev - 1);
 }, 1000);
 }
 return () => clearInterval(interval);
 }, [resendTimer]);

 const handleResendOtp = async () => {
 if (!emailWatch || !emailWatch.includes('@')) {
 toast.error(t('verify.toast_invalid_email'));
 return;
 }

 setIsResending(true);
 try {
 const result = await resendVerificationEmailAction(emailWatch);
 if (result.success) {
 toast.success(t('verify.toast_resent_success'));
 setResendTimer(60); // Đếm ngược 60 giây
 } else {
 toast.error(result.error || t('verify.toast_resent_fail'));
 }
 } catch {
 toast.error(t('verify.toast_network_error'));
 } finally {
 setIsResending(false);
 }
 };

 const onSubmit = async (data: VerifyFormValues) => {
 setErrorMsg('');
 try {
 const result = await verifyEmailAction(data);

 if (result.error) {
 setErrorMsg(result.error);
 return;
 }
 if (result.success) {
 setSuccess(true);
 }
 } catch {
 setErrorMsg(t('verify.error_unexpected'));
 }
 };

 if (success) {
 return (
 <div className="min-h-dvh flex items-center justify-center bg-[var(--bg-void)] relative overflow-hidden font-sans px-4 py-10">
 {/* Decorative */}
 <div className="absolute top-[20%] right-[30%] w-96 h-96 bg-[var(--success-bg)] rounded-full filter blur-[120px] opacity-40 animate-pulse" />
 <GlassCard className="relative z-10 w-full max-w-md p-10 text-center animate-in zoom-in-95 duration-700">
 <div className="w-20 h-20 bg-[var(--success-bg)] rounded-2xl flex items-center justify-center mx-auto mb-6 shadow-[0_0_30px_var(--success)] animate-pulse">
 <CheckCircle2 className="w-10 h-10 text-[var(--success)]" />
 </div>
	 <h2 className="text-3xl font-black italic tracking-tighter tn-text-primary mb-4 uppercase">{t('verify.success_title')}</h2>
	 <p className="tn-text-secondary font-medium mb-8 leading-relaxed">
	 {t('verify.success_desc')}
	 </p>
	 <Link href="/login" tabIndex={-1}>
	 <Button variant="brand" size="lg" fullWidth>
	 {t('verify.success_cta')}
	 </Button>
	 </Link>
 </GlassCard>
 </div>
 );
 }

 return (
	 <AuthLayout title={t('verify.title')} subtitle={t('verify.subtitle')}
	 >
 {errorMsg && (
 <div className="mb-6 p-4 rounded-xl bg-[var(--danger)]/10 border border-[var(--danger)]/20 text-[var(--danger)] text-sm animate-in fade-in slide-in-from-top-2 flex items-center gap-3">
 <div className="w-1.5 h-1.5 rounded-full bg-[var(--danger)] animate-pulse" />
 {errorMsg}
 </div>
 )}

 <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
	 <Input
	 label={t('verify.email_label')}
	 type="email"
	 leftIcon={<Mail className="w-5 h-5" />}
	 placeholder={t('verify.email_placeholder')}
	 error={errors.email?.message}
	 {...register('email')}
	 />

	 <Input
	 label={t('verify.otp_label')}
	 type="text"
	 leftIcon={<KeyRound className="w-5 h-5" />}
	 placeholder={t('verify.otp_placeholder')}
	 maxLength={6}
	 error={errors.otpCode?.message}
	 {...register('otpCode')}
	 className="text-center font-bold tracking-widest text-xl"
	 />

 <div className="pt-2">
	 <Button
 type="submit"
 variant="brand"
 size="lg"
 fullWidth
 isLoading={isSubmitting}
	 rightIcon={!isSubmitting && <CheckCircle2 className="w-5 h-5 ml-2" />}
	 >
	 {t('verify.cta')}
	 </Button>
 </div>
 </form>

 <div className="text-center mt-6">
	 <button
 onClick={handleResendOtp}
 disabled={resendTimer > 0 || isResending}
	 className="text-[10px] font-black uppercase tracking-widest tn-text-muted hover:tn-text-primary transition-all flex items-center justify-center gap-2 mx-auto disabled:opacity-50 disabled:cursor-not-allowed group min-h-11 px-3 rounded-xl hover:tn-surface-soft"
	 >
 <RefreshCcw className={`w-3.5 h-3.5 ${isResending ? 'animate-spin' : 'group-hover:rotate-180 transition-transform duration-500'}`} />
	 {resendTimer > 0 ? t('verify.resend_with_timer', { seconds: resendTimer }) : t('verify.resend')}
	 </button>
 </div>
 </AuthLayout>
 );
}
