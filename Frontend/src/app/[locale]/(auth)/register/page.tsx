import { RegisterPage } from '@/features/auth/public';
import { redirectAuthenticatedAuthEntry } from '@/shared/server/auth/redirectAuthenticatedAuthEntry';

export default async function RegisterRoutePage({
 params,
}: {
 params: Promise<{ locale: string }>;
}) {
 const { locale } = await params;
 await redirectAuthenticatedAuthEntry({ locale });

 return <RegisterPage />;
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
