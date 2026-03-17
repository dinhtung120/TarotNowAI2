'use client';

import { useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Mail, ArrowLeft, Send } from 'lucide-react';
import { forgotPasswordAction } from '@/actions/authActions';
import { Link } from '@/i18n/routing';
import { useTranslations } from 'next-intl';

import AuthLayout from '@/components/layout/AuthLayout';
import { Input, Button, GlassCard } from '@/components/ui';

type ForgotFormValues = { email: string };

export default function ForgotPasswordPage() {
 const [errorMsg, setErrorMsg] = useState('');
 const [success, setSuccess] = useState(false);
 const t = useTranslations('Auth');

 const forgotSchema = useMemo(
 () =>
 z.object({
 email: z.string().email(t('validation.email_invalid')),
 }),
 [t],
 );

 const {
 register,
 handleSubmit,
 formState: { errors, isSubmitting },
 } = useForm<ForgotFormValues>({
 resolver: zodResolver(forgotSchema),
 });

 const onSubmit = async (data: ForgotFormValues) => {
 setErrorMsg('');
 try {
 const result = await forgotPasswordAction(data);
 if (result.error) {
 setErrorMsg(result.error);
 return;
 }
 setSuccess(true);
 } catch {
 setErrorMsg(t('forgot.error_unexpected'));
 }
 };

 if (success) {
 return (
 <div className="min-h-screen flex items-center justify-center bg-[var(--bg-void)] relative overflow-hidden font-sans">
 {/* Decorative */}
 <div className="absolute top-[20%] right-[30%] w-96 h-96 bg-[var(--info-bg)] rounded-full filter blur-[120px] opacity-40 animate-pulse" />
 <GlassCard className="relative z-10 w-full max-w-md p-10 text-center animate-in zoom-in-95 duration-700">
 <div className="w-20 h-20 bg-[var(--info-bg)] rounded-2xl flex items-center justify-center mx-auto mb-6 shadow-[0_0_30px_var(--info)] animate-pulse">
 <Mail className="w-10 h-10 text-[var(--info)]" />
 </div>
	 <h2 className="text-3xl font-black italic tracking-tighter tn-text-primary mb-4 uppercase">{t('forgot.success_title')}</h2>
	 <p className="tn-text-secondary font-medium mb-8 leading-relaxed">
	 {t('forgot.success_desc')}
	 </p>
	 <Link href="/reset-password" tabIndex={-1}>
	 <Button variant="brand" size="lg" fullWidth>
	 {t('forgot.success_cta')}
	 </Button>
	 </Link>
 </GlassCard>
 </div>
 );
 }

 return (
	 <AuthLayout title={t('forgot.title')} subtitle={t('forgot.subtitle')}
	 >
 <div className="mb-6 flex justify-center">
	 <Link href="/login" className="inline-flex items-center text-xs font-bold tn-text-secondary hover:tn-text-primary transition-colors group uppercase tracking-widest">
 <ArrowLeft className="w-4 h-4 mr-1.5 transform group-hover:-translate-x-1 transition-transform" />
	 {t('forgot.back_to_login')}
	 </Link>
 </div>

 {errorMsg && (
 <div className="mb-6 p-4 rounded-xl bg-[var(--danger)]/10 border border-[var(--danger)]/20 text-[var(--danger)] text-sm animate-in fade-in slide-in-from-top-2 flex items-center gap-3">
 <div className="w-1.5 h-1.5 rounded-full bg-[var(--danger)] animate-pulse" />
 {errorMsg}
 </div>
 )}

 <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
	 <Input
	 label={t('forgot.email_label')}
	 type="email"
	 leftIcon={<Mail className="w-5 h-5" />}
	 placeholder={t('forgot.email_placeholder')}
	 error={errors.email?.message}
	 {...register('email')}
	 />

	 <Button
 type="submit"
 variant="brand"
 size="lg"
 fullWidth
 isLoading={isSubmitting}
	 rightIcon={!isSubmitting && <Send className="w-5 h-5 ml-2" />}
	 >
	 {t('forgot.cta')}
	 </Button>
 </form>
 </AuthLayout>
 );
}
