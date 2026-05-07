import ForgotPasswordPage from '@/features/auth/recovery/ForgotPasswordPage';
import { redirectAuthenticatedAuthEntry } from '@/app/_shared/server/auth/redirectAuthenticatedAuthEntry';

export default async function ForgotPasswordRoutePage({
 params,
}: {
 params: Promise<{ locale: string }>;
}) {
 const { locale } = await params;
 await redirectAuthenticatedAuthEntry({ locale });

 return <ForgotPasswordPage />;
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
