'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Mail, Loader2, ArrowLeft } from 'lucide-react';
import { forgotPasswordAction } from '@/actions/authActions';
import { Link } from '@/i18n/routing';

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
            <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-indigo-900 via-purple-900 to-slate-900 relative overflow-hidden font-sans">
                <div className="relative z-10 w-full max-w-md p-8 bg-white/10 backdrop-blur-xl border border-white/20 shadow-2xl rounded-3xl text-center">
                    <div className="w-20 h-20 bg-blue-500/20 rounded-full flex items-center justify-center mx-auto mb-6">
                        <Mail className="w-10 h-10 text-blue-300" />
                    </div>
                    <h2 className="text-3xl font-bold text-white mb-4">Check Your Mail</h2>
                    <p className="text-purple-200/80 mb-8">
                        If the email is valid, an OTP to reset your password has been sent.
                    </p>
                    <Link href="/reset-password" className="inline-flex w-full py-3.5 px-4 bg-gradient-to-r from-purple-500 to-fuchsia-500 hover:from-purple-400 hover:to-fuchsia-400 text-white rounded-2xl font-semibold justify-center items-center transition-all">
                        Enter Reset Code
                    </Link>
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-indigo-900 via-purple-900 to-slate-900 relative overflow-hidden font-sans">
            <div className="relative z-10 w-full max-w-md p-8 bg-white/10 backdrop-blur-xl border border-white/20 shadow-2xl rounded-3xl">

                <Link href="/login" className="inline-flex items-center text-sm text-purple-300 hover:text-white transition-colors mb-6 group">
                    <ArrowLeft className="w-4 h-4 mr-1 transform group-hover:-translate-x-1 transition-transform" />
                    Back to Login
                </Link>

                <div className="mb-8">
                    <h1 className="text-3xl font-extrabold text-transparent bg-clip-text bg-gradient-to-r from-purple-200 to-fuchsia-200 tracking-tight mb-2">
                        Forgot Password
                    </h1>
                    <p className="text-purple-200/80 text-sm">Enter your email to receive a reset code</p>
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

                    <button
                        type="submit"
                        disabled={isSubmitting}
                        className="w-full py-3.5 px-4 bg-gradient-to-r from-purple-500 to-fuchsia-500 hover:from-purple-400 hover:to-fuchsia-400 active:scale-[0.98] text-white rounded-2xl font-semibold shadow-lg shadow-purple-500/30 transition-all flex justify-center items-center gap-2 disabled:opacity-70 disabled:cursor-not-allowed"
                    >
                        {isSubmitting ? <Loader2 className="w-5 h-5 animate-spin" /> : 'Send Reset Code'}
                    </button>
                </form>
            </div>
        </div>
    );
}
