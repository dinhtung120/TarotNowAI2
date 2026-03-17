'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Mail, Lock, User, AtSign, Calendar, Sparkles } from 'lucide-react';
import { registerAction } from '@/actions/authActions';
import { Link } from '@/i18n/routing';

import AuthLayout from '@/components/layout/AuthLayout';
import { Input, Button, GlassCard } from '@/components/ui';

const registerSchema = z.object({
    email: z.string().email('Invalid email address'),
    username: z.string().min(3, 'Username must be at least 3 characters').regex(/^[a-zA-Z0-9_]+$/, 'Only letters, numbers and underscores allowed'),
    password: z.string().min(8, 'Password must be at least 8 characters').regex(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/, 'Requires uppercase, lowercase, number and special char'),
    confirmPassword: z.string(),
    displayName: z.string().min(1, 'Display name is required'),
    dateOfBirth: z.string().refine((date) => {
        const age = (new Date().getTime() - new Date(date).getTime()) / (365.25 * 24 * 60 * 60 * 1000);
        return age >= 16;
    }, 'You must be at least 16 years old to use this service.'),
    hasConsented: z.boolean().refine((val) => val === true, {
        message: 'You must accept the terms and conditions',
    }),
}).refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match",
    path: ['confirmPassword'],
});

type RegisterFormValues = z.infer<typeof registerSchema>;

export default function RegisterPage() {
    const [errorMsg, setErrorMsg] = useState('');
    const [success, setSuccess] = useState(false);

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
                        <Mail className="w-10 h-10 text-[var(--success)]" />
                    </div>
                    <h2 className="text-3xl font-black italic tracking-tighter text-white mb-4 uppercase">Check Your Inbox</h2>
                    <p className="text-zinc-400 font-medium mb-8 leading-relaxed">
                        An email with a verification OTP has been sent. Please verify your email to unlock your psychic realm account.
                    </p>
                    <Link href="/verify-email" tabIndex={-1}>
                        <Button variant="brand" size="lg" fullWidth>
                            Proceed to Verification
                        </Button>
                    </Link>
                </GlassCard>
            </div>
        );
    }

    return (
        <AuthLayout 
            title="Join the Mystic" 
            subtitle="Create an account to begin your journey"
        >
            {errorMsg && (
                <div className="mb-6 p-4 rounded-xl bg-red-500/10 border border-red-500/20 text-red-400 text-sm animate-in fade-in slide-in-from-top-2 flex items-center gap-3">
                    <div className="w-1.5 h-1.5 rounded-full bg-red-500 animate-pulse" />
                    {errorMsg}
                </div>
            )}

            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <Input 
                        label="Email" 
                        type="email" 
                        leftIcon={<Mail className="w-4 h-4" />}
                        placeholder="seeker@tarotnow.com" 
                        error={errors.email?.message} 
                        {...register('email')} 
                    />
                    <Input 
                        label="Username" 
                        type="text" 
                        leftIcon={<AtSign className="w-4 h-4" />}
                        placeholder="mystic_seeker" 
                        error={errors.username?.message} 
                        {...register('username')} 
                    />
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <Input 
                        label="Display Name" 
                        type="text" 
                        leftIcon={<User className="w-4 h-4" />}
                        placeholder="Mystic Seeker" 
                        error={errors.displayName?.message} 
                        {...register('displayName')} 
                    />
                    <Input 
                        label="Date of Birth" 
                        type="date"
                        leftIcon={<Calendar className="w-4 h-4 z-10" />}
                        error={errors.dateOfBirth?.message} 
                        {...register('dateOfBirth')} 
                        style={{ colorScheme: 'dark' }} 
                    />
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <Input 
                        label="Password" 
                        type="password" 
                        leftIcon={<Lock className="w-4 h-4" />}
                        placeholder="••••••••" 
                        error={errors.password?.message} 
                        {...register('password')} 
                    />
                    <Input 
                        label="Confirm Password" 
                        type="password" 
                        leftIcon={<Lock className="w-4 h-4" />}
                        placeholder="••••••••" 
                        error={errors.confirmPassword?.message} 
                        {...register('confirmPassword')} 
                    />
                </div>

                <div className="pt-2">
                    <label className="flex items-start gap-4 cursor-pointer group p-3 rounded-xl hover:bg-white/[0.02] border border-transparent hover:border-white/5 transition-all">
                        <div className="relative flex items-center justify-center mt-0.5">
                            <input
                                type="checkbox"
                                {...register('hasConsented')}
                                className="peer appearance-none w-5 h-5 border border-[var(--purple-accent)]/50 rounded-md bg-black/50 checked:bg-[var(--purple-accent)] transition-all cursor-pointer"
                            />
                            <svg className="absolute w-3 h-3 text-black pointer-events-none opacity-0 peer-checked:opacity-100 transition-opacity" viewBox="0 0 14 10" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path d="M1 5L5 9L13 1" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round" />
                            </svg>
                        </div>
                        <span className="text-sm font-medium text-zinc-400 group-hover:text-zinc-200 transition-colors leading-relaxed">
                            I agree to the <Link href="/terms" className="text-[var(--purple-accent)] hover:underline border-b border-[var(--purple-accent)]/30 pb-0.5">Terms of Service</Link> and <Link href="/privacy" className="text-[var(--purple-accent)] hover:underline border-b border-[var(--purple-accent)]/30 pb-0.5">Privacy Policy</Link>
                        </span>
                    </label>
                    {errors.hasConsented && <p className="text-[11px] text-red-400 font-medium mt-1 ml-4">{errors.hasConsented.message}</p>}
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
                        Embark on the Journey
                    </Button>
                </div>
            </form>

            <p className="mt-8 text-center text-sm text-zinc-500 font-medium">
                Already a seeker?{' '}
                <Link href="/login" className="text-[var(--purple-accent)] font-bold hover:text-white transition-colors">
                    Return to realm
                </Link>
            </p>
        </AuthLayout>
    );
}
