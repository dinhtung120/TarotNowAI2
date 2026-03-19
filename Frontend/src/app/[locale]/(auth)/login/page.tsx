/*
 * ===================================================================
 * FILE: login/page.tsx (Đăng Nhập)
 * BỐI CẢNH (CONTEXT):
 *   Giao diện đăng nhập chính. Kết nối với zustand store (useAuthStore)
 *   để lưu trạng thái xác thực trên Client.
 * 
 * BẢO MẬT & UX:
 *   - Gọi loginAction (Server Action) để verify thông tin. Token được Set-Cookie
 *     ở Backend, frontend chỉ giữ access token tạm thời trong bộ nhớ State.
 *   - Tính năng "Remember Me" lưu tài khoản vào LocalStorage.
 * ===================================================================
 */
'use client';

import { useMemo, useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Mail, Lock } from 'lucide-react';
import { loginAction } from '@/actions/authActions';
import { useAuthStore } from '@/store/authStore';
import { Link, useRouter } from '@/i18n/routing';
import { useTranslations } from 'next-intl';

import AuthLayout from '@/components/layout/AuthLayout';
import { Input, Button } from '@/components/ui';

type LoginFormValues = {
 emailOrUsername: string;
 password: string;
 rememberMe?: boolean;
};

export default function LoginPage() {
 const setAuth = useAuthStore((state) => state.setAuth);
 const [errorMsg, setErrorMsg] = useState('');
 const [isRedirecting, setIsRedirecting] = useState(false);
 const t = useTranslations('Auth');
 const router = useRouter();

 const loginSchema = useMemo(
 () =>
 z.object({
 emailOrUsername: z.string().min(1, t('validation.email_or_username_required')),
 password: z.string().min(1, t('validation.password_required')),
 rememberMe: z.boolean().optional(),
 }),
 [t],
 );

 const {
 register,
 handleSubmit,
 setValue,
 formState: { errors, isSubmitting },
 } = useForm<LoginFormValues>({
 resolver: zodResolver(loginSchema),
 defaultValues: {
 rememberMe: false,
 }
 });

 useEffect(() => {
 // Khôi phục email nếu người dùng đã tích Remember me lần trước
 const savedEmail = localStorage.getItem('tarot_remembered_email');
 if (savedEmail) {
 setValue('emailOrUsername', savedEmail);
 setValue('rememberMe', true);
 }
 }, [setValue]);

 useEffect(() => {
  router.prefetch('/');
  router.prefetch('/reading');
 }, [router]);

 const onSubmit = async (data: LoginFormValues) => {
 setErrorMsg('');

 // Xử lý lưu hoặc xóa ghi nhớ đăng nhập
 if (data.rememberMe) {
 localStorage.setItem('tarot_remembered_email', data.emailOrUsername);
 } else {
 localStorage.removeItem('tarot_remembered_email');
 }

 try {
 const result = await loginAction(data);
 if (result.error) {
 setErrorMsg(result.error);
 return;
 }
			 if (result.success && result.data) {
			 // Lưu local Zustand
			 setAuth(result.data.user, result.data.accessToken);
			 setIsRedirecting(true);
			 router.replace("/");
			 }
		 } catch {
		 setErrorMsg(t('login.error_unexpected'));
		 setIsRedirecting(false);
		 }
		 };

 return (
	 <AuthLayout title={t('login.title')} subtitle={t('login.subtitle')}
	 >
 {errorMsg && (
 <div className="mb-6 p-4 rounded-xl bg-[var(--danger)]/10 border border-[var(--danger)]/20 text-[var(--danger)] text-sm animate-in fade-in slide-in-from-top-2 flex items-center gap-3">
 <div className="w-1.5 h-1.5 rounded-full bg-[var(--danger)] animate-pulse" />
 {errorMsg}
 </div>
 )}

	 <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
	 <Input
	 label={t('login.email_or_username_label')}
	 leftIcon={<Mail className="w-5 h-5" />}
	 placeholder={t('login.email_or_username_placeholder')}
	 error={errors.emailOrUsername?.message}
	 {...register('emailOrUsername')}
	 />

 <div className="space-y-1">
	 <Input
	 label={t('login.password_label')}
	 type="password"
	 leftIcon={<Lock className="w-5 h-5" />}
	 placeholder={t('login.password_placeholder')}
	 error={errors.password?.message}
	 {...register('password')}
	 />
 <div className="flex justify-end pt-1">
	 <Link href="/forgot-password" className="inline-flex items-center min-h-11 px-1 text-[11px] font-bold text-[var(--purple-accent)] hover:tn-text-primary transition-colors uppercase tracking-widest">
	 {t('login.forgot_password')}
	 </Link>
 </div>
 </div>

 <div className="flex items-center ml-1 py-1">
 <label className="flex items-start gap-3 cursor-pointer group">
 <div className="relative flex items-center justify-center mt-0.5">
 <input
 type="checkbox"
 {...register('rememberMe')}
 className="peer appearance-none w-4 h-4 border border-[var(--purple-accent)]/50 rounded tn-overlay-soft checked:bg-[var(--purple-accent)] transition-all cursor-pointer"
 />
 <svg className="absolute w-2.5 h-2.5 tn-text-ink pointer-events-none opacity-0 peer-checked:opacity-100 transition-opacity" viewBox="0 0 14 10" fill="none" xmlns="http://www.w3.org/2000/svg">
 <path d="M1 5L5 9L13 1" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round" />
 </svg>
 </div>
	 <span className="text-sm font-medium tn-text-secondary group-hover:tn-text-secondary transition-colors select-none">
	 {t('login.remember_me')}
	 </span>
 </label>
 </div>

	 <Button
 type="submit"
 variant="brand"
 size="lg"
 fullWidth
 isLoading={isSubmitting || isRedirecting}
	 className="mt-2"
	 >
	 {t('login.cta')}
	 </Button>
 </form>

	 <p className="mt-8 text-center text-sm tn-text-muted font-medium">
	 {t('login.footer_prompt')}{' '}
	 <Link href="/register" className="inline-flex items-center min-h-11 px-1 text-[var(--purple-accent)] font-bold hover:tn-text-primary transition-colors">
	 {t('login.footer_link')}
	 </Link>
	 </p>
 </AuthLayout>
 );
}
