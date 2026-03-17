'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Mail, ArrowLeft, Send } from 'lucide-react';
import { forgotPasswordAction } from '@/actions/authActions';
import { Link } from '@/i18n/routing';

import AuthLayout from '@/components/layout/AuthLayout';
import { Input, Button, GlassCard } from '@/components/ui';

const forgotSchema = z.object({
    email: z.string().email('Invalid email address'),
});

type ForgotFormValues = z.infer<typeof forgotSchema>;

export default function ForgotPasswordPage() {
    const [errorMsg, setErrorMsg] = useState('');
    const [success, setSuccess] = useState(false);

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
            setErrorMsg('An unexpected error occurred.');
        }
    };

    if (success) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-[var(--bg-void)] relative overflow-hidden font-sans">
                {/* Decorative */}
                <div className="absolute top-[20%] right-[30%] w-96 h-96 bg-[var(--info-bg)] rounded-full mix-blend-screen filter blur-[120px] opacity-40 animate-pulse" />
                
                <GlassCard className="relative z-10 w-full max-w-md p-10 text-center animate-in zoom-in-95 duration-700">
                    <div className="w-20 h-20 bg-[var(--info-bg)] rounded-2xl flex items-center justify-center mx-auto mb-6 shadow-[0_0_30px_var(--info)] animate-pulse">
                        <Mail className="w-10 h-10 text-[var(--info)]" />
                    </div>
                    <h2 className="text-3xl font-black italic tracking-tighter text-white mb-4 uppercase">Check Your Mail</h2>
                    <p className="text-zinc-400 font-medium mb-8 leading-relaxed">
                        If the email is valid, an OTP to reset your password has been sent to your inbox.
                    </p>
                    <Link href="/reset-password" tabIndex={-1}>
                        <Button variant="brand" size="lg" fullWidth>
                            Enter Reset Code
                        </Button>
                    </Link>
                </GlassCard>
            </div>
        );
    }

    return (
        <AuthLayout 
            title="Forgot Password" 
            subtitle="Enter your email to receive a reset code"
        >
            <div className="mb-6 flex justify-center">
                <Link href="/login" className="inline-flex items-center text-xs font-bold text-zinc-400 hover:text-white transition-colors group uppercase tracking-widest">
                    <ArrowLeft className="w-4 h-4 mr-1.5 transform group-hover:-translate-x-1 transition-transform" />
                    Back to Login
                </Link>
            </div>

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

                <Button
                    type="submit"
                    variant="brand"
                    size="lg"
                    fullWidth
                    isLoading={isSubmitting}
                    rightIcon={!isSubmitting && <Send className="w-5 h-5 ml-2" />}
                >
                    Send Reset Code
                </Button>
            </form>
        </AuthLayout>
    );
}
