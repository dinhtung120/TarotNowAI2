import ForgotPasswordPage from '@/features/auth/presentation/ForgotPasswordPage';
import { redirectAuthenticatedAuthEntry } from '@/shared/server/auth/redirectAuthenticatedAuthEntry';

export default async function ForgotPasswordRoutePage({
 params,
}: {
 params: Promise<{ locale: string }>;
}) {
 const { locale } = await params;
 await redirectAuthenticatedAuthEntry({ locale });

 return <ForgotPasswordPage />;
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
