'use client';

import { useEffect, useState } from 'react';
import { useForm, useWatch } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Mail, CheckCircle2, KeyRound, RefreshCcw } from 'lucide-react';
import { verifyEmailAction, resendVerificationEmailAction } from '@/actions/authActions';
import { Link } from '@/i18n/routing';
import toast from 'react-hot-toast';

import AuthLayout from '@/components/layout/AuthLayout';
import { Input, Button, GlassCard } from '@/components/ui';

const verifySchema = z.object({
    email: z.string().email('Invalid email address'),
    otpCode: z.string().length(6, 'OTP must be exactly 6 digits').regex(/^\d+$/, 'OTP must contain only numbers'),
});

type VerifyFormValues = z.infer<typeof verifySchema>;

export default function VerifyEmailPage() {
    const [errorMsg, setErrorMsg] = useState('');
    const [success, setSuccess] = useState(false);
    const [resendTimer, setResendTimer] = useState(0);
    const [isResending, setIsResending] = useState(false);

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
            toast.error('Vui lòng nhập email hợp lệ để gửi lại mã.');
            return;
        }

        setIsResending(true);
        try {
            const result = await resendVerificationEmailAction(emailWatch);
            if (result.success) {
                toast.success('Mã OTP mới đã được gửi vào email của bạn.');
                setResendTimer(60); // Đếm ngược 60 giây
            } else {
                toast.error(result.error || 'Gửi lại mã thất bại.');
            }
        } catch {
            toast.error('Lỗi kết nối mạng.');
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
            setErrorMsg('An unexpected error occurred.');
        }
    };

    if (success) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-[var(--bg-void)] relative overflow-hidden font-sans">
                {/* Decorative */}
                <div className="absolute top-[20%] right-[30%] w-96 h-96 bg-[var(--success-bg)] rounded-full mix-blend-screen filter blur-[120px] opacity-40 animate-pulse" />
                
                <GlassCard className="relative z-10 w-full max-w-md p-10 text-center animate-in zoom-in-95 duration-700">
                    <div className="w-20 h-20 bg-[var(--success-bg)] rounded-2xl flex items-center justify-center mx-auto mb-6 shadow-[0_0_30px_var(--success)] animate-pulse">
                        <CheckCircle2 className="w-10 h-10 text-[var(--success)]" />
                    </div>
                    <h2 className="text-3xl font-black italic tracking-tighter text-white mb-4 uppercase">Email Verified</h2>
                    <p className="text-zinc-400 font-medium mb-8 leading-relaxed">
                        Your account is now active. You may proceed to login to the mystical realm.
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
        <AuthLayout 
            title="Verify Email" 
            subtitle="Enter the OTP sent to your email"
        >
            {errorMsg && (
                <div className="mb-6 p-4 rounded-xl bg-red-500/10 border border-red-500/20 text-red-400 text-sm animate-in fade-in slide-in-from-top-2 flex items-center gap-3">
                    <div className="w-1.5 h-1.5 rounded-full bg-red-500 animate-pulse" />
                    {errorMsg}
                </div>
            )}

            <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
                <Input
                    label="Email"
                    type="email"
                    leftIcon={<Mail className="w-5 h-5" />}
                    placeholder="seeker@tarotnow.com"
                    error={errors.email?.message}
                    {...register('email')}
                />

                <Input
                    label="Mã OTP (6 số)"
                    type="text"
                    leftIcon={<KeyRound className="w-5 h-5" />}
                    placeholder="123456"
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
                        Confirm Verification
                    </Button>
                </div>
            </form>

            <div className="text-center mt-6">
                <button
                    onClick={handleResendOtp}
                    disabled={resendTimer > 0 || isResending}
                    className="text-[10px] font-black uppercase tracking-widest text-zinc-500 hover:text-white transition-all flex items-center justify-center gap-2 mx-auto disabled:opacity-50 disabled:cursor-not-allowed group"
                >
                    <RefreshCcw className={`w-3.5 h-3.5 ${isResending ? 'animate-spin' : 'group-hover:rotate-180 transition-transform duration-500'}`} />
                    {resendTimer > 0 ? `Gửi lại mã (${resendTimer}s)` : 'Gửi lại mã OTP'}
                </button>
            </div>
        </AuthLayout>
    );
}
