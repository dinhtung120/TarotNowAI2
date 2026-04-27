export interface LoginFormValues {
 emailOrUsername: string;
 password: string;
 rememberMe?: boolean;
}

export interface RegisterFormValues {
 email: string;
 username: string;
 password: string;
 confirmPassword: string;
 displayName: string;
 dateOfBirth: string;
 hasConsented: boolean;
}

export interface VerifyEmailFormValues {
 email: string;
 otpCode: string;
}

export interface ForgotPasswordFormValues {
 email: string;
}

export interface ResetPasswordFormValues {
 email: string;
 otpCode: string;
 newPassword: string;
}
