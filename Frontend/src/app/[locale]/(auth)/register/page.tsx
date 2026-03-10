'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Mail, Lock, User, AtSign, Calendar, Loader2 } from 'lucide-react';
import { registerAction } from '@/actions/authActions';
import { Link } from '@/i18n/routing';

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
            <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-indigo-900 via-purple-900 to-slate-900 relative overflow-hidden font-sans">
                <div className="relative z-10 w-full max-w-md p-8 bg-white/10 backdrop-blur-xl border border-white/20 shadow-2xl rounded-3xl text-center">
                    <div className="w-20 h-20 bg-green-500/20 rounded-full flex items-center justify-center mx-auto mb-6">
                        <Mail className="w-10 h-10 text-green-300" />
                    </div>
                    <h2 className="text-3xl font-bold text-white mb-4">Check Your Inbox</h2>
                    <p className="text-purple-200/80 mb-8">
                        An email with verification OTP has been sent. Please verify your email to unlock your account.
                    </p>
                    <Link href="/verify-email" className="inline-flex w-full py-3.5 px-4 bg-gradient-to-r from-purple-500 to-fuchsia-500 hover:from-purple-400 hover:to-fuchsia-400 text-white rounded-2xl font-semibold justify-center items-center transition-all">
                        Proceed to Verification
                    </Link>
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-indigo-900 via-purple-900 to-slate-900 relative overflow-hidden font-sans py-12">
            {/* Decorative */}
            <div className="absolute top-1/4 left-1/4 w-[500px] h-[500px] bg-indigo-500 rounded-full mix-blend-screen filter blur-[120px] opacity-30 animate-pulse" />
            <div className="absolute bottom-1/4 right-1/4 w-[500px] h-[500px] bg-fuchsia-500 rounded-full mix-blend-screen filter blur-[120px] opacity-30 animate-pulse delay-700" />

            <div className="relative z-10 w-full max-w-xl p-8 bg-white/10 backdrop-blur-xl border border-white/20 shadow-2xl rounded-3xl">
                <div className="text-center mb-8">
                    <h1 className="text-4xl font-extrabold text-transparent bg-clip-text bg-gradient-to-r from-purple-200 to-fuchsia-200 tracking-tight mb-2">
                        Join the Mystic
                    </h1>
                    <p className="text-purple-200/80 text-sm">Create an account to begin your journey</p>
                </div>

                {errorMsg && (
                    <div className="mb-6 p-4 rounded-xl bg-red-500/20 border border-red-500/50 text-red-200 text-sm">
                        {errorMsg}
                    </div>
                )}

                <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                        {/* Left Column */}
                        <div className="space-y-5">
                            <InputField label="Email" icon={Mail} type="email" name="email" register={register} error={errors.email} placeholder="seeker@tarotnow.com" />
                            <InputField label="Username" icon={AtSign} type="text" name="username" register={register} error={errors.username} placeholder="mystic_seeker" />
                            <InputField label="Display Name" icon={User} type="text" name="displayName" register={register} error={errors.displayName} placeholder="Mystic Seeker" />
                            <div className="space-y-1">
                                <label className="text-sm font-medium text-purple-200/90 ml-1">Date of Birth</label>
                                <div className="relative">
                                    <div className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
                                        <Calendar className="h-5 w-5 text-purple-300/50" />
                                    </div>
                                    <input
                                        type="date"
                                        {...register('dateOfBirth')}
                                        className="w-full pl-11 pr-4 py-3 bg-black/20 border border-white/10 rounded-2xl focus:ring-2 focus:ring-purple-400 focus:border-transparent text-white transition-all outline-none"
                                        style={{ colorScheme: 'dark' }} // Force calendar icon to be white in dark bg
                                    />
                                </div>
                                {errors.dateOfBirth && <p className="text-red-300 text-xs mt-1 ml-1">{errors.dateOfBirth.message}</p>}
                            </div>
                        </div>

                        {/* Right Column */}
                        <div className="space-y-5">
                            <InputField label="Password" icon={Lock} type="password" name="password" register={register} error={errors.password} placeholder="••••••••" />
                            <InputField label="Confirm Password" icon={Lock} type="password" name="confirmPassword" register={register} error={errors.confirmPassword} placeholder="••••••••" />

                            <div className="pt-2">
                                <label className="flex items-start gap-3 cursor-pointer group">
                                    <div className="relative flex items-center justify-center mt-1">
                                        <input
                                            type="checkbox"
                                            {...register('hasConsented')}
                                            className="peer appearance-none w-5 h-5 border border-purple-400/50 rounded bg-black/30 checked:bg-purple-500 transition-all"
                                        />
                                        <svg className="absolute w-3 h-3 text-white pointer-events-none opacity-0 peer-checked:opacity-100 transition-opacity" viewBox="0 0 14 10" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M1 5L5 9L13 1" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
                                        </svg>
                                    </div>
                                    <span className="text-sm text-purple-200/80 group-hover:text-purple-200 transition-colors">
                                        I agree to the <Link href="/terms" className="text-purple-300 underline">Terms of Service</Link> and <Link href="/privacy" className="text-purple-300 underline">Privacy Policy</Link>
                                    </span>
                                </label>
                                {errors.hasConsented && <p className="text-red-300 text-xs mt-1 ml-1">{errors.hasConsented.message}</p>}
                            </div>
                        </div>
                    </div>

                    <div className="pt-4">
                        <button
                            type="submit"
                            disabled={isSubmitting}
                            className="w-full py-3.5 px-4 bg-gradient-to-r from-purple-500 to-fuchsia-500 hover:from-purple-400 hover:to-fuchsia-400 active:scale-[0.98] text-white rounded-2xl font-semibold shadow-lg shadow-purple-500/30 transition-all flex justify-center items-center gap-2 group disabled:opacity-70 disabled:cursor-not-allowed"
                        >
                            {isSubmitting ? <Loader2 className="w-5 h-5 animate-spin" /> : 'Embark on the Journey'}
                        </button>
                    </div>
                </form>

                <p className="mt-8 text-center text-sm text-purple-200/60">
                    Already a seeker?{' '}
                    <Link href="/login" className="text-purple-300 font-semibold hover:text-white transition-colors">
                        Return to realm
                    </Link>
                </p>
            </div>
        </div>
    );
}

// Helper Component cho đỡ lặp code
// eslint-disable-next-line @typescript-eslint/no-explicit-any
function InputField({ label, icon: Icon, type, name, register, error, placeholder }: any) {
    return (
        <div className="space-y-1">
            <label className="text-sm font-medium text-purple-200/90 ml-1">{label}</label>
            <div className="relative">
                <div className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
                    <Icon className="h-5 w-5 text-purple-300/50" />
                </div>
                <input
                    type={type}
                    {...register(name)}
                    className="w-full pl-11 pr-4 py-3 bg-black/20 border border-white/10 rounded-2xl focus:ring-2 focus:ring-purple-400 focus:border-transparent text-white placeholder-purple-300/30 transition-all outline-none"
                    placeholder={placeholder}
                />
            </div>
            {error && <p className="text-red-300 text-xs mt-1 ml-1">{error.message}</p>}
        </div>
    );
}
