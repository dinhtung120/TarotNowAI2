import RegisterPage from '@/features/auth/register/RegisterPage';
import { redirectAuthenticatedAuthEntry } from '@/app/_shared/server/auth/redirectAuthenticatedAuthEntry';

export default async function RegisterRoutePage({
 params,
}: {
 params: Promise<{ locale: string }>;
}) {
 const { locale } = await params;
 await redirectAuthenticatedAuthEntry({ locale });

 return <RegisterPage />;
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
