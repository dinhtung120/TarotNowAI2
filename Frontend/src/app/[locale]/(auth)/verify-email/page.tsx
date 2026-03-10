'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Mail, CheckCircle2, Loader2, KeyRound } from 'lucide-react';
import { verifyEmailAction } from '@/actions/authActions';
import { Link } from '@/i18n/routing';

const verifySchema = z.object({
    email: z.string().email('Invalid email address'),
    otpCode: z.string().length(6, 'OTP must be exactly 6 digits').regex(/^\d+$/, 'OTP must contain only numbers'),
});

type VerifyFormValues = z.infer<typeof verifySchema>;

export default function VerifyEmailPage() {
    const [errorMsg, setErrorMsg] = useState('');
    const [success, setSuccess] = useState(false);

    const {
        register,
        handleSubmit,
        formState: { errors, isSubmitting },
    } = useForm<VerifyFormValues>({
        resolver: zodResolver(verifySchema),
    });

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
            <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-indigo-900 via-purple-900 to-slate-900 relative overflow-hidden font-sans">
                <div className="relative z-10 w-full max-w-md p-8 bg-white/10 backdrop-blur-xl border border-white/20 shadow-2xl rounded-3xl text-center">
                    <div className="w-20 h-20 bg-green-500/20 rounded-full flex items-center justify-center mx-auto mb-6">
                        <CheckCircle2 className="w-10 h-10 text-green-300" />
                    </div>
                    <h2 className="text-3xl font-bold text-white mb-4">Email Verified</h2>
                    <p className="text-purple-200/80 mb-8">
                        Your account is now active. You may proceed to login to the mystical realm.
                    </p>
                    <Link href="/login" className="inline-flex w-full py-3.5 px-4 bg-gradient-to-r from-purple-500 to-fuchsia-500 hover:from-purple-400 hover:to-fuchsia-400 text-white rounded-2xl font-semibold justify-center items-center transition-all">
                        Return to Login
                    </Link>
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-indigo-900 via-purple-900 to-slate-900 relative overflow-hidden font-sans">
            <div className="absolute top-0 right-0 w-96 h-96 bg-purple-500 rounded-full mix-blend-screen filter blur-[100px] opacity-30 animate-pulse" />
            <div className="absolute bottom-0 left-0 w-96 h-96 bg-fuchsia-500 rounded-full mix-blend-screen filter blur-[100px] opacity-30 animate-pulse delay-1000" />

            <div className="relative z-10 w-full max-w-md p-8 bg-white/10 backdrop-blur-xl border border-white/20 shadow-2xl rounded-3xl">
                <div className="text-center mb-10">
                    <h1 className="text-3xl font-extrabold text-transparent bg-clip-text bg-gradient-to-r from-purple-200 to-fuchsia-200 tracking-tight mb-2">
                        Verify Email
                    </h1>
                    <p className="text-purple-200/80 text-sm">Enter the OTP sent to your email</p>
                </div>

                {errorMsg && (
                    <div className="mb-6 p-4 rounded-xl bg-red-500/20 border border-red-500/50 text-red-200 text-sm">
                        {errorMsg}
                    </div>
                )}

                <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
                    <div className="space-y-1">
                        <label className="text-sm font-medium text-purple-200/90 ml-1">Email</label>
                        <div className="relative">
                            <div className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
                                <Mail className="h-5 w-5 text-purple-300/50" />
                            </div>
                            <input
                                {...register('email')}
                                className="w-full pl-11 pr-4 py-3 bg-black/20 border border-white/10 rounded-2xl focus:ring-2 focus:ring-purple-400 focus:border-transparent text-white placeholder-purple-300/30 transition-all outline-none"
                                placeholder="seeker@tarotnow.com"
                            />
                        </div>
                        {errors.email && <p className="text-red-300 text-xs mt-1 ml-1">{errors.email.message}</p>}
                    </div>

                    <div className="space-y-1">
                        <label className="text-sm font-medium text-purple-200/90 ml-1">Mã OTP (6 số)</label>
                        <div className="relative">
                            <div className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
                                <KeyRound className="h-5 w-5 text-purple-300/50" />
                            </div>
                            <input
                                {...register('otpCode')}
                                maxLength={6}
                                className="w-full pl-11 pr-4 py-3 bg-black/20 border border-white/10 rounded-2xl focus:ring-2 focus:ring-purple-400 focus:border-transparent text-white placeholder-purple-300/30 transition-all outline-none text-center tracking-widest text-xl font-bold"
                                placeholder="123456"
                            />
                        </div>
                        {errors.otpCode && <p className="text-red-300 text-xs mt-1 ml-1">{errors.otpCode.message}</p>}
                    </div>

                    <button
                        type="submit"
                        disabled={isSubmitting}
                        className="w-full py-3.5 px-4 bg-gradient-to-r from-purple-500 to-fuchsia-500 hover:from-purple-400 hover:to-fuchsia-400 active:scale-[0.98] text-white rounded-2xl font-semibold shadow-lg shadow-purple-500/30 transition-all flex justify-center items-center gap-2 disabled:opacity-70 disabled:cursor-not-allowed"
                    >
                        {isSubmitting ? <Loader2 className="w-5 h-5 animate-spin" /> : 'Confirm Verification'}
                    </button>
                </form>
            </div>
        </div>
    );
}
