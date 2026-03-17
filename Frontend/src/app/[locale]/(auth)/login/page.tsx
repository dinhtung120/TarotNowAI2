'use client';

import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Mail, Lock } from 'lucide-react';
import { loginAction } from '@/actions/authActions';
import { useAuthStore } from '@/store/authStore';
import { Link } from '@/i18n/routing';

import AuthLayout from '@/components/layout/AuthLayout';
import { Input, Button } from '@/components/ui';

const loginSchema = z.object({
    emailOrUsername: z.string().min(1, 'Email or Username is required'),
    password: z.string().min(1, 'Password is required'),
    rememberMe: z.boolean().optional(),
});

type LoginFormValues = z.infer<typeof loginSchema>;

export default function LoginPage() {
    const setAuth = useAuthStore((state) => state.setAuth);
    const [errorMsg, setErrorMsg] = useState('');

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
                window.location.assign('/'); // Hard redirect để xoá Next.js cache
            }
        } catch {
            setErrorMsg('An unexpected error occurred.');
        }
    };

    return (
        <AuthLayout 
            title="Welcome Back" 
            subtitle="Enter your credentials to access your psychic realm"
        >
            {errorMsg && (
                <div className="mb-6 p-4 rounded-xl bg-red-500/10 border border-red-500/20 text-red-400 text-sm animate-in fade-in slide-in-from-top-2 flex items-center gap-3">
                    <div className="w-1.5 h-1.5 rounded-full bg-red-500 animate-pulse" />
                    {errorMsg}
                </div>
            )}

            <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
                <Input
                    label="Email or Username"
                    leftIcon={<Mail className="w-5 h-5" />}
                    placeholder="seeker@tarotnow.com"
                    error={errors.emailOrUsername?.message}
                    {...register('emailOrUsername')}
                />

                <div className="space-y-1">
                    <Input
                        label="Password"
                        type="password"
                        leftIcon={<Lock className="w-5 h-5" />}
                        placeholder="••••••••"
                        error={errors.password?.message}
                        {...register('password')}
                    />
                    <div className="flex justify-end pt-1">
                        <Link href="/forgot-password" className="text-[11px] font-bold text-[var(--purple-accent)] hover:text-white transition-colors uppercase tracking-widest">
                            Forgot password?
                        </Link>
                    </div>
                </div>

                <div className="flex items-center ml-1 py-1">
                    <label className="flex items-start gap-3 cursor-pointer group">
                        <div className="relative flex items-center justify-center mt-0.5">
                            <input
                                type="checkbox"
                                {...register('rememberMe')}
                                className="peer appearance-none w-4 h-4 border border-[var(--purple-accent)]/50 rounded bg-black/30 checked:bg-[var(--purple-accent)] transition-all cursor-pointer"
                            />
                            <svg className="absolute w-2.5 h-2.5 text-black pointer-events-none opacity-0 peer-checked:opacity-100 transition-opacity" viewBox="0 0 14 10" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path d="M1 5L5 9L13 1" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round" />
                            </svg>
                        </div>
                        <span className="text-sm font-medium text-zinc-400 group-hover:text-zinc-200 transition-colors select-none">
                            Remember me
                        </span>
                    </label>
                </div>

                <Button
                    type="submit"
                    variant="brand"
                    size="lg"
                    fullWidth
                    isLoading={isSubmitting}
                    className="mt-2"
                >
                    Reveal Your Destiny
                </Button>
            </form>

            <p className="mt-8 text-center text-sm text-zinc-500 font-medium">
                New to the realm?{' '}
                <Link href="/register" className="text-[var(--purple-accent)] font-bold hover:text-white transition-colors">
                    Create an account
                </Link>
            </p>
        </AuthLayout>
    );
}
