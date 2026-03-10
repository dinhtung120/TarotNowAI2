'use client';

import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Mail, Lock, Loader2 } from 'lucide-react';
import { loginAction } from '@/actions/authActions';
import { useAuthStore } from '@/store/authStore';
import { Link } from '@/i18n/routing';

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
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-indigo-900 via-purple-900 to-slate-900 relative overflow-hidden font-sans">
            {/* Decorative Orbs */}
            <div className="absolute top-[-10%] left-[-10%] w-96 h-96 bg-purple-500 rounded-full mix-blend-screen filter blur-[100px] opacity-40 animate-pulse" />
            <div className="absolute bottom-[-10%] right-[-10%] w-96 h-96 bg-fuchsia-500 rounded-full mix-blend-screen filter blur-[100px] opacity-40 animate-pulse delay-1000" />

            <div className="relative z-10 w-full max-w-md p-8 bg-white/10 backdrop-blur-xl border border-white/20 shadow-2xl rounded-3xl">
                <div className="text-center mb-10">
                    <h1 className="text-4xl font-extrabold text-transparent bg-clip-text bg-gradient-to-r from-purple-200 to-fuchsia-200 tracking-tight mb-2">
                        Welcome Back
                    </h1>
                    <p className="text-purple-200/80 text-sm">Enter your credentials to access your psychic realm</p>
                </div>

                {errorMsg && (
                    <div className="mb-6 p-4 rounded-xl bg-red-500/20 border border-red-500/50 text-red-200 text-sm animate-in fade-in slide-in-from-top-2">
                        {errorMsg}
                    </div>
                )}

                <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
                    <div className="space-y-1">
                        <label className="text-sm font-medium text-purple-200/90 ml-1">Email or Username</label>
                        <div className="relative">
                            <div className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
                                <Mail className="h-5 w-5 text-purple-300/50" />
                            </div>
                            <input
                                {...register('emailOrUsername')}
                                className="w-full pl-11 pr-4 py-3 bg-black/20 border border-white/10 rounded-2xl focus:ring-2 focus:ring-purple-400 focus:border-transparent text-white placeholder-purple-300/30 transition-all outline-none"
                                placeholder="seeker@tarotnow.com"
                            />
                        </div>
                        {errors.emailOrUsername && <p className="text-red-300 text-xs mt-1 ml-1">{errors.emailOrUsername.message}</p>}
                    </div>

                    <div className="space-y-1">
                        <div className="flex justify-between items-center ml-1">
                            <label className="text-sm font-medium text-purple-200/90">Password</label>
                            <Link href="/forgot-password" className="text-xs text-purple-300 hover:text-white transition-colors">
                                Forgot password?
                            </Link>
                        </div>
                        <div className="relative">
                            <div className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
                                <Lock className="h-5 w-5 text-purple-300/50" />
                            </div>
                            <input
                                type="password"
                                {...register('password')}
                                className="w-full pl-11 pr-4 py-3 bg-black/20 border border-white/10 rounded-2xl focus:ring-2 focus:ring-purple-400 focus:border-transparent text-white placeholder-purple-300/30 transition-all outline-none"
                                placeholder="••••••••"
                            />
                        </div>
                        {errors.password && <p className="text-red-300 text-xs mt-1 ml-1">{errors.password.message}</p>}
                    </div>

                    <div className="flex items-center ml-1">
                        <input
                            type="checkbox"
                            id="rememberMe"
                            {...register('rememberMe')}
                            className="w-4 h-4 rounded border-white/20 bg-black/20 text-purple-500 focus:ring-purple-400 focus:ring-offset-0 transition-colors accent-purple-500 cursor-pointer"
                        />
                        <label htmlFor="rememberMe" className="ml-2 text-sm text-purple-200/80 cursor-pointer select-none">
                            Remember me
                        </label>
                    </div>

                    <button
                        type="submit"
                        disabled={isSubmitting}
                        className="w-full py-3.5 px-4 bg-gradient-to-r from-purple-500 to-fuchsia-500 hover:from-purple-400 hover:to-fuchsia-400 active:scale-[0.98] text-white rounded-2xl font-semibold shadow-lg shadow-purple-500/30 transition-all flex justify-center items-center gap-2 group disabled:opacity-70 disabled:cursor-not-allowed"
                    >
                        {isSubmitting ? <Loader2 className="w-5 h-5 animate-spin" /> : 'Reveal Your Destiny'}
                    </button>
                </form>

                <p className="mt-8 text-center text-sm text-purple-200/60">
                    New to the realm?{' '}
                    <Link href="/register" className="text-purple-300 font-semibold hover:text-white transition-colors">
                        Create an account
                    </Link>
                </p>
            </div>
        </div>
    );
}
