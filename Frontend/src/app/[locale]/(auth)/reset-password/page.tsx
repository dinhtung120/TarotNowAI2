'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Mail, Lock, KeyRound, CheckCircle2 } from 'lucide-react';
import { resetPasswordAction } from '@/actions/authActions';
import { Link } from '@/i18n/routing';

import AuthLayout from '@/components/layout/AuthLayout';
import { Input, Button, GlassCard } from '@/components/ui';

const resetSchema = z.object({
 email: z.string().email('Invalid email address'),
 otpCode: z.string().length(6, 'OTP must be exactly 6 digits').regex(/^\d+$/, 'OTP must contain only numbers'),
 newPassword: z.string().min(8, 'Password must be at least 8 characters').regex(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/, 'Requires uppercase, lowercase, number and special char'),
});

type ResetFormValues = z.infer<typeof resetSchema>;

export default function ResetPasswordPage() {
 const [errorMsg, setErrorMsg] = useState('');
 const [success, setSuccess] = useState(false);

 const {
 register,
 handleSubmit,
 formState: { errors, isSubmitting },
 } = useForm<ResetFormValues>({
 resolver: zodResolver(resetSchema),
 });

 const onSubmit = async (data: ResetFormValues) => {
 setErrorMsg('');
 try {
 const result = await resetPasswordAction(data);
 if (result.error) {
 setErrorMsg(result.error);
 return;
 }
 setSuccess(true);
 } catch {
 setErrorMsg('An unexpected error occurred.');
 }
 };

 if (success) {
 return (
 <div className="min-h-screen flex items-center justify-center bg-[var(--bg-void)] relative overflow-hidden font-sans">
 {/* Decorative */}
 <div className="absolute top-[20%] right-[30%] w-96 h-96 bg-[var(--success-bg)] rounded-full filter blur-[120px] opacity-40 animate-pulse" />
 <GlassCard className="relative z-10 w-full max-w-md p-10 text-center animate-in zoom-in-95 duration-700">
 <div className="w-20 h-20 bg-[var(--success-bg)] rounded-2xl flex items-center justify-center mx-auto mb-6 shadow-[0_0_30px_var(--success)] animate-pulse">
 <CheckCircle2 className="w-10 h-10 text-[var(--success)]" />
 </div>
 <h2 className="text-3xl font-black italic tracking-tighter tn-text-primary mb-4 uppercase">Password Reset!</h2>
 <p className="tn-text-secondary font-medium mb-8 leading-relaxed">
 Your password has been successfully updated. All active devices have been logged out.
 </p>
 <Link href="/login" tabIndex={-1}>
 <Button variant="brand" size="lg" fullWidth>
 Return to Login
 </Button>
 </Link>
 </GlassCard>
 </div>
 );
 }

 return (
 <AuthLayout title="Create New Password" subtitle="Enter your OTP and a new secure password"
 >
 {errorMsg && (
 <div className="mb-6 p-4 rounded-xl bg-[var(--danger)]/10 border border-[var(--danger)]/20 text-[var(--danger)] text-sm animate-in fade-in slide-in-from-top-2 flex items-center gap-3">
 <div className="w-1.5 h-1.5 rounded-full bg-[var(--danger)] animate-pulse" />
 {errorMsg}
 </div>
 )}

 <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
 <Input
 label="Email"
 type="email"
 leftIcon={<Mail className="w-5 h-5" />}
 placeholder="seeker@tarotnow.com"
 error={errors.email?.message}
 {...register('email')}
 />

 <Input
 label="Reset OTP Code"
 type="text"
 leftIcon={<KeyRound className="w-5 h-5" />}
 placeholder="123456"
 maxLength={6}
 error={errors.otpCode?.message}
 {...register('otpCode')}
 className="text-center font-bold tracking-widest text-lg"
 />

 <Input
 label="New Password"
 type="password"
 leftIcon={<Lock className="w-5 h-5" />}
 placeholder="••••••••"
 error={errors.newPassword?.message}
 {...register('newPassword')}
 />

 <div className="pt-2">
 <Button
 type="submit"
 variant="brand"
 size="lg"
 fullWidth
 isLoading={isSubmitting}
 rightIcon={!isSubmitting && <Lock className="w-4 h-4 ml-2" />}
 >
 Reset My Password
 </Button>
 </div>
 </form>
 </AuthLayout>
 );
}
